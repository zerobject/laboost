using System;
using AttrTargets = System.AttributeTargets;

namespace Zerobject.Laboost.Runtime.Attributes
{
    [AttributeUsage(AttrTargets.Field | AttrTargets.Property | AttrTargets.Method | AttrTargets.Parameter)]
    public sealed class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {
        }

        public InjectAttribute(string id) : this()
        {
            Id = id;
        }

        public InjectAttribute(string id, bool optional = false) : this(id)
        {
            Optional = optional;
        }

        public string Id       { get; }
        public bool   Optional { get; }
    }
}