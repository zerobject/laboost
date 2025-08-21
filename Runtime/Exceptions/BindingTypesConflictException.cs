using System;

namespace Zerobject.Laboost.Runtime.Exceptions
{
    public class BindingTypesConflictException : Exception
    {
        public BindingTypesConflictException(Type existingImplType, Type providedInstanceType)
            : base($"ImplType уже задан как '{existingImplType.FullName}', " +
                   $"но передаваемый экземпляр имеет тип ' {providedInstanceType.FullName}.")
        {
        }
    }
}