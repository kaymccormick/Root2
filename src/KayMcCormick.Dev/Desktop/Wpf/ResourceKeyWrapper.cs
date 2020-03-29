#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// ResourceKeyWrapper.cs
// 
// 2020-03-19-3:11 PM
// 
// ---
#endregion
using System.Windows ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IResourceKeyWrapper < T >
        where T : ResourceKey
    {
        /// <summary>
        /// 
        /// </summary>
        T ResourceKey { get ; set ; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResourceKeyWrapper < T > : IResourceKeyWrapper < T >, IResourceKeyWrapper1
        where T : ResourceKey
    {
        private T _resourceKey ; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rkey"></param>
        public ResourceKeyWrapper ( T rkey ) { _resourceKey = rkey ; }

        /// <summary>
        /// 
        /// </summary>
        public T ResourceKey { get { return _resourceKey ; } set { _resourceKey = value ; } }

        #region Implementation of IResourceKeyWrapper1
        /// <summary>
        /// 
        /// </summary>
        public object ResourceKeyObject
        {
            get { return ResourceKey ; }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IResourceKeyWrapper1
    {
        /// <summary>
        /// 
        /// </summary>
        object ResourceKeyObject { get ; }
    }
}