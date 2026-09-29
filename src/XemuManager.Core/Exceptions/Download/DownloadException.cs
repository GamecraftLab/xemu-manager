namespace XemuManager.Core.Exceptions.Download;

public class DownloadException:Exception
{
    public DownloadError Error { get; }
    public DownloadException(DownloadError error,
        Exception? innerException = null) : 
        base(error.ToString(), innerException)
        {
            Error = error;
        }
}