using XemuManager.Core.DownloadManager.ReleaseManager;

namespace XemuManager.Core.DownloadManager.Downloader;

public interface IDownloader
{
    public Task<string> DownloadAsync(
        string owner,
        string repo,
        Func<GitHubAsset, bool> assetFilter,
        string destinationDirectory,
        CancellationToken cancellationToken = default);
}
