using Microsoft.CodeAnalysis;
using Piranha;
using Piranha.Cache;
using Piranha.Models;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Extensions;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Models;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CacheLevel = Piranha.Cache.CacheLevel;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Services
{
    public class SingletonService
    {
        private readonly IApi _api;

        private readonly ISingletonRepository _repo;

        private readonly ICache _cache;

        public SingletonService(IApi api, ISingletonRepository repo, ICache cache = null)
        {
            _api = api;
            _repo = repo;

            if (App.CacheLevel != CacheLevel.None)
            {
                _cache = cache;
            }
        }

        public async Task<string> GetUrlAsync<T>(Guid? siteId = null) where T : PageBase
        {
            if (!SingletonsModule.Instance.Types.TryGet<T>(out var typeInfo))
            {
                return null;
            }

            var page = await GetPageAsync<T>(siteId);

            return page.Permalink;
        }

        public async Task<PageBase> GetPageAsync(string typeId = null, Guid? siteId = null)
        {
            var pageType = App.PageTypes.GetById(typeId);

            if (pageType == null)
            {
                return null;
            }

            var type = Type.GetType(pageType.CLRType);

            if (type == null)
            {
                return null;
            }

            var genericMethod = typeof(SingletonService)
                .GetMethod("GetPageAsync", 1, new Type[] { typeof(Guid?) })
                .MakeGenericMethod(type);
            dynamic task = genericMethod.Invoke(this, new object[] { siteId });

            PageBase page = await task;

            return page;
        }

        public async Task<T> GetPageAsync<T>(Guid? siteId = null) where T : PageBase
        {
            if (!SingletonsModule.Instance.Types.TryGet<T>(out var typeInfo))
            {
                return null;
            }

            siteId = await EnsureSiteIdAsync(siteId);
            var instanceId = await _cache.GetOrAddAsync(typeInfo.IdCacheKey, () => _repo.GetPageIdAsync(typeInfo.Id, siteId.Value));

            return instanceId != null
                ? await _api.Pages.GetByIdAsync<T>(instanceId.Value)
                : await CreatePageAsync<T>(typeInfo, siteId.Value);
        }

        public async Task<GenericContent> GetContentAsync(string typeId = null, Guid? languageId = null)
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

            var genericMethod = typeof(SingletonService)
                .GetMethod("GetContentAsync", 1, new Type[] { typeof(Guid?) })
                .MakeGenericMethod(type);
            dynamic task = genericMethod.Invoke(this, new object[] { languageId });

            GenericContent content = await task;

            return content;
        }

        public async Task<T> GetContentAsync<T>(Guid? languageId = null) where T : GenericContent
        {
            if (!SingletonsModule.Instance.Types.TryGet<T>(out var typeInfo))
            {
                return null;
            }

            languageId = await EnsureLanguageIdAsync(languageId);
            var instanceId = await _cache.GetOrAddAsync(typeInfo.IdCacheKey, () => _repo.GetContentIdAsync(typeInfo.Id));

            return instanceId != null
                ? await _api.Content.GetByIdAsync<T>(instanceId.Value)
                : await CreateContentAsync<T>(typeInfo, languageId.Value);
        }

        private async Task<T> CreatePageAsync<T>(SingletonType typeInfo, Guid siteId) where T : PageBase
        {
            var instance = await _api.Pages.CreateAsync<T>(typeInfo.Id);

            instance.Title = typeInfo.Title;
            instance.SiteId = siteId;

            await _api.Pages.SaveAsync(instance);

            _cache?.Set(typeInfo.IdCacheKey, instance.Id);

            return instance;
        }

        private async Task<T> CreateContentAsync<T>(SingletonType typeInfo, Guid languageId) where T : GenericContent
        {
            var instance = await _api.Content.CreateAsync<T>(typeInfo.Id);

            instance.Title = typeInfo.Title;

            await _api.Content.SaveAsync(instance, languageId);

            _cache?.Set(typeInfo.IdCacheKey, instance.Id);

            return instance;
        }

        private async Task<Guid> EnsureSiteIdAsync(Guid? siteId)
        {
            if (!siteId.HasValue)
            {
                var site = await _api.Sites.GetDefaultAsync().ConfigureAwait(false);

                if (site != null)
                {
                    return site.Id;
                }
            }
            return siteId.Value;
        }

        private async Task<Guid> EnsureLanguageIdAsync(Guid? languageId)
        {
            if (!languageId.HasValue)
            {
                return (await _api.Languages.GetDefaultAsync()).Id;
            }
            return languageId.Value;
        }
    }
}
