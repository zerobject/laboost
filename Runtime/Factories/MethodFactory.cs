using System;
using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Extensions;

namespace Zerobject.Laboost.Runtime.Factories
{
    public delegate T FactoryMethod<out T>();

    public class MethodFactory<T> : IFactory<T>
    {
        private readonly Container        m_Container;
        private readonly FactoryMethod<T> m_Method;

        public MethodFactory(Container container, FactoryMethod<T> method)
        {
            m_Container = container;
            m_Method    = method ?? throw new ArgumentNullException(nameof(method));
        }

        public T Create()
        {
            var instance = m_Method();
            m_Container.Inject(instance);
            return instance;
        }
    }
}