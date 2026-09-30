namespace XemuManager.Core.DownloadManager.FileDownloader;

public interface IFileDownloader
{
    public Task DownloadFileAsync(
        Uri uri,
        string destinationPath,
        CancellationToken cancellationToken = default);
}
