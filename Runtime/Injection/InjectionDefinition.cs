using System;
using System.Collections.Generic;
using Zerobject.Laboost.Runtime.Injection.Units;

namespace Zerobject.Laboost.Runtime.Injection
{
    /// <summary>
    /// Represents a set of dependency injections for a specific target type.
    /// </summary>
    /// <remarks>
    /// Stores the target type and a read-only list of <see cref="BaseInjectUnit"/>
    /// instances that define which members will receive injections.
    /// </remarks>
    [Serializable]
    public sealed class InjectionDefinition
    {
        /// <summary>Type of the target that receives injections.</summary>
        public readonly Type TargetType;

        /// <summary>Read-only list of injection units associated with the target.</summary>
        public readonly IReadOnlyList<BaseInjectUnit> Injections;

        /// <summary>Initializes a new <see cref="InjectionDefinition"/> instance.</summary>
        /// <param name="targetType">Type of the injection target.</param>
        /// <param name="injections">List of injection units for the target.</param>
        public InjectionDefinition(
            Type                          targetType,
            IReadOnlyList<BaseInjectUnit> injections
        )
        {
            TargetType = targetType;
            Injections = injections;
        }
    }
}