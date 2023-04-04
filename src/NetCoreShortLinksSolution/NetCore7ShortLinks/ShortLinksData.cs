namespace NetCoreShortLinks;

public class ShortLinksData
{
    public ShortLinksData()
    {
        CreatedDate = DateTime.UtcNow;
        ShortUrl = Guid.NewGuid().ToString("N");
    }
    public string? ApplicationName { get; set; }
    public string? UserName { get; set; }
    public string?  Url { get; set; }
    public string ShortUrl { get; set; }
    public DateTime CreatedDate { get; set; }

    public string? FullUrl
    {
        get
        {
            if (Url == null)
                return null;
            var uri = new Uri(Url);
            var auth= uri.GetLeftPart(UriPartial.Authority);
            return auth + "/short/goto/" + ShortUrl;
        }
        set
        {
            throw new ArgumentException("cannot set;just for visibility in serialize");
        }
    }
}

