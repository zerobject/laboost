using System;
using System.Reflection;

namespace Zerobject.Laboost.Runtime.Injection.Units
{
    /// <summary>Injection unit for a property.</summary>
    /// <remarks>Stores metadata for injecting dependencies into a property.</remarks>
    public class PropertyInjectUnit : BaseInjectUnit
    {
        /// <summary>Initializes a new <see cref="PropertyInjectUnit"/> instance.</summary>
        /// <param name="declaringType">Type declaring the property.</param>
        /// <param name="injectType">Type of dependency to inject.</param>
        /// <param name="id">Unique identifier for this unit.</param>
        /// <param name="property">Property information.</param>
        internal PropertyInjectUnit(
            Type         declaringType,
            Type         injectType,
            string       id,
            PropertyInfo property
        )
        {
            DeclaringType = declaringType;
            InjectType    = injectType;
            Id            = id;
            Property      = property;
        }

        /// <inheritdoc />
        public Type DeclaringType { get; }

        /// <inheritdoc />
        public Type InjectType { get; }

        /// <inheritdoc />
        public InjectTarget TargetType => InjectTarget.Property;

        /// <inheritdoc />
        public string Id { get; }

        /// <summary>Property info representing the property to be injected.</summary>
        public PropertyInfo Property { get; }
    }
}