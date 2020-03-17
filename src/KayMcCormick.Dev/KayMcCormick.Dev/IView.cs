namespace KayMcCormick.Dev
{
    /// <summary>
    /// Indicates a view.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IView < out T >
    {
        /// <summary>
        /// Get the viewmodel associated with the view.
        /// </summary>
        T ViewModel { get ; }
    }
}