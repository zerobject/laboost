using System;

using Zerobject.Laboost.Runtime.Core;
using Zerobject.Laboost.Runtime.Factories;

namespace Zerobject.Laboost.Runtime.Builders
{
    public class BindingBuilder
    {
        internal readonly Container Container;
        internal readonly Type      ContractType;
        internal          object    Factory;
        internal          string    Id;
        internal          Type      ImplType;

        private  Binding m_CachedBinding;
        internal Scope   Scope;

        public BindingBuilder(Container container, Type contractType)
        {
            Container = container;
            ContractType = contractType;
            ImplType = null;
            Id = null;
            Factory = null;
            Scope = Scope.Transient;
        }

        internal void UpdateBinding()
        {
            ValidateBinding();
            m_CachedBinding = new(ContractType, ImplType, Id, Factory, Scope);
        }

        internal void FinalizeBinding()
        {
            UpdateBinding();
            Container.Bindings[(ContractType, Id)] = m_CachedBinding;
        }

        private void ValidateBinding()
        {
            ImplType ??= ContractType;

            if (Factory == null && ImplType != null && ImplType.GetConstructor(Type.EmptyTypes) != null)
            {
                var genFactoryType = typeof(CtorFactory<>).MakeGenericType(ImplType);
                Factory = Activator.CreateInstance(genFactoryType, Container);
            }
        }
    }
}