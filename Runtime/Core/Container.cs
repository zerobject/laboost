using System;
using System.Collections.Generic;
using Zerobject.Laboost.Core.Factories;

namespace Zerobject.Laboost.Core
{
    public sealed class Container
    {
        private readonly Dictionary<Type, Binding> m_Bindings = new();
        private readonly Dictionary<Type, Type> m_ContractResolvers = new();

        public void Bind<T>(IFactory factory)
        {
            if (factory == null) throw new ArgumentException(nameof(factory));
            m_Bindings[typeof(T)] = new(factory, LifetimeType.Single);
        }

        public void BindInstance<T>(T target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            var factory = new InstanceFactory<T>(target);
            m_Bindings[typeof(T)] = new(factory, LifetimeType.Single) { Instance = target };
        }

        public T Resolve<T>()
        {
            var type = typeof(T);

            if (!m_Bindings.TryGetValue(type, out var binding))
                throw new InvalidOperationException($"Не найдена привязка с типом '{type}'.");

            switch (binding.LifetimeType)
            {
                case LifetimeType.Single:
                case LifetimeType.Scoped:
                    binding.Instance ??= binding.InstanceFactory.Create();
                    return (T)binding.Instance!;
                case LifetimeType.Transient:
                    return (T)binding.InstanceFactory.Create();
                default:
                    throw new NotSupportedException(
                        $"Неподдерживаемый тип жизненного цикла привязки ('{binding.LifetimeType}').");
            }
        }
    }
}