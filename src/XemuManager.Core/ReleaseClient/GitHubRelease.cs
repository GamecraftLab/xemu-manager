namespace XemuManager.Core.ReleaseClient;

public record GitHubRelease
{
    public readonly string TagName;
    public readonly DateTimeOffset PublishedAt;
    IReadOnlyList<GitHubAsset> Assets;

}

public record GitHubAsset
{
    public readonly string Name;
    Uri BrowserDownloadUri;
    private long Size;
}