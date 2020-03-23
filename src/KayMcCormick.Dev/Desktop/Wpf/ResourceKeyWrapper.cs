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
    public interface IResourceKeyWrapper < T >
        where T : ResourceKey
    {
        T ResourceKey { get ; set ; }
    }

    public class ResourceKeyWrapper < T > : IResourceKeyWrapper < T >, IResourceKeyWrapper1
        where T : ResourceKey
    {
        private T _resourceKey ; 

        public ResourceKeyWrapper ( T rkey ) { _resourceKey = rkey ; }

        public T ResourceKey { get { return _resourceKey ; } set { _resourceKey = value ; } }

        #region Implementation of IResourceKeyWrapper1
        public object ResourceKeyObject
        {
            get { return ResourceKey ; }
        }
        #endregion
    }

    public interface IResourceKeyWrapper1
    {
        object ResourceKeyObject { get ; }
    }
}