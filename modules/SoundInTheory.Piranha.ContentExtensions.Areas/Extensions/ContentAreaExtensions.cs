using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Piranha;
using Piranha.AspNetCore;
using Piranha.AspNetCore.Models;
using Piranha.AttributeBuilder;
using Piranha.AspNetCore.Services;
using Piranha.Models;
using Piranha.Runtime;
using SoundInTheory.Piranha.ContentExtensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using SoundInTheory.Piranha.ContentExtensions.Areas;
using SoundInTheory.Piranha.ContentExtensions.Areas.Services;

public static class ContentAreaExtensions
{
    /// <summary>
    /// Adds the Content Areas module.
    /// </summary>
    public static PiranhaServiceBuilder UseContentAreas(this PiranhaServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddContentAreas();
        return serviceBuilder;
    }

    /// <summary>
    /// Uses the Content Areas module.
    /// </summary>
    /// <param name="applicationBuilder">The current application builder</param>
    /// <returns>The builder</returns>
    public static PiranhaApplication UseContentAreas(this PiranhaApplication applicationBuilder)
    {
        applicationBuilder.Builder.UseContentAreas();

        var builder = new ContentAreaBuilder();

        builder
            .AddRegisteredTypes()
            .Build();

        return applicationBuilder;
    }

    /// <summary>
    /// Uses the Content Areas.
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The builder</returns>
    public static IApplicationBuilder UseContentAreas(this IApplicationBuilder builder)
    {
        return builder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = FileProvider,
            RequestPath = "/manager/contentextensions/areas/assets"
        });
    }

    /// <summary>
    /// Adds the Content Areas module.
    /// </summary>
    /// <param name="services">The current service collection</param>
    /// <returns>The services</returns>
    public static IServiceCollection AddContentAreas(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<ContentAreaService>();

        // Add the Content Areas module
        App.Modules.Register<ContentAreasModule>();

        // TODO: Setup authorization policies
        /* services.AddAuthorization(o =>
        {

        }); */

        // Return the service collection
        return services;
    }

    /// <summary>
    /// Static accessor to Content Areas module if it is registered in the Piranha application.
    /// </summary>
    /// <param name="modules">The available modules</param>
    /// <returns>The Content Areas module</returns>
    public static ContentAreasModule ContentAreas(this Piranha.Runtime.AppModuleList modules)
    {
        return ContentAreasModule.Instance;
    }

    /// <summary>
    /// Static accessor to content area hooks
    /// </summary>
    /// <returns>content area hooks</returns>
    public static ContentAreaHooks ContentAreas(this HookManager hookManager)
    {
        return ContentAreasModule.Hooks;
    }

    public static RoutedContentBase GetCurrentItem(this IApplicationService app)
    {
        if (app.CurrentPost != null)
        {
            return app.CurrentPost;
        }

        return app.CurrentPage;
    }

    public static Guid? GetCurrentItemId(this IApplicationService app)
    {
        return app.GetCurrentItem()?.Id;
    }

    private static IFileProvider FileProvider
    {
        get
        {
            if (IsDebugBuild)
            {
                return new PhysicalFileProvider(GetProjectPath("assets"));
            }

            return new EmbeddedFileProvider(typeof(ContentAreasModule).Assembly, "SoundInTheory.Piranha.ContentExtensions.Areas.assets");
        }
    }

    private static string GetProjectPath(string path = null)
    {
        var filePath = GetCurrentFilePath() ?? "";
        var dir = Directory.GetParent(Directory.GetParent(filePath).FullName).FullName;

        if (!string.IsNullOrWhiteSpace(path))
        {
            return Path.Join(dir, path);
        }

        return dir;
    }

    private static string GetCurrentFilePath([CallerFilePath] string callerFilePath = null) => callerFilePath;

    private static bool IsDebugBuild
    {
        get
        {
#if DEBUG
            return true;
#else
                return false;
#endif
        }
    }
}
