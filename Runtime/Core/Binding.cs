using System;
using JetBrains.Annotations;
using Zerobject.Laboost.Core.Factories;

namespace Zerobject.Laboost.Core
{
    public struct Binding : IEquatable<Binding>
    {
        public Binding(IFactory factory, LifetimeType lifetimeType)
        {
            InstanceFactory = factory ?? throw new ArgumentNullException(nameof(factory));
            Instance = null;
            LifetimeType = lifetimeType;
        }

        public IFactory InstanceFactory { get; }
        [CanBeNull] public object Instance { get; set; }
        public LifetimeType LifetimeType { get; }

        public bool Equals(Binding other)
        {
            return Equals(InstanceFactory, other.InstanceFactory)
                   && Equals(Instance, other.Instance)
                   && LifetimeType == other.LifetimeType;
        }

        public override bool Equals(object obj)
        {
            return obj is Binding other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(InstanceFactory, Instance, (int)LifetimeType);
        }
    }
}