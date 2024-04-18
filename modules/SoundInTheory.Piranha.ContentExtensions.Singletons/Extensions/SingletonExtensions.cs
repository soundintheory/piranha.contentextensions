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
using SoundInTheory.Piranha.ContentExtensions.Singletons;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Services;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Repositories;

public static class SingletonExtensions
{
    /// <summary>
    /// Adds the Singletons module.
    /// </summary>
    public static PiranhaServiceBuilder UseSingletons(this PiranhaServiceBuilder serviceBuilder)
    {
        serviceBuilder.Services.AddSingletons();
        return serviceBuilder;
    }

    /// <summary>
    /// Uses the Singletons module.
    /// </summary>
    /// <param name="applicationBuilder">The current application builder</param>
    /// <returns>The builder</returns>
    public static PiranhaApplication UseSingletons(this PiranhaApplication applicationBuilder)
    {
        applicationBuilder.Builder.UseSingletons();

        var builder = new SingletonBuilder();

        builder
            .AddRegisteredTypes()
            .Build();

        return applicationBuilder;
    }

    /// <summary>
    /// Uses the Singletons.
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The builder</returns>
    public static IApplicationBuilder UseSingletons(this IApplicationBuilder builder)
    {
        return builder.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = FileProvider,
            RequestPath = "/manager/contentextensions/singletons/assets"
        });
    }

    /// <summary>
    /// Adds the Singletons module.
    /// </summary>
    /// <param name="services">The current service collection</param>
    /// <returns>The services</returns>
    public static IServiceCollection AddSingletons(this IServiceCollection services)
    {
        // Register services
        services.AddScoped<SingletonService>();
        services.TryAddScoped<ISingletonRepository, SingletonRepository>();

        // Add the Singletons module
        App.Modules.Register<SingletonsModule>();

        // TODO: Setup authorization policies
        /* services.AddAuthorization(o =>
        {

        }); */

        // Return the service collection
        return services;
    }

    /// <summary>
    /// Static accessor to Singletons module if it is registered in the Piranha application.
    /// </summary>
    /// <param name="modules">The available modules</param>
    /// <returns>The Singletons module</returns>
    public static SingletonsModule Singletons(this Piranha.Runtime.AppModuleList modules)
    {
        return SingletonsModule.Instance;
    }

    /// <summary>
    /// Static accessor to singleton hooks
    /// </summary>
    /// <returns>singleton hooks</returns>
    public static SingletonHooks Singletons(this HookManager hookManager)
    {
        return SingletonsModule.Hooks;
    }

    private static IFileProvider FileProvider
    {
        get
        {
            if (IsDebugBuild)
            {
                return new PhysicalFileProvider(GetProjectPath("assets"));
            }

            return new EmbeddedFileProvider(typeof(SingletonsModule).Assembly, "SoundInTheory.Piranha.ContentExtensions.Singletons.assets");
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
