namespace KayMcCormick.Dev
{
    public interface IView < out T >
    {
        T ViewModel { get ; }
    }
}