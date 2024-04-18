using SoundInTheory.Piranha.ContentExtensions.Areas.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Areas.Runtime
{
    public class ContentAreaTypeList
    {
        private readonly ConcurrentDictionary<string, ContentAreaType> _types = new();

        public ContentAreaType[] GetAll() => _types.Values.ToArray();

        public bool Register(ContentAreaType type)
        {
            return _types.TryAdd(type.TypeId, type);
        }

        public ContentAreaType this[string id]
        {
            get => GetById(id);
            set => Register(value);
        }

        public ContentAreaType GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return _types.TryGetValue(id, out var menu) ? menu : null;
        }

        public bool ContainsId(string id)
        {
            return _types.ContainsKey(id);
        }
    }
}
