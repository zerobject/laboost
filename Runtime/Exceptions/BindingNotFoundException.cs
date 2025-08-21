using System;

namespace Zerobject.Laboost.Runtime.Exceptions
{
    public class BindingNotFoundException : Exception
    {
        public BindingNotFoundException(Type requestedType) : base(
                                                                   $"Не удалось найти привязку с типом контракта '{requestedType.Name}'.")
        {
        }
    }
}