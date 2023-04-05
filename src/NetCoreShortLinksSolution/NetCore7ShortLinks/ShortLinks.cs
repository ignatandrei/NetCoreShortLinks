namespace NetCoreShortLinks;

public class ShortLinks
{
    internal readonly ShortUrlOptions opt;

    public ShortLinks(ShortUrlOptions opt)
    {
        this.opt = opt;
    }
    private Dictionary<string,ShortLinksData> ShortLinksData { get; } = new();
    public void Add( ShortLinksData data)
    {
        if (data.Url == null)
            return;
        data.ApplicationName ??= opt.AppName;
        if (!ShortLinksData.ContainsKey(data.Key()))
            ShortLinksData.Add(data.Key(), data);
        else
            ShortLinksData[data.Key()].LastAccessedDate = data.LastAccessedDate;
    }
    public ShortLinksData? FindShortUrl(string id)
    {
        return ShortLinksData.Values.FirstOrDefault(it => it.ShortUrl == id);
    }
    public ShortLinksData[] All()
    {
        return ShortLinksData.Values.ToArray();
    }
        public ShortLinksData[] From(string user)
    {
        var userLower=user.ToLowerInvariant();
        return ShortLinksData.Values
            
            .Where(it=>it.UserName != null && it.UserName!.ToLowerInvariant() == user )
            
            .ToArray();
    }
    public ShortLinksData[] NoUser()
    {
        return ShortLinksData.Values
            .Where(it => it.UserName == null)
            .ToArray();

    }


}

