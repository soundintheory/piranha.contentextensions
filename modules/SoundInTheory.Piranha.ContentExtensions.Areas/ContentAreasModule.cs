using Piranha;
using Piranha.Extend;
using Piranha.Manager;
using Piranha.Security;
using System.Collections.Generic;
using Piranha.Runtime;
using SoundInTheory.Piranha.ContentExtensions.Areas.Runtime;

namespace SoundInTheory.Piranha.ContentExtensions
{
    public class ContentAreasModule : IModule
    {
        private readonly List<PermissionItem> _permissions = new()
        {
            // TODO: Add permissions
        };

        private readonly ContentAreaHooks _hooks = new ContentAreaHooks();

        /// <summary>
        /// Collection of lists that have been registered to display in the manager
        /// </summary>
        public ContentAreaTypeList Types { get; private set; } = new ContentAreaTypeList();

        /// <summary>
        /// The singleton module instance.
        /// </summary>
        public static ContentAreasModule Instance { get; private set; }

        /// <summary>
        /// Gets the module author
        /// </summary>
        public string Author => "Sound in Theory";

        /// <summary>
        /// Gets the module name
        /// </summary>
        public string Name => "Content Areas";

        /// <summary>
        /// Gets the module version
        /// </summary>
        public string Version => Utils.GetAssemblyVersion(GetType().Assembly);

        /// <summary>
        /// Gets the module description
        /// </summary>
        public string Description => "Global content areas";

        /// <summary>
        /// Gets the module package url
        /// </summary>
        public string PackageUrl => "";

        /// <summary>
        /// Gets the module icon url
        /// </summary>
        public string IconUrl => "/manager/contentextensions/areas/assets/images/logo.svg";

        /// <summary>
        /// The hooks for the menu data model
        /// </summary>
        public static ContentAreaHooks Hooks => Instance._hooks;

        public void Init()
        {
            Instance = this;

            // Register permissions
            foreach (var permission in _permissions)
            {
                App.Permissions["ContentAreas"].Add(permission);
            }
        }
    }
}
