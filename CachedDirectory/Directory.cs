using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;

public static class Directory
{

    static List<Cache> Caches = new List<Cache>();
    static object lockObject = new object();
    private static Cache GetCache(string path, SearchOption searchOption, CacheOptions cacheOptions)
    {       

        lock (lockObject)
        {
            //remove expired
            Caches.RemoveAll(cache => cache.Expired);

            //get cache
            var cache = Caches.FirstOrDefault(cache => cache.Path == path && cache.SearchOption == searchOption);
            if (cache == null)
            {
                cache = new Cache(path, searchOption, cacheOptions.Expiration);
                Caches.Add(cache);
            }

            return cache;
        }
        
    }

    public static string[] GetFiles(string path, string searchPattern, CacheOptions? cacheOptions)
    {
        return GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly, cacheOptions);
    }

    public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption, CacheOptions? cacheOptions)
    {

        if (cacheOptions?.Enabled ?? false)
        {
            var cache = GetCache(path, searchOption, cacheOptions ?? new CacheOptions(TimeSpan.FromSeconds(30)));

            var r = cache.Files.AsParallel().Where(s => FileSystemName.MatchesSimpleExpression(searchPattern, Path.GetFileName(s))).ToArray();
            return r;
        }

        return System.IO.Directory.GetFiles(path, searchPattern, searchOption);
    }

    public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, CacheOptions? cacheOptions)
    {
        return EnumerateFiles(path, searchPattern, SearchOption.TopDirectoryOnly, cacheOptions);
    }

    public static IEnumerable<string> EnumerateFiles(string path, string searchPattern, SearchOption searchOption, CacheOptions? cacheOptions)
    {
        if (cacheOptions?.Enabled ?? false)
        {
            var cache = GetCache(path, searchOption, cacheOptions ?? new CacheOptions(TimeSpan.FromSeconds(30)));

            var r = cache.Files.AsParallel().Where(s => FileSystemName.MatchesSimpleExpression(searchPattern, Path.GetFileName(s)));
            return r;
        }

        return System.IO.Directory.EnumerateFiles(path, searchPattern, searchOption);
    }

    public static bool Exists(string? path)
    {
        return System.IO.Directory.Exists(path);
    }

    public static void CreateDirectory(string path)
    {
        System.IO.Directory.CreateDirectory(path);
    }

    public static void DeleteDirectory(string path, bool recursive)
    {
        System.IO.Directory.Delete(path, recursive);
    }

    public static string GetCurrentDirectory()
    {
        return System.IO.Directory.GetCurrentDirectory();
    }
}

