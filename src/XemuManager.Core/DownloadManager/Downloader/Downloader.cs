using XemuManager.Core.DownloadManager.Exceptions;
using XemuManager.Core.DownloadManager.FileDownloader;
using XemuManager.Core.DownloadManager.Installation;
using XemuManager.Core.DownloadManager.ReleaseManager;

namespace XemuManager.Core.DownloadManager.Downloader;

public class Downloader(IGitHubReleaseManager gitHubReleaseManager, IFileDownloader fileDownloader) : IDownloader
{
    private readonly IGitHubReleaseManager _gitHubReleaseManager = gitHubReleaseManager;
    private readonly IFileDownloader _fileDownloader = fileDownloader;

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

        await _fileDownloader.DownloadFileAsync(asset.BrowserDownloadUrl, path, cancellationToken);
        await PackageInstaller.InstallAsync(path, destinationDirectory, cancellationToken);

        return destinationDirectory;
    }
}
