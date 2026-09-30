using System.IO.Compression;
using XemuManager.Core.Exceptions.Download;

namespace XemuManager.Core.Installation;

public static class PackageInstaller
{
    public static async Task InstallAsync(
        string filePath,
        string destinationDirectory,
        CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(filePath);

        if (string.Equals(extension, ".zip", StringComparison.OrdinalIgnoreCase))
        {
            await Task.Run(
                () => ZipFile.ExtractToDirectory(filePath, destinationDirectory, overwriteFiles: true),
                cancellationToken);

            File.Delete(filePath);
        }
        else if (string.Equals(extension, ".AppImage", StringComparison.OrdinalIgnoreCase))
        {
            Directory.CreateDirectory(destinationDirectory);
            var targetPath = Path.Combine(destinationDirectory, Path.GetFileName(filePath));

            File.Move(filePath, targetPath, overwrite: true);

            if (!OperatingSystem.IsWindows())
            {
                File.SetUnixFileMode(targetPath, File.GetUnixFileMode(targetPath) | UnixFileMode.UserExecute);
            }
        }
        else
        {
            throw new DownloadException(DownloadError.UnexpectedContent);
        }
    }
}
