using Piranha.Manager;
using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Models;
using Piranha.Models;
using SoundInTheory.Piranha.ManagerExtensions.Lists.Attributes;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons
{
    public class SingletonBuilder
    {
        private readonly Dictionary<string, SingletonType> _types = new();

        public SingletonBuilder Build()
        {
            foreach (var type in _types.Values)
            {
                SingletonsModule.Instance.Types.Register(type);

                if (type.ShowInMenu)
                {
                    Menu.Items["Content"].Items.Add(new MenuItem
                    {
                        InternalId = $"{type.Id}_Singleton",
                        Name = type.Title,
                        Route = type.Route,
                        Css = type.Icon ?? "fa fa-page"
                    });
                }
            }

            return this;
        }

        /// <summary>
        /// Loops through all registered content types and adds areas configured via attributes
        /// </summary>
        /// <returns>The builder</returns>
        internal SingletonBuilder AddRegisteredTypes()
        {
            // Content
            foreach (var contentType in App.ContentTypes)
            {
                var singletonType = GetSingletonType(contentType);

                if (singletonType != null)
                {
                    _types[singletonType.Id] = singletonType;
                }
            }

            // Pages
            foreach (var contentType in App.PageTypes)
            {
                var singletonType = GetSingletonType(contentType);

                if (singletonType != null)
                {
                    _types[singletonType.Id] = singletonType;
                }
            }

            return this;
        }

        private SingletonType GetSingletonType(ContentTypeBase contentType)
        {
            var modelType = Type.GetType(contentType.CLRType);
            var attr = modelType?.GetTypeInfo()?.GetCustomAttribute<SingletonAttribute>();

            if (attr == null)
            {
                return null;
            }

            var singletonType = new SingletonType(contentType)
            {
                Icon = attr.Icon,
                ShowInMenu = attr.ShowInMenu,
                Slug = attr.Slug
            };

            if (!string.IsNullOrEmpty(attr.Title))
            {
                singletonType.Title = attr.Title;
            }

            return singletonType;
        }
    }
}
