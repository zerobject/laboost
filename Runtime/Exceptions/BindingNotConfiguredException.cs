using System;

namespace Zerobject.Laboost.Runtime.Exceptions
{
    public class BindingNotConfiguredException : Exception
    {
        public BindingNotConfiguredException(Type contractType)
            : base($"Для контракта '{contractType.FullName}' не задан ImplType. " +
                   "Вызов методов From[...](...) невозможен.")
        {
        }
    }
}