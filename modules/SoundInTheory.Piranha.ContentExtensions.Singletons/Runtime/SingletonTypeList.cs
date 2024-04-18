using Piranha;
using Piranha.Models;
using SoundInTheory.Piranha.ContentExtensions.Singletons.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Runtime
{
    public class SingletonTypeList
    {
        private readonly ConcurrentDictionary<string, SingletonType> _types = new();

        public SingletonType[] GetAll() => _types.Values.ToArray();

        public bool Register(SingletonType type)
        {
            return _types.TryAdd(type.Id, type);
        }

        public SingletonType this[string id]
        {
            get => GetById(id);
            set => Register(value);
        }

        public bool TryGet<T>(out SingletonType typeInfo) where T : ContentBase
        {
            typeInfo = GetOrCreate<T>();
            return typeInfo != null;
        }

        public SingletonType GetOrCreate<T>() where T : ContentBase
        {
            var clrType = typeof(T).AssemblyQualifiedName;
            var type = _types.Values.FirstOrDefault(t => t.CLRType == clrType);

            if (type != null)
            {
                return type;
            }

            ContentTypeBase piranhaType = App.ContentTypes.Find(x => x.CLRType == clrType);
            piranhaType ??= App.PageTypes.Find(x => x.CLRType == clrType);
            piranhaType ??= App.PostTypes.Find(x => x.CLRType == clrType);

            if (piranhaType != null)
            {
                type = _types.GetOrAdd(piranhaType.Id, (key) => new SingletonType(piranhaType));
            }

            return type;
        }

        public SingletonType GetById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }

            return _types.TryGetValue(id, out var type) ? type : null;
        }

        public bool ContainsId(string id)
        {
            return _types.ContainsKey(id);
        }
    }
}
