using System;

namespace Zerobject.Laboost.Runtime.Exceptions
{
    public class BindingTypesMismatchException : Exception
    {
        public BindingTypesMismatchException(Type contractType, Type implType)
            : base($"Тип '{implType.FullName}' не реализует контракт '{contractType.FullName}'.")
        {
        }
    }
}