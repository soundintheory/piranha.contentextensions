using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Repositories
{
    public interface ISingletonRepository
    {
        Task<Guid?> GetPageIdAsync(string typeId, Guid siteId);

        Task<Guid?> GetContentIdAsync(string typeId);
    }
}
