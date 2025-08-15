using System;

namespace Zerobject.Laboost.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {
        }
    }
}