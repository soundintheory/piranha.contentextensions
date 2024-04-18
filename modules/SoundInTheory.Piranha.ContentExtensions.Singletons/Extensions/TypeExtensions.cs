using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SoundInTheory.Piranha.ContentExtensions.Singletons.Extensions
{
    public static class TypeExtensions
    {
        public static Type UnwrapNullable(this Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition().IsAssignableFrom(typeof(Nullable<>)))
            {
                return type.GenericTypeArguments[0];
            }

            return type;
        }

        public static Type GetReturnType(this MemberInfo memberInfo)
        {
            if (memberInfo == null)
            {
                return null;
            }

            return memberInfo switch
            {
                PropertyInfo propertyInfo => propertyInfo.PropertyType,
                FieldInfo fieldInfo => fieldInfo.FieldType,
                MethodInfo methodInfo => methodInfo.ReturnType,
                _ => null
            };
        }

        public static object GetDefaultValue(this Type t)
        {
            if (t.IsValueType)
                return Activator.CreateInstance(t);

            return null;
        }
    }
}
