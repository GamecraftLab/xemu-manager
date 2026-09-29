namespace XemuManager.Core.DownloaderClient;

public class DownloadManager
{
    private readonly HttpClient _httpClient;

    public DownloadManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    
    
}