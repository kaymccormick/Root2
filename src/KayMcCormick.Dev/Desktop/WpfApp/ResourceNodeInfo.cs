#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// WpfApp
// ResourceNodeInfo.cs
// 
// 2020-03-11-10:11 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;

namespace WpfApp
{
    public class ResourceNodeInfo
    {
        public ResourceNodeInfo ( ) {
        }

        private object                 _data ;
        private List<ResourceNodeInfo> _children = new List < ResourceNodeInfo > ();
        private Uri                    _sourceUri ;
        private object                 _key ;

        public object Data { get { return _data ; } set { _data = value ; } }

        public List<ResourceNodeInfo> Children { get { return _children ; } set { _children = value ; } }

        public Uri SourceUri { get { return _sourceUri ; } set { _sourceUri = value ; } }

        public object Key { get { return _key ; } set { _key = value ; } }
    }
}