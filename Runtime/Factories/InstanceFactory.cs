using System;
using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Extensions;

namespace Zerobject.Laboost.Runtime.Factories
{
    public class InstanceFactory<T> : IFactory<T>
    {
        private readonly Container m_Container;
        private readonly T         m_Instance;

        public InstanceFactory(Container container, T instance)
        {
            m_Container = container;
            m_Instance  = instance ?? throw new ArgumentNullException(nameof(instance));
        }

        public T Create()
        {
            m_Container.Inject(m_Instance);
            return m_Instance;
        }
    }
}