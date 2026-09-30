using System.IO;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace XemuManager.Core.LibraryManager;

public class SearchClient
{
    private readonly string _libRootPath;
    private static readonly EnumerationOptions ScanOptions = 
        new EnumerationOptions
    {
        RecurseSubdirectories = true,
        MatchCasing = MatchCasing.CaseInsensitive,
    };
    public SearchClient(string libRootPath)
    {
        _libRootPath = libRootPath;
    }

    public IEnumerable<string>? Search(string query, Func<string,bool> predicate)
    {
        if (!Directory.Exists(_libRootPath))
            return null;
        var gameFilePaths = Directory.EnumerateFiles(_libRootPath, "*", ScanOptions);
        gameFilePaths = gameFilePaths.Where(predicate);
        // gameFilePaths = gameFilePaths.Where(p => p.EndsWith(".iso", StringComparison.OrdinalIgnoreCase) || p.EndsWith(".xiso", StringComparison.OrdinalIgnoreCase));
        return gameFilePaths;
    }
}