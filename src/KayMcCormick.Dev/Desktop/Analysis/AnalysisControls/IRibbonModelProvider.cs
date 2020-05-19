using Autofac;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRibbonModelProvider<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T ProvideModelItem();
    }

    
}