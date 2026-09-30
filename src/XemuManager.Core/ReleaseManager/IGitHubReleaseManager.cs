namespace XemuManager.Core.ReleaseManager;

public interface IGitHubReleaseManager
{
    Task<GitHubRelease?> GetLatestReleaseAsync(
        string owner,
        string repo,
        Func<GitHubAsset, bool>? filter = null,
        CancellationToken cancellationToken = default);
}
