using System;

namespace Zerobject.Laboost.Runtime.Core
{
    public readonly struct Binding
    {
        internal readonly Type   ContractType;
        internal readonly Type   ImplType;
        internal readonly string Id;
        internal readonly object Factory;
        internal readonly Scope  Scope;

        public Binding(Type contractType)
        {
            ContractType = contractType;
            ImplType     = null;
            Id           = null;
            Factory      = null;
            Scope        = Scope.Transient;
        }

        public Binding(Type contractType, Type implType, string id, object factory, Scope scope)
        {
            ContractType = contractType;
            ImplType     = implType;
            Id           = id;
            Factory      = factory;
            Scope        = scope;
        }
    }
}