namespace XemuManager.Core.DownloadManager.Exceptions;

public enum DownloadError
{
    NoConnection,
    ServerUnreachable,
    SecureConnectionFailed,
    ProxyFailed,
    RateLimited,
    NotFound,
    ProxyAuthRequired,
    ServerError,
    UnexpectedContent,
    Timeout,
    Stalled,
    ConnectionLost,
    IncompleteFile,
    DiskFull,
    AccessDenied,
    DirectoryNotFound,
    PathTooLong,
    FileInUse,
    FileRemoved,
    SizeMismatch,
    HashMismatch,
    CorruptArchive,
    Unknown
}