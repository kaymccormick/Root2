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
using System.Collections ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    /// <summary>Class representing a node in the resource tree. Relatively generic in that Key and Data refer to instances of type 'object' or in other words any chosen type.</summary>
    public sealed class ResourceNodeInfo : INotifyPropertyChanged, IEnumerable <ResourceNodeInfo>, IHierarchicalNode
    {
        private int _depth ;
        private List < ResourceNodeInfo > _children = new List < ResourceNodeInfo > ( ) ;
        private object                    _data ;


        private bool ? _internalIsExpanded ;
        private bool ? _isValueChildren ;
        private object _key ;
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
        [ JsonIgnore ]
        public object Key { get { return _key ; } set { _key = value ; } }

        /// <summary>
        /// </summary>
        public object TemplateKey { get { return _templateKey ; } set { _templateKey = value ; } }

        /// <summary>
        /// </summary>
        public bool IsExpanded
        {
            // ReSharper disable once UnusedMember.Global
            get { return _internalIsExpanded.GetValueOrDefault ( ) ; }
            set
            {
                DebugUtils.WriteLine ( $"isExpanded = {value} for {Key}" ) ;
                _internalIsExpanded = value ;
                OnPropertyChanged ( ) ;
            }
        }

        /// <summary>
        /// </summary>
        [ UsedImplicitly ] public object StyleKey { get { return _styleKey ; } set { _styleKey = value ; } }

        /// <summary>
        /// </summary>
        public bool ? IsValueChildren
        {
            // ReSharper disable once UnusedMember.Global
            get { return _isValueChildren ; }
            set { _isValueChildren = value ; }
        }

        /// <summary>
        /// Depth of node. 0 for a top-level node.
        /// </summary>
        public int Depth { get { return _depth ; } set { _depth = value ; } }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator < ResourceNodeInfo > GetEnumerator ( ) { return _children.GetEnumerator ( ) ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return $"{new String(' ', Depth)}Key: {_key}; Data: {_data}" ;
        }

        IEnumerator IEnumerable.GetEnumerator ( ) { return ( ( IEnumerable ) _children ).GetEnumerator ( ) ; }

        /// <summary>
        /// </summary>
        /// <param name="propertyName"></param>
        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IHierarchicalNode
    {
        /// <summary>
        /// </summary>
        List < ResourceNodeInfo > Children { get ; set ; }

        /// <summary>
        /// </summary>
        bool IsExpanded
        {
            // ReSharper disable once UnusedMember.Global
            get ;
            set ;
        }

        /// <summary>
        /// Depth of node. 0 for a top-level node.
        /// </summary>
        int Depth { get ; set ; }
    }
}