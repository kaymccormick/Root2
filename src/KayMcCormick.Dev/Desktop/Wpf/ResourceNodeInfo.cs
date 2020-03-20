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
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceNodeInfo : INotifyPropertyChanged
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

        public override string ToString ( )
        {
            return $"{nameof ( _data )}: {_data}, {nameof ( _key )}: {_key}" ;
        }

        
        private bool? _internalIsExpanded ;

        public bool IsExpanded
        {
            get
            {
                return _internalIsExpanded.GetValueOrDefault ( ) ;
            }
            set
            {
                _internalIsExpanded = value ;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}