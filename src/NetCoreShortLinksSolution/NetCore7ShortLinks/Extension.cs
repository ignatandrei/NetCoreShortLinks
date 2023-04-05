
namespace NetCoreShortLinks;
public static class Extension
{

    public static void AddShortUrl(this IServiceCollection services, ShortUrlOptions? opt=null)
    {
        opt ??=new ShortUrlOptions();
        services.AddSingleton(opt);
        services.AddSingleton<ShortLinks>();
        services.AddSingleton<ShortUrlMiddleware>();
    }
    public static IEndpointRouteBuilder MapShortUrlEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var sp = endpoints.ServiceProvider;
        var links = sp.GetRequiredService<ShortLinks>();
        endpoints.MapGet("/short/goto/{name}", (HttpContext context, string name) =>
        {
            var link = links.FindShortUrl(name);
            if (link != null)
            {
                context.Response.Redirect(link.Url!);
                return;
            }
            //LINK IS NULL
            context.Response.WriteAsJsonAsync("not a valid short endpoint");

        }).WithTags("ShortUrl");

        endpoints.MapGet("/short/list/noAuth/json", (HttpContext context) =>
        {
            ShortLinksData[] data= links.NoUser();            
            context.Response.WriteAsJsonAsync(data);

        }).WithTags("ShortUrl");

        endpoints.MapGet("/short/list/auth/json", (HttpContext context) =>
        {
            var nameUser = context.User?.Identity?.Name ?? "";
            ShortLinksData[] data = links.From(nameUser);            
            context.Response.WriteAsJsonAsync(data);

        }).RequireAuthorization().WithTags("ShortUrl");


        endpoints.MapGet("/short/add/{url:alpha}/", (HttpContext context,string url) =>
        {
            ShortLinksData data=new();
            data.Url = url;
            data.ApplicationName = url;
            links.Add(data);
            context.Response.WriteAsJsonAsync(data);

        }).WithTags("ShortUrl");

        endpoints.MapGet("/short/addAuth/{url:alpha}/", (HttpContext context, string url) =>
        {
            ShortLinksData data = new();
            data.Url = url;
            data.ApplicationName = url;
            links.Add(data);
            context.Response.WriteAsJsonAsync(data);

        }).WithTags("ShortUrl").RequireAuthorization(links.opt.AuthPolicy);


        return endpoints;
    }
    public static IApplicationBuilder UseShortUrl(this IApplicationBuilder app)
    { 
        app.UseMiddleware<ShortUrlMiddleware>();
        
        return app;
    }

}
