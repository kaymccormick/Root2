namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IView < out T >
    {
        /// <summary>
        /// 
        /// </summary>
        T ViewModel { get ; }
    }
}
