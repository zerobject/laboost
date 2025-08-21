using System;
using System.Reflection;

namespace Zerobject.Laboost.Runtime.Injection.Units
{
    /// <summary>Injection unit for a method.</summary>
    /// <remarks>Stores metadata for injecting dependencies into a method.</remarks>
    public class MethodInjectUnit : BaseInjectUnit
    {
        /// <summary>Initializes a new <see cref="MethodInjectUnit"/> instance.</summary>
        /// <param name="declaringType">Type declaring the method.</param>
        /// <param name="injectType">Type of dependency to inject.</param>
        /// <param name="id">Unique identifier for this unit.</param>
        /// <param name="method">Method information.</param>
        internal MethodInjectUnit(
            Type       declaringType,
            Type       injectType,
            string     id,
            MethodInfo method
        )
        {
            DeclaringType = declaringType;
            InjectType    = injectType;
            Id            = id;
            Method        = method;
        }

        /// <inheritdoc />
        public Type DeclaringType { get; }

        /// <inheritdoc />
        public Type InjectType { get; }

        /// <inheritdoc />
        public InjectTarget TargetType => InjectTarget.Method;

        /// <inheritdoc />
        public string Id { get; }

        /// <summary>Method info representing the method to be injected.</summary>
        public MethodInfo Method { get; }
    }
}