namespace XemuManager.Core.ReleaseClient;

public interface IGitHubReleaseClient
{
    Task<GitHubRelease?> GetLatestReleaseAsync(
        string owner,
        string repo,
        CancellationToken cancellationToken = default);
    Task<Stream> DownloadAssetAsync(Uri assetUri, CancellationToken cancellationToken = default);
}