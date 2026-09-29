using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using XemuManager.Core.Exceptions.Download;


namespace XemuManager.Core.ReleaseClient;

public class GitHubReleaseClient : IGitHubReleaseClient
{
    private readonly HttpClient _httpClient;

    public GitHubReleaseClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.github.com/");
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("XemuManager");
    }
    
    public async Task<GitHubRelease?> GetLatestReleaseAsync(string owner, string repo, CancellationToken cancellationToken = default)
    {
        try
        {
            var release = await _httpClient.GetFromJsonAsync<GitHubRelease>(
                $"repos/{owner}/{repo}/releases/latest",
                cancellationToken);

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

    Task<Stream> DownloadAssetAsync(Uri assetUri, CancellationToken cancellationToken = default)
    {
        
    }
}