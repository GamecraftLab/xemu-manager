

namespace XemuManager.Core.ReleaseManager;

public record GitHubRelease(string TagName, 
    DateTimeOffset PublishedAt,
    IReadOnlyList<GitHubAsset> Assets);

public record GitHubAsset(string Name, Uri BrowserDownloadUrl, long Size);
