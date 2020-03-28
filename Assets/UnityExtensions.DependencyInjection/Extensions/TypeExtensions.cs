using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityExtensions.DependencyInjection.Extensions
{
    internal static class TypeExtensions
    {
        internal static bool MemberHasInjectAttribute<T>(this T memberInfo) where T : MemberInfo
        {
            if (memberInfo is null) throw new ArgumentNullException(nameof(memberInfo));

            return memberInfo.GetCustomAttributes<InjectAttribute>().Any();
        }

        internal static IEnumerable<Type> GetParentTypes(this Type type)
        {
            if (type == null || type.BaseType() == null || type == typeof(object) || type.BaseType() == typeof(object))
            {
                yield break;
            }

            yield return type.BaseType();

            foreach (var ancestor in type.BaseType().GetParentTypes())
            {
                yield return ancestor;
            }
        }

        private static Type BaseType(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

#if UNITY_WSA && ENABLE_DOTNET && !UNITY_EDITOR
            return type.GetTypeInfo().BaseType;
#else
            return type.BaseType;
#endif
        }
    }
}
