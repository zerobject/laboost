namespace Zerobject.Laboost.Runtime.Factories
{
    public interface IFactory<out T>
    {
        T Create();
    }
}