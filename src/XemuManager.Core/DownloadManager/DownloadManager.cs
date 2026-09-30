using System.IO.Compression;
using XemuManager.Core.Exceptions.Download;
using XemuManager.Core.ReleaseManager;

namespace XemuManager.Core.DownloadManager;

public class DownloadManager(IGitHubReleaseManager gitHubReleaseManager, HttpClient httpClient) : IDownloadManager
{
    private readonly IGitHubReleaseManager _gitHubReleaseManager = gitHubReleaseManager;
    private readonly HttpClient _httpClient = httpClient;

    public async Task<string> DownloadAsync(
        string owner,
        string repo,
        Func<GitHubAsset, bool> assetFilter,
        string destinationDirectory,
        CancellationToken cancellationToken = default)
    {
        var release = await _gitHubReleaseManager.GetLatestReleaseAsync(owner, repo, assetFilter, cancellationToken)
                      ?? throw new DownloadException(DownloadError.NotFound);

        var asset = release.Assets.FirstOrDefault()
                    ?? throw new DownloadException(DownloadError.NotFound);

        Directory.CreateDirectory(destinationDirectory);
        var path = Path.Combine(destinationDirectory, Path.GetFileName(asset.Name));

        await using var stream = await DownloadAssetAsync(asset.BrowserDownloadUrl, cancellationToken);
        await SaveAsync(stream, path, cancellationToken);
        await Unpack(path, cancellationToken);

        return path;
    }

    private async Task<Stream> DownloadAssetAsync(Uri assetUri, CancellationToken cancellationToken)
    {
        return await _httpClient.GetStreamAsync(assetUri, cancellationToken);
    }

    private static async Task SaveAsync(Stream stream, string path, CancellationToken cancellationToken)
    {
        await using var file = File.Create(path);
        await stream.CopyToAsync(file, cancellationToken);
    }
    
    private static async Task Unpack(string path, CancellationToken cancellationToken)
    {
        var extension = Path.GetExtension(path);

        if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase))
        {
            var destinationDirectory = Path.GetDirectoryName(path)!;

            await Task.Run(
                () => ZipFile.ExtractToDirectory(path, destinationDirectory, overwriteFiles: true),
                cancellationToken);

            File.Delete(path);
        }
        else if (string.Equals(extension, ".AppImage", StringComparison.OrdinalIgnoreCase))
        {
            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(path, File.GetUnixFileMode(path) | UnixFileMode.UserExecute);
            }
        }
        else
        {
            throw new DownloadException(DownloadError.UnexpectedContent);
        }
    }

}
