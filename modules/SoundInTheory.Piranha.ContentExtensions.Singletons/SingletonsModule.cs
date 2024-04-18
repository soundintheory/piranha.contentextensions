using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using Piranha.Security;
using System.Collections.Generic;
using Piranha.Runtime;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Runtime;

namespace SoundInTheory.Piranha.ContentExtensions
{
    public class SingletonsModule : IModule
    {
        private readonly List<PermissionItem> _permissions = new()
        {
            // TODO: Add permissions
        };

        private readonly SingletonHooks _hooks = new SingletonHooks();

        /// <summary>
        /// Collection of singletons that have been registered
        /// </summary>
        public SingletonTypeList Types { get; private set; } = new SingletonTypeList();

        /// <summary>
        /// The singleton module instance.
        /// </summary>
        public static SingletonsModule Instance { get; private set; }

        /// <summary>
        /// Gets the module author
        /// </summary>
        public string Author => "Sound in Theory";

        /// <summary>
        /// Gets the module name
        /// </summary>
        public string Name => "Singletons";

        /// <summary>
        /// Gets the module version
        /// </summary>
        public string Version => Utils.GetAssemblyVersion(GetType().Assembly);

        /// <summary>
        /// Gets the module description
        /// </summary>
        public string Description => "Global singletons";

        /// <summary>
        /// Gets the module package url
        /// </summary>
        public string PackageUrl => "";

        /// <summary>
        /// Gets the module icon url
        /// </summary>
        public string IconUrl => "/manager/contentextensions/singletons/assets/images/logo.svg";

        /// <summary>
        /// The hooks for the menu data model
        /// </summary>
        public static SingletonHooks Hooks => Instance._hooks;

        public static string CacheKey(string key) => $"Singletons:{key}";

        public void Init()
        {
            Instance = this;

            // Register permissions
            foreach (var permission in _permissions)
            {
                App.Permissions["Singletons"].Add(permission);
            }
        }
    }
}
