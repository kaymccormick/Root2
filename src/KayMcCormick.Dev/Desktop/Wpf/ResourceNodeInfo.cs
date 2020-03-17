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
using System.Text.Json.Serialization ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceNodeInfo
    {
        private object                 _data ;
        private List<ResourceNodeInfo> _children = new List < ResourceNodeInfo > ();
        private Uri                    _sourceUri ;
        private object                 _key ;
        [JsonIgnore]
        public object Data { get { return _data ; } set { _data = value ; } }
        [JsonIgnore]
        public List<ResourceNodeInfo> Children { get { return _children ; } set { _children = value ; } }

        public Uri SourceUri { get { return _sourceUri ; } set { _sourceUri = value ; } }

        [JsonIgnore]
        public object Key { get { return _key ; } set { _key = value ; } }

        public object TemplateKey { get => _templateKey ; set => _templateKey = value ; }

        private object _templateKey ;
    }
}