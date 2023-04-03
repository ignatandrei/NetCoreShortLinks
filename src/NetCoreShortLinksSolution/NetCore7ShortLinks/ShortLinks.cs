﻿namespace NetCore7ShortLinks;

public class ShortLinks
{
    private Dictionary<string,ShortLinksData> ShortLinksData { get; } = new();
    public void Add( ShortLinksData data)
    {
        if (data.Url == null)
            return;
        if(!ShortLinksData.ContainsKey(data.Url))
            ShortLinksData.Add(data.Url,data);
    }
    public ShortLinksData? FindShortUrl(string id)
    {
        return ShortLinksData.Values.FirstOrDefault(it => it.ShortUrl == id);
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
