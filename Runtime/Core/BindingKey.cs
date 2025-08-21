using System;

namespace Zerobject.Laboost.Runtime.Core
{
    public readonly struct BindingKey : IEquatable<BindingKey>
    {
        public readonly Type   ContractType;
        public readonly string Id;

        public BindingKey(Type contractType, string id)
        {
            ContractType = contractType;
            Id           = id;
        }

        public bool Equals(BindingKey other)
        {
            return ContractType == other.ContractType && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is BindingKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ContractType, Id);
        }

        public static bool operator ==(BindingKey left, BindingKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(BindingKey left, BindingKey right)
        {
            return !(left == right);
        }

        public static implicit operator (Type, string)(BindingKey key)
        {
            return (key.ContractType, key.Id);
        }

        public static implicit operator BindingKey((Type contractType, string id) tuple)
        {
            return new BindingKey(tuple.contractType, tuple.id);
        }
    }
}