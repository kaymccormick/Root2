using Autofac;
using KayMcCormick.Dev.Interfaces;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRibbonModelProvider<T> : IModelProvider, IHaveObjectId
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        T ProvideModelItem();
    }

    public interface IModelProvider
    {
    }
}