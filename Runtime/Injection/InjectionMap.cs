using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Zerobject.Laboost.Runtime.Injection.Units;

namespace Zerobject.Laboost.Runtime.Injection
{
    /// <summary>
    /// Caches and provides access to injection definitions and their consumers.
    /// </summary>
    /// <remarks>
    /// Maintains lookup dictionaries for targets by type and by ID, and provides
    /// helper methods to retrieve injections for fields, properties, methods, and parameters.
    /// </remarks>
    public static class InjectionMap
    {
        private static Dictionary<Type, InjectionDefinition>         m_DefinitionsByType;
        private static Dictionary<Type, List<InjectionDefinition>>   m_ConsumersByType;
        private static Dictionary<string, List<InjectionDefinition>> m_ConsumersById;

        /// <summary>Read-only dictionary of injection definitions keyed by target type.</summary>
        public static IReadOnlyDictionary<Type, InjectionDefinition> DefinitionsByType => m_DefinitionsByType;

        /// <summary>Read-only dictionary of consumers keyed by injected type.</summary>
        public static IReadOnlyDictionary<Type, List<InjectionDefinition>> ConsumersByType => m_ConsumersByType;

        /// <summary>Read-only dictionary of consumers keyed by injection ID.</summary>
        public static IReadOnlyDictionary<string, List<InjectionDefinition>> ConsumersById => m_ConsumersById;

        static InjectionMap()
        {
            m_DefinitionsByType = new();
            m_ConsumersByType   = new();
            m_ConsumersById     = new();
        }

        /// <summary>Updates internal caches with the provided injection definitions.</summary>
        /// <param name="definitions">Collection of injection definitions to cache.</param>
        internal static void UpdateCache(IEnumerable<InjectionDefinition> definitions)
        {
            Dictionary<Type, InjectionDefinition>         newDefByType       = new();
            Dictionary<Type, List<InjectionDefinition>>   newConsumersByType = new();
            Dictionary<string, List<InjectionDefinition>> newConsumersById   = new();

            foreach (var def in definitions)
            {
                newDefByType[def.TargetType] = def;

                foreach (var injection in def.Injections)
                {
                    if (newConsumersByType.TryGetValue(injection.InjectType, out var list))
                    {
                        list                                     = new();
                        newConsumersByType[injection.InjectType] = list;
                    }

                    list!.Add(def);

                    if (string.IsNullOrEmpty(injection.Id)) continue;

                    if (!newConsumersById.TryGetValue(injection.Id, out var idList))
                    {
                        idList                         = new();
                        newConsumersById[injection.Id] = idList;
                    }

                    idList!.Add(def);
                }

                m_DefinitionsByType = newDefByType;
                m_ConsumersByType   = newConsumersByType;
                m_ConsumersById     = newConsumersById;
            }
        }

        /// <summary>Attempts to get the injection definition for a specific target type.</summary>
        [PublicAPI]
        public static bool TryGetDefinition<T>(out InjectionDefinition def)
            => TryGetDefinition(typeof(T), out def);

        /// <summary>Attempts to get the injection definition for a specific target type.</summary>
        [PublicAPI]
        public static bool TryGetDefinition(Type targetType, out InjectionDefinition def)
            => m_DefinitionsByType.TryGetValue(targetType, out def);

        [PublicAPI]
        public static bool TryGetDefinitionsByInjectType<T>(out IReadOnlyList<InjectionDefinition> defs)
            => TryGetDefinitionsByInjectType(typeof(T), out defs);

        /// <summary>Attempts to get all injection definitions for a given injected type.</summary>
        [PublicAPI]
        public static bool TryGetDefinitionsByInjectType(Type injectType, out IReadOnlyList<InjectionDefinition> defs)
        {
            if (m_ConsumersByType.TryGetValue(injectType, out var list))
            {
                defs = list;
                return true;
            }

            defs = null;
            return false;
        }

        /// <summary>Attempts to get all injection definitions associated with a specific ID.</summary>
        [PublicAPI]
        public static bool TryGetDefinitionById(string id, out IReadOnlyList<InjectionDefinition> defs)
        {
            if (m_ConsumersById.TryGetValue(id, out var list))
            {
                defs = list;
                return true;
            }

            defs = null;
            return false;
        }

        #region Injection Receiving Methods

        /// <summary>Gets all field injections for a target type.</summary>
        [PublicAPI]
        public static IEnumerable<FieldInjectUnit> GetFieldInjections(Type targetType)
            => GetSpecificUnitInjections<FieldInjectUnit>(targetType);
        
        /// <summary>Gets all property injections for a target type.</summary>
        [PublicAPI]
        public static IEnumerable<PropertyInjectUnit> GetPropertyInjections(Type targetType)
            => GetSpecificUnitInjections<PropertyInjectUnit>(targetType);

        /// <summary>Gets all method injections for a target type.</summary>
        [PublicAPI]
        public static IEnumerable<MethodInjectUnit> GetMethodInjections(Type targetType)
            => GetSpecificUnitInjections<MethodInjectUnit>(targetType);

        /// <summary>Gets all parameter injections for a target type.</summary>
        [PublicAPI]
        public static IEnumerable<ParamInjectUnit> GetParamInjections(Type targetType)
            => GetSpecificUnitInjections<ParamInjectUnit>(targetType);

        /// <summary>Gets injections of a specific unit type for a target type.</summary>
        /// <typeparam name="TUnit">The injection unit type.</typeparam>
        [PublicAPI]
        public static IEnumerable<TUnit> GetSpecificUnitInjections<TUnit>(Type targetType)
            where TUnit : BaseInjectUnit
            => TryGetDefinition(targetType, out var def)
                ? def.Injections.OfType<TUnit>()
                : Enumerable.Empty<TUnit>();

        #endregion
    }
}