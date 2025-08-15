using System;

namespace Zerobject.Laboost.Core.Factories
{
    public abstract class FactoryBase<T> : IFactory
    {
        public abstract object Create();

        protected static object CreateDefault()
        {
            return Activator.CreateInstance<T>()!;
        }
    }

    public class DefaultFactory<T> : FactoryBase<T>
    {
        public override object Create()
        {
            return CreateDefault();
        }
    }

    public class InstanceFactory<T> : FactoryBase<T>
    {
        private readonly T m_Instance;

        public InstanceFactory(T instance)
        {
            m_Instance = instance;
        }

        public override object Create()
        {
            return m_Instance!;
        }

        public T CreateInstance()
        {
            return m_Instance;
        }
    }
}