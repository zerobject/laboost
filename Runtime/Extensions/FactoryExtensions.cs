namespace Zerobject.Laboost.Runtime.Extensions
{
    public static class FactoryExtensions
    {
        private const string FactoryCreateMethodName = "Create";

        public static object Create(this object factory)
        {
            var factoryType = factory.GetType();
            var method      = factoryType.GetMethod(FactoryCreateMethodName);

            return method?.Invoke(factory, null);
        }
    }
}