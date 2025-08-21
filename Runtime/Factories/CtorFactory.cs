using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Extensions;

namespace Zerobject.Laboost.Runtime.Factories
{
    public class CtorFactory<T> : IFactory<T> where T : new()
    {
        private readonly Container m_Container;

        public CtorFactory(Container container)
        {
            m_Container = container;
        }

        public T Create()
        {
            T instance = new();
            m_Container.Inject(instance);
            return instance;
        }
    }
}