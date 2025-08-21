using System;

namespace Zerobject.Laboost.Runtime.Exceptions
{
    public class FactoryTypeMismatchException : Exception
    {
        public FactoryTypeMismatchException(Type factoryReturnType, Type implType)
            : base($"Фабрика типа '{factoryReturnType.FullName}' " +
                   $"несовместима с типом реализации '{implType.FullName}'.")
        {
        }
    }
}