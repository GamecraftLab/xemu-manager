using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using XemuManager.Core.Exceptions.Download;
using System.Linq;

namespace XemuManager.Core.ReleaseManager;

public class GitHubReleaseManager : IGitHubReleaseManager
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly HttpClient _httpClient;

    public GitHubReleaseManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.github.com/");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("XemuManager");
    }
    
    public async Task<GitHubRelease?> GetLatestReleaseAsync(
        string owner, string repo,
        Func<GitHubAsset, bool>? filter = null,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var release = await _httpClient.GetFromJsonAsync<GitHubRelease>(
                $"repos/{owner}/{repo}/releases/latest",
                JsonOptions,
                cancellationToken);

            if (release is not null && filter is not null)
            {
                release = release with { Assets = release.Assets.Where(filter).ToList() };
            }

            return release;
        }
        catch (HttpRequestException ex) when (ex.StatusCode is not null)
        {
            var error = ex.StatusCode switch
            {
                HttpStatusCode.NotFound => DownloadError.NotFound,
                HttpStatusCode.Forbidden => DownloadError.AccessDenied,
                HttpStatusCode.TooManyRequests => DownloadError.RateLimited,
                HttpStatusCode.ProxyAuthenticationRequired => DownloadError.ProxyAuthRequired,
                HttpStatusCode.InternalServerError => DownloadError.ServerError,
                _ => DownloadError.Unknown
            };

            throw new DownloadException(error, ex);
        }
        catch (HttpRequestException ex)
        {
            var error = ex.HttpRequestError switch
            {
                HttpRequestError.NameResolutionError => DownloadError.NoConnection,
                HttpRequestError.ConnectionError => DownloadError.ServerUnreachable,
                HttpRequestError.SecureConnectionError => DownloadError.SecureConnectionFailed,
                HttpRequestError.ProxyTunnelError => DownloadError.ProxyFailed,
                _ => DownloadError.Unknown
            };

            throw new DownloadException(error, ex);
        }
        catch (OperationCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            throw new DownloadException(DownloadError.Timeout, ex);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (JsonException ex)
        {
            throw new DownloadException(DownloadError.UnexpectedContent, ex);
        }
        catch (NotSupportedException ex)
        {
            throw new DownloadException(DownloadError.UnexpectedContent, ex);
        }
        catch (HttpIOException ex)
        {
            throw new DownloadException(DownloadError.ConnectionLost, ex);
        }
    }
}
