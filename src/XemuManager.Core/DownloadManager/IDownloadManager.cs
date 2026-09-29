using XemuManager.Core.ReleaseClient;

namespace XemuManager.Core.DownloadManager;

public interface IDownloadManager
{
    public Task DownloadAsync(string owner, string repo, CancellationToken cancellationToken = default);
}