using Microsoft.EntityFrameworkCore;
using Piranha;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Repositories
{
    public class SingletonRepository : ISingletonRepository
    {
        private readonly IDb _db;

        public SingletonRepository(IDb db)
        {
            _db = db;
        }

        /// <summary>
        /// Get the latest existing id for the given page type
        /// </summary>
        /// <param name="typeId">The page type id</param>
        /// <returns></returns>
        public async Task<Guid?> GetPageIdAsync(string typeId, Guid siteId)
        {
            var pageId = await _db.Pages
                .Where(p => p.PageTypeId == typeId && p.SiteId == siteId)
                .OrderByDescending(p => p.LastModified)
                .Select(p => (Guid?)p.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return pageId;
        }

        /// <summary>
        /// Get the latest existing id for the given content type
        /// </summary>
        /// <param name="typeId">The content type id</param>
        /// <returns></returns>
        public async Task<Guid?> GetContentIdAsync(string typeId)
        {
            var contentId = await _db.Content
                .Where(c => c.TypeId == typeId)
                .OrderByDescending(c => c.LastModified)
                .Select(c => (Guid?)c.Id)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            return contentId;
        }
    }
}
