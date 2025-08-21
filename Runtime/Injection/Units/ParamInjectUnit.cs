using System;
using System.Reflection;

namespace Zerobject.Laboost.Runtime.Injection.Units
{
    /// <summary>Injection unit for a method parameter.</summary>
    /// <remarks>Stores metadata for injecting dependencies into a method parameter.</remarks>
    public class ParamInjectUnit : BaseInjectUnit
    {
        /// <summary>Initializes a new <see cref="ParamInjectUnit"/> instance.</summary>
        /// <param name="declaringType">Type declaring the method containing the parameter.</param>
        /// <param name="injectType">Type of dependency to inject.</param>
        /// <param name="id">Unique identifier for this unit.</param>
        /// <param name="param">Parameter information.</param>
        internal ParamInjectUnit(
            Type          declaringType,
            Type          injectType,
            string        id,
            ParameterInfo param
        )
        {
            DeclaringType = declaringType;
            InjectType    = injectType;
            Id            = id;
            Param         = param;
        }

        /// <inheritdoc />
        public Type DeclaringType { get; }

        /// <inheritdoc />
        public Type InjectType { get; }

        /// <inheritdoc />
        public InjectTarget TargetType => InjectTarget.Param;

        /// <inheritdoc />
        public string Id { get; }

        /// <summary>Parameter info representing the parameter to be injected.</summary>
        public ParameterInfo Param { get; }
    }
}