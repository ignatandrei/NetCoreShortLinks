namespace NetCore7ShortLinks;

public class ShortUrlMiddleware : IMiddleware
{
    public ShortUrlMiddleware(ShortUrlOptions opt, ShortLinks shortLinks)
    {
        this.opt = opt;
        this.shortLinks = shortLinks;
    }
    private readonly ShortLinks shortLinks ;
    private readonly ShortUrlOptions opt;


    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var reqUrl = context.Request.GetDisplayUrl();

        await next(context);
        if (reqUrl == null) return;

        var resp = context.Response;
        if (resp.StatusCode != 200) return;
        if (resp.ContentType != "text/html") return;
        ShortLinksData data = new();
        data.Url = reqUrl;
        data.ApplicationName= opt.AppName;
        if (context.User?.Identity?.IsAuthenticated == true)
            data.UserName = context.User.Identity.Name;
        data.ShortUrl =Guid.NewGuid().ToString("N");
        shortLinks.Add(data);


    }
}
