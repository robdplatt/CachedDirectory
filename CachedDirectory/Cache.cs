internal class Cache
{
    public readonly string Path;
    public readonly SearchOption SearchOption;
    public readonly string[] Files;
    public readonly DateTime Created;
    public readonly DateTime Expires;
    public bool Expired
    {
        get
        {
            return DateTime.Now > Expires;
        }
    }

    internal Cache(string path, SearchOption searchOption, TimeSpan expiration)
    {
        this.Path = path;
        this.Files = System.IO.Directory.GetFiles(path, string.Empty, searchOption);
        this.SearchOption = searchOption;
        this.Created = DateTime.Now;
        this.Expires = Created.AddMilliseconds(expiration.TotalMilliseconds);
    }
}

