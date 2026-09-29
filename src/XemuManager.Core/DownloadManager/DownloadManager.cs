using XemuManager.Core.ReleaseClient;

namespace XemuManager.Core.DownloadManager;

public class DownloadManager(IGitHubReleaseClient gitHubReleaseClient) : IDownloadManager
{
    private readonly IGitHubReleaseClient _gitHubReleaseClient = gitHubReleaseClient;

    public async Task DownloadAsync(string owner, string repo, 
        CancellationToken cancellationToken = default)
    {

        return await SaveAsync(stream, path);
    }

    private async Task SaveAsync(Stream stream, string path)
    {
        return await 
    }
}