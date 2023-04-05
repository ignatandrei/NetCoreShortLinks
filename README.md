
# NetCoreShortLinks

.NET Core short links -  short links for your site
[![Build Nuget](https://github.com/ignatandrei/NetCoreShortLinks/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/ignatandrei/NetCoreShortLinks/actions/workflows/dotnet.yml)

![Nuget](https://img.shields.io/nuget/dt/NetCore7ShortLinks?label=NetCore7ShortLinks&style=for-the-badge)


# How to use it

Install the package from https://www.nuget.org/packages/NetCore7ShortLinks

Use the following code for .NET 7

```csharp

builder.Services.AddShortUrl();
//code
app.UseShortUrl();
app.UseDefaultFiles();
app.UseStaticFiles();
//code
app.MapControllers();
app.MapShortUrlEndpoints();

```

# What it does

It adds a middleware that creates short url for each response that matches 200 and content type text/html. 

You can see the list at /short/list/json

You can follow a short url from /short/goto/{shorturl}

# Github

https://github.com/ignatandrei/NetCoreShortLinks
