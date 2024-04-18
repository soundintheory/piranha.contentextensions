using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Piranha.Models;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class HtmlHelperExtensions
{
    public static Task<T> GetPageSingletonAsync<T>(this IHtmlHelper html, Guid? siteId = null) where T : PageBase
    {
        var service = html.ViewContext.HttpContext.RequestServices.GetRequiredService<SingletonService>();
        return service.GetPageAsync<T>(siteId);
    }

    public static T GetPageSingleton<T>(this IHtmlHelper html, Guid? siteId = null) where T : PageBase
        => html.GetPageSingletonAsync<T>(siteId).GetAwaiter().GetResult();

    public static Task<T> GetContentSingletonAsync<T>(this IHtmlHelper html, Guid? languageId = null) where T : GenericContent
    {
        var service = html.ViewContext.HttpContext.RequestServices.GetRequiredService<SingletonService>();
        return service.GetContentAsync<T>(languageId);
    }

    public static T GetContentSingleton<T>(this IHtmlHelper html, Guid? languageId = null) where T : GenericContent
        => html.GetContentSingletonAsync<T>(languageId).GetAwaiter().GetResult();

    public static Task<string> GetPageUrlAsync<T>(this IHtmlHelper html, Guid? siteId = null) where T : PageBase
    {
        var service = html.ViewContext.HttpContext.RequestServices.GetRequiredService<SingletonService>();
        return service.GetUrlAsync<T>(siteId);
    }

    public static string GetPageUrl<T>(this IHtmlHelper html, Guid? siteId = null) where T : PageBase 
        => html.GetPageUrlAsync<T>(siteId).GetAwaiter().GetResult();
}