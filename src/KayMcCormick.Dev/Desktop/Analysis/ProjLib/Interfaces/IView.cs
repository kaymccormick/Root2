namespace ProjLib.Interfaces
{
    public interface IView < out T >
    {
        T ViewModel { get ; }
    }
}