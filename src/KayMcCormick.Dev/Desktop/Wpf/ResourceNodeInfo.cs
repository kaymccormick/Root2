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
    /// <summary>
    /// </summary>
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

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public object Data { get { return _data ; } set { _data = value ; } }

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public List < ResourceNodeInfo > Children
        {
            get { return _children ; }
            set { _children = value ; }
        }

        /// <summary>
        /// </summary>
        public Uri SourceUri { get { return _sourceUri ; } set { _sourceUri = value ; } }

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public object Key { get { return _key ; } set { _key = value ; } }

        /// <summary>
        /// </summary>
        public object TemplateKey { get { return _templateKey ; } set { _templateKey = value ; } }

        /// <summary>
        /// </summary>
        public bool IsExpanded
        {
            get { return _internalIsExpanded.GetValueOrDefault ( ) ; }
            set
            {
                Debug.WriteLine ( $"isExpanded = {value} for {Key}" ) ;
                _internalIsExpanded = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        public object StyleKey { get { return _styleKey ; } set { _styleKey = value ; } }

        /// <summary>
        /// </summary>
        public bool ? IsValueChildren
        {
            get { return _isValueChildren ; }
            set { _isValueChildren = value ; }
        }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return $"{nameof ( _data )}: {_data}, {nameof ( _key )}: {_key}" ;
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyName"></param>
        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}