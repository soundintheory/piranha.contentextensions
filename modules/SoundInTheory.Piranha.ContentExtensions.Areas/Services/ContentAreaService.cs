using Microsoft.EntityFrameworkCore;
using Piranha;
using Piranha.Manager.Localization;
using Piranha.Models;
using Piranha.Services;
using SoundInTheory.Piranha.ContentExtensions.Areas.Attributes;
using SoundInTheory.Piranha.ContentExtensions.Areas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Services
{
    public class ContentAreaService
    {
        private readonly IDb _db;
        private readonly IApi _api;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="db">The current db context</param>
        /// <param name="factory">The content service factory</param>
        public ContentAreaService(IDb db, IApi api)
        {
            _db = db;
            _api = api;
        }

        public async Task<ContentArea> GetAreaAsync(string typeId, Guid? languageId = null)
        {
            var contentType = App.ContentTypes.GetById(typeId);

            if (contentType == null)
            {
                return null;
            }

            var type = Type.GetType(contentType.CLRType);

            if (type == null)
            {
                return null;
            }

            var genericMethod = typeof(ContentAreaService)
                .GetMethod("GetAreaAsync", 1, new Type[] { typeof(Guid?) })
                .MakeGenericMethod(type);
            dynamic task = genericMethod.Invoke(this, new object[] { languageId });

            ContentArea area = await task;

            return area;
        }

        public async Task<T> GetAreaAsync<T>(Guid? languageId = null) where T : ContentArea
        {
            languageId = await EnsureLanguageIdAsync(languageId).ConfigureAwait(false);
            var clrType = typeof(T).AssemblyQualifiedName;
            var contentType = App.ContentTypes.Find(x => x.CLRType == clrType);

            if (contentType == null)
            {
                return null;
            }

            var typeId = contentType.Id;
            var contentId = await _db.ContentTranslations
                .AsNoTracking()
                .Where(c => c.LanguageId == languageId && c.Content.TypeId == typeId)
                .OrderByDescending(c => c.LastModified)
                .Select(p => (Guid?)p.ContentId)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);
            T area = default;

            if (!contentId.HasValue)
            {
                // Create if it doesn't exist
                area = await CreateAreaAsync<T>(languageId);
            }
            else
            {
                area = await _api.Content.GetByIdAsync<T>(contentId.Value);
            }

            return area;
        }

        private async Task<T> CreateAreaAsync<T>(Guid? languageId = null) where T : ContentArea
        {
            var area = typeof(T).GetTypeInfo().GetCustomAttribute<ContentAreaAttribute>();
            var clrType = typeof(T).AssemblyQualifiedName;
            var contentType = App.ContentTypes.Find(x => x.CLRType == clrType);
            var instance = await _api.Content.CreateAsync<T>(contentType.Id);

            instance.Title = area.Title;

            await _api.Content.SaveAsync(instance, languageId);

            return instance;
        }

        private async Task<Guid> EnsureLanguageIdAsync(Guid? languageId)
        {
            if (!languageId.HasValue)
            {
                return (await _api.Languages.GetDefaultAsync()).Id;
            }
            return languageId.Value;
        }

        private async Task<Guid?> EnsureSiteIdAsync(Guid? siteId)
        {
            if (!siteId.HasValue)
            {
                var site = await _api.Sites.GetDefaultAsync().ConfigureAwait(false);

                if (site != null)
                {
                    return site.Id;
                }
            }
            return siteId;
        }
    }
}
