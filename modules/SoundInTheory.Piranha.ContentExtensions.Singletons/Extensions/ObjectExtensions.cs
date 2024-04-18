using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNullOrDefault(this object obj)
        {
            if (obj == null)
                return true;

            var type = obj.GetType();

            return obj.Equals(type.GetDefaultValue());
        }
    }
}
