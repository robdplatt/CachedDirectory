public class CacheOptions
{
    public readonly bool Enabled;
    public readonly TimeSpan Expiration;
    public string[]? Cache;
    public CacheOptions(TimeSpan expiration, bool enabled = true)
    {
        this.Expiration = expiration;
        this.Enabled = enabled;
    }
}

