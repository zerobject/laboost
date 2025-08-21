using System.Collections.Generic;

namespace Zerobject.Laboost.Runtime.Core
{
    public sealed class Container
    {
        internal readonly Dictionary<BindingKey, Binding> Bindings        = new();
        internal readonly Dictionary<BindingKey, object>  CachedInstances = new();
        internal readonly List<Container>                 Children        = new();
        internal readonly Container                       Parent;

        internal readonly Dictionary<BindingKey, object> SingleInstances = new();

        public Container(Container parent = null)
        {
            Parent = parent;
            Parent?.Children.Add(this);
        }
    }
}