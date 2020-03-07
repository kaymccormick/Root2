namespace ProjLib
{
    public interface IView < out T >
    {
        T ViewModel { get ; }
    }
}