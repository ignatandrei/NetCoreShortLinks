NetCoreShortLinks
.NET Core short links -  short links for your site


How to use it
Install the package from <a href="https://www.nuget.org/packages/NetCore7ShortLinks">https://www.nuget.org/packages/NetCore7ShortLinks
Use the following code for .NET 7

builder.Services.AddShortUrl();
//code
app.UseShortUrl();
app.UseDefaultFiles();
app.UseStaticFiles();
//code
app.MapControllers();
app.MapShortUrlEndpoints();


What it does
It adds a middleware that creates short url for each response that matches 200 and content type text/html.
You can see the list at /short/list/json
You can follow a short url from /short/goto/
Github
<a href="https://github.com/ignatandrei/NetCoreShortLinks">https://github.com/ignatandrei/NetCoreShortLinks

