using System;
using System.Reflection;

namespace Zerobject.Laboost.Runtime.Extensions.Internal
{
    public static class AttributeExtenions
    {
        public static bool TryGetAttribute<T>(this MemberInfo member, out T attribute) where T : Attribute
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            attribute = member.GetCustomAttribute<T>();
            return attribute != null;
        }

        public static bool TryGetAttribute(this MemberInfo member, Type attributeType, out Attribute attribute)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException($"'{attributeType.Name}' не наследует '{nameof(Attribute)}'.");

            attribute = member.GetCustomAttribute(attributeType);
            return attribute != null;
        }

        public static bool TryGetAttribute<T>(this ParameterInfo param, out T attribute) where T : Attribute
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));
            
            attribute = param.GetCustomAttribute<T>();
            return attribute != null;
        }

        public static bool TryGetAttribute(this ParameterInfo param, Type attributeType, out Attribute attribute)
        {
            if (param == null)
                throw new ArgumentNullException(nameof(param));
            if (attributeType == null)
                throw new ArgumentNullException(nameof(attributeType));
            if (!typeof(Attribute).IsAssignableFrom(attributeType))
                throw new ArgumentException($"'{attributeType.Name}' не наследует '{nameof(Attribute)}'.");
            
            attribute = param.GetCustomAttribute(attributeType);
            return attribute != null;
        }
    }
}