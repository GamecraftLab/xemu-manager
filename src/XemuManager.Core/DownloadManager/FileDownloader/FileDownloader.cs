namespace XemuManager.Core.DownloadManager.FileDownloader;

public class FileDownloader(HttpClient httpClient) : IFileDownloader
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task DownloadFileAsync(
        Uri uri,
        string destinationPath,
        CancellationToken cancellationToken = default)
    {
        await using var stream = await DownloadStreamAsync(uri, cancellationToken);
        await SaveAsync(stream, destinationPath, cancellationToken);
    }

    private async Task<Stream> DownloadStreamAsync(Uri uri, CancellationToken cancellationToken)
    {
        return await _httpClient.GetStreamAsync(uri, cancellationToken);
    }

    private static async Task SaveAsync(Stream stream, string path, CancellationToken cancellationToken)
    {
        await using var file = File.Create(path);
        await stream.CopyToAsync(file, cancellationToken);
    }
}
