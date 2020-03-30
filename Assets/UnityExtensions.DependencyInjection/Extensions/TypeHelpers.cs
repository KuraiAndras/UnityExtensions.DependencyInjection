using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace UnityExtensions.DependencyInjection.Extensions
{
    internal static class TypeHelpers
    {
        internal static bool IsAutoProperty(this PropertyInfo property)
        {
            if (property is null) throw new ArgumentNullException(nameof(property));

            return property
                       .DeclaringType
                       ?.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                       .Any(f => f.Name.Contains("<" + property.Name + ">"))
                   ?? false;
        }

        internal static FieldInfo GetAutoPropertyBackingField(this PropertyInfo property)
        {
            if (property is null) throw new ArgumentNullException(nameof(property));

            return property
                .DeclaringType
                ?.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                .Single(f => f.Name.Contains("<" + property.Name + ">"));
        }

        internal static bool MemberHasInjectAttribute<T>(this T memberInfo) where T : MemberInfo
        {
            if (memberInfo is null) throw new ArgumentNullException(nameof(memberInfo));

            return memberInfo.GetCustomAttributes<InjectAttribute>().Any();
        }

        internal static IEnumerable<Type> GetAllTypes(this Type type)
        {
            yield return type;

            foreach (var parentType in type.GetParentTypes())
            {
                yield return parentType;
            }
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

        public static IEnumerable<T> FilterMembers<T>(this IEnumerable<T> members) where T : MemberInfo => members.Where(MemberHasInjectAttribute).Distinct();

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var element in source)
                action(element);
        }
    }
}
