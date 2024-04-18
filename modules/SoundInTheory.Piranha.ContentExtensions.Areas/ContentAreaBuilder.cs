using Piranha.Manager;
using Piranha;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoundInTheory.Piranha.ContentExtensions.Areas.Models;
using Piranha.Models;
using System.Reflection;
using SoundInTheory.Piranha.ContentExtensions.Areas.Attributes;

namespace SoundInTheory.Piranha.ContentExtensions.Areas
{
    public class ContentAreaBuilder
    {
        private readonly Dictionary<string, ContentAreaType> _types = new();

        public ContentAreaBuilder Build()
        {
            foreach (var type in _types.Values)
            {
                ContentAreasModule.Instance.Types.Register(type);
            }

            if (_types.Count > 0)
            {
                var deleteAction = Actions.Toolbars.ContentEdit.Find(x => x.InternalId == "Delete");

                if (deleteAction != null)
                {
                    deleteAction.ActionView = "Partial/Actions/_AreaDelete";
                }
            }

            return this;
        }

        /// <summary>
        /// Loops through all registered content types and adds areas configured via attributes
        /// </summary>
        /// <returns>The builder</returns>
        internal ContentAreaBuilder AddRegisteredTypes()
        {
            // Content groups
            foreach (var contentType in App.ContentTypes)
            {
                var areaType = GetAreaType(contentType);

                if (areaType != null)
                {
                    _types[areaType.TypeId] = areaType;
                }
            }

            return this;
        }

        private ContentAreaType GetAreaType(ContentType contentType)
        {
            var modelType = Type.GetType(contentType.CLRType);
            var attr = modelType?.GetTypeInfo()?.GetCustomAttribute<ContentAreaAttribute>();

            if (attr == null)
            {
                return null;
            }

            return new ContentAreaType
            {
                Title = attr.Title,
                Description = attr.Description,
                Icon = attr.Icon,
                TypeId = contentType.Id,
                CLRType = contentType.CLRType
            };
        }
    }
}
