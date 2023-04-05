
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using System.Runtime.CompilerServices;

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

        }).RequireAuthorization(links.opt.AuthPolicy).WithTags("ShortUrl");

        Func<string,string, ShortLinksData> Construct = (string urlToAdd, string urlRequest) =>
        {
            var uriOrig = new Uri(urlRequest);
            var auth = uriOrig.GetLeftPart(UriPartial.Authority);

            string formattedUrl = urlToAdd;
            try
            {
                //ensure it is an url
                var uriToAdd = new Uri(urlToAdd);
                var uri = new UriBuilder(uriToAdd);
                uri.Host = uriOrig.Host;
                uri.Scheme = uriOrig.Scheme;
                uri.Port = uriOrig.Port;
                formattedUrl = uri.ToString();
            }
            catch
            {
                //add initial
                formattedUrl = auth + "/"+formattedUrl;
            }
            ShortLinksData data = new();
            data.Url = formattedUrl;
            return data;

        };
        endpoints.MapGet("/short/add/noAuth/{*url}/", (HttpContext context,string url) =>
        {

            ShortLinksData data=Construct(url, context.Request.GetDisplayUrl());
            links.Add(data);
            context.Response.WriteAsJsonAsync(data);

        }).WithTags("ShortUrl");

        endpoints.MapGet("/short/add/auth/{*url}/", (HttpContext context, string url) =>
        {
            ShortLinksData data = Construct(url, context.Request.GetDisplayUrl());
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
