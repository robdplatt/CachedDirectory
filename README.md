# CachedDirectory

Add caching to speed up Directory.GetFiles() and Directory.EnumerateFiles() when doing frequent/repeated operations.

An example might be repeated filesystem searches for various filenames within a short period of time.

## Usage

- Add a reference to this library
- Define a new CacheOptions()
- Override the .NET Directory.GetFiles() or Directory.EnumerateFiles() methods by including CacheOptions

## Examples

<pre>TimeSpan expiration = new TimeSpan(0, 0, 30, 0);
CacheOptions cacheOptions = new CacheOptions(expiration);

var dllFiles = Directory.GetFiles("c:\windows", "*.dll", SearchOption.AllDirectories, cacheOptions);
var exeFiles = Directory.GetFiles("c:\windows", "*.exe", SearchOption.AllDirectories, cacheOptions);
</pre>

The cache will be created during the first lookup. The second lookup will return without scanning the drive.
