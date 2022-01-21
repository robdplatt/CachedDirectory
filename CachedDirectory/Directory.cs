using System.IO.Enumeration;

public static class Directory
{

    static List<Cache> Caches = new List<Cache>();
    private static Cache GetCache(string path, SearchOption searchOption, CacheOptions cacheOptions)
    {
        //remove expired
        Caches.RemoveAll(cache => cache.Expired);

        //get cache
        var cache = Caches.FirstOrDefault(cache => cache.Path == path && cache.SearchOption == searchOption);
        if (cache == null) {
            cache = new Cache(path, searchOption, cacheOptions.Expiration);
            Caches.Add(cache);
        }

        //if (cacheOptions.Cache == null) cacheOptions.Cache = Directory.GetFiles(path, string.Empty, searchOption);
        return cache;
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
}

