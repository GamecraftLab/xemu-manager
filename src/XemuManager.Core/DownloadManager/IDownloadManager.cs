using XemuManager.Core.ReleaseManager;

namespace XemuManager.Core.DownloadManager;

public interface IDownloadManager
{
    public Task<string> DownloadAsync(
        string owner,
        string repo,
        Func<GitHubAsset, bool> assetFilter,
        string destinationDirectory,
        CancellationToken cancellationToken = default);
}
