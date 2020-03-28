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
using System.Diagnostics ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    public class ResourceNodeInfo : INotifyPropertyChanged
    {
        private List < ResourceNodeInfo > _children = new List < ResourceNodeInfo > ( ) ;
        private object                    _data ;


        private bool ? _internalIsExpanded ;
        private bool ? _isValueChildren ;
        private object _key ;
        private Uri    _sourceUri ;
        private object _styleKey ;

        private object _templateKey ;

        [ JsonIgnore ]
        public object Data { get { return _data ; } set { _data = value ; } }

        [ JsonIgnore ]
        public List < ResourceNodeInfo > Children
        {
            get { return _children ; }
            set { _children = value ; }
        }

        public Uri SourceUri { get { return _sourceUri ; } set { _sourceUri = value ; } }

        [ JsonIgnore ]
        public object Key { get { return _key ; } set { _key = value ; } }

        public object TemplateKey { get { return _templateKey ; } set { _templateKey = value ; } }

        public bool IsExpanded
        {
            get { return _internalIsExpanded.GetValueOrDefault ( ) ; }
            set
            {
                Debug.WriteLine ( $"isExpanded = {value} for {Key.ToString ( )}" ) ;
                _internalIsExpanded = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public object StyleKey { get { return _styleKey ; } set { _styleKey = value ; } }

        public bool ? IsValueChildren
        {
            get { return _isValueChildren ; }
            set { _isValueChildren = value ; }
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        public override string ToString ( )
        {
            return $"{nameof ( _data )}: {_data}, {nameof ( _key )}: {_key}" ;
        }

        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}