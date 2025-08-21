using System;
using System.Reflection;

namespace Zerobject.Laboost.Runtime.Injection.Units
{
    /// <summary>Injection unit for a field.</summary>
    /// <remarks>Contains metadata for injecting a dependency into a field.</remarks>
    public class FieldInjectUnit : BaseInjectUnit
    {
        /// <summary>Initializes a new <see cref="FieldInjectUnit"/> instance.</summary>
        /// <param name="declaringType">Type declaring the field.</param>
        /// <param name="injectType">Type of dependency to inject.</param>
        /// <param name="id">Unique identifier for this unit.</param>
        /// <param name="field">Field information.</param>
        internal FieldInjectUnit(
            Type      declaringType,
            Type      injectType,
            string    id,
            FieldInfo field
        )
        {
            DeclaringType = declaringType;
            InjectType    = injectType;
            Id            = id;
            Field         = field;
        }

        /// <inheritdoc />
        public Type DeclaringType { get; }

        /// <inheritdoc />
        public Type InjectType { get; }

        /// <inheritdoc />
        public InjectTarget TargetType => InjectTarget.Field;

        /// <inheritdoc />
        public string Id { get; }

        /// <summary>Field info representing the field to be injected.</summary>
        public FieldInfo Field { get; }
    }
}