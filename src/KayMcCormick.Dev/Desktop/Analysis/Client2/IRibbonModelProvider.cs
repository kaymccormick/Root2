using Autofac;

namespace Client2
{
    public interface IRibbonModelProvider<T>
    {
        T ProvideModelItem(IComponentContext context);
    }
}