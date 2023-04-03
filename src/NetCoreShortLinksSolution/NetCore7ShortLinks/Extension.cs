
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace NetCore7ShortLinks;
public class ShortUrlOptions
{
    public string? AppName { get; set; }
}
public static class Extension
{

    public static void AddShortUrl(this IServiceCollection services, ShortUrlOptions? opt)
    {
        opt ??=new ShortUrlOptions();
        services.AddSingleton(opt);
        services.AddSingleton<ShortLinks>();
        services.AddSingleton<ShortUrlMiddleware>();
    }
    public static IEndpointRouteBuilder MapShortUrlEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var sp = endpoints.ServiceProvider;
        endpoints.MapGet("/short/goto/{name:alpha}", (HttpContext context, string name) =>
        {
            var links = sp.GetRequiredService<ShortLinks>();
            var link = links.FindShortUrl(name);
            if (link != null)
            {
                context.Response.Redirect(link.Url!);
                return;
            }
            //LINK IS NULL
            context.Response.WriteAsJsonAsync("not a valid short endpoint");

        });

        endpoints.MapGet("/short/list/", (HttpContext context) =>
        {
            var links = sp.GetRequiredService<ShortLinks>();
            ShortLinksData[] data;
            if (context.User?.Identity?.IsAuthenticated == true)
            {

                var nameUser = context.User.Identity.Name ?? "";
                data = links.From(nameUser);
            }
            else
            {
                data = links.NoUser();
            }
            context.Response.WriteAsJsonAsync(data);

        });
        return endpoints;
    }
    public static IApplicationBuilder UseShortUrl(this IApplicationBuilder app)
    {
        app.UseMiddleware<ShortUrlMiddleware>();
        
        return app;
    }

}
