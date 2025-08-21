using System;

namespace Zerobject.Laboost.Runtime.Injection.Units
{
    /// <summary>
    /// Base contract for all injection units in the dependency injection system.
    /// </summary>
    /// <remarks>
    /// Provides uniform access to injection metadata for all injection unit types.
    /// </remarks>
    public interface BaseInjectUnit
    {
        /// <summary>Type that declares the member to be injected.</summary>
        Type DeclaringType { get; }

        /// <summary>Type of dependency to be injected.</summary>
        Type InjectType { get; }

        /// <summary>Target type of injection (field, property, method, or parameter).</summary>
        InjectTarget TargetType { get; }

        /// <summary>Unique identifier of the injection unit.</summary>
        string Id { get; }
    }
}