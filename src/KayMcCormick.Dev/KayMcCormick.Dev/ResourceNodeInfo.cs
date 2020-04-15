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
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    /// <summary>
    ///     Class representing a node in the resource tree. Relatively generic
    ///     in that Key and Data refer to instances of type 'object' or in other
    ///     words any chosen type.
    /// </summary>
    public class ResourceNodeInfo : INotifyPropertyChanged
      , IEnumerable < ResourceNodeInfo >
      , IHierarchicalNode
    {
        private List < ResourceNodeInfo > _children = new List < ResourceNodeInfo > ( ) ;

        private Func < ResourceNodeInfo , object , object , bool ? , bool , ResourceNodeInfo > _createNodeFunc ;

        private object _data ;
        private int    _depth ;

        private Func < ResourceNodeInfo , Func < object , object , ResourceNodeInfo > ,
            IEnumerable < ResourceNodeInfo > > _getChildrenFunc ;

        private bool ? _internalIsExpanded ;
        private bool ? _isChildrenLoaded ;
        private bool ? _isValueChildren ;
        private object _key ;
        private object _styleKey ;
        private object _templateKey ;

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public virtual object Data { get { return _data ; } set { _data = value ; } }

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public virtual object Key { get { return _key ; } set { _key = value ; } }

        /// <summary>
        /// </summary>
        public virtual object TemplateKey
        {
            get { return _templateKey ; }
            set { _templateKey = value ; }
        }

        /// <summary>
        /// </summary>
        [ UsedImplicitly ] public object StyleKey
        {
            get { return _styleKey ; }
            set { _styleKey = value ; }
        }

        /// <summary>
        /// </summary>
        public bool ? IsValueChildren
        {
            // ReSharper disable once UnusedMember.Global
            get { return _isValueChildren ; }
            set { _isValueChildren = value ; }
        }

        /// <summary>
        /// </summary>
        public Func < ResourceNodeInfo , object , object , bool ? , bool, ResourceNodeInfo >
            CreateNodeFunc { get { return _createNodeFunc ; } set { _createNodeFunc = value ; } }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator < ResourceNodeInfo > GetEnumerator ( )
        {
            return _children.GetEnumerator ( ) ;
        }

        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return ( ( IEnumerable ) _children ).GetEnumerator ( ) ;
        }

        /// <summary>
        /// </summary>
        [ JsonIgnore ] [ CanBeNull ] public List < ResourceNodeInfo > Children
        {
            get
            {
                if ( _isChildrenLoaded.HasValue
                     && _isChildrenLoaded.Value == false )
                {
                    _children = _getChildrenFunc?.Invoke ( this , ( o , o1 ) => {
                                                              DebugUtils.WriteLine ( $"{o} {o1}" ) ;
                                                  var r = CreateNodeFunc (
                                                                                         this
                                                                                       , o
                                                                                       , o1
                                                                                       , false
                                                                                ,        false
                                                                                        ) ;
                                                  
                                                  return r ;
                                              }
                                             ).ToList() ;
                }

                return _children ;
            }
            set { _children = value ; }
        }

        /// <summary>
        /// </summary>
        public virtual bool IsExpanded
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

        /// <inheritdoc />
        public virtual bool ? IsChildrenLoaded
        {
            get { return _isChildrenLoaded ; }
            set { _isChildrenLoaded = value ; }
        }

        /// <summary>
        ///     Depth of node. 0 for a top-level node.
        /// </summary>
        public int Depth { get { return _depth ; } set { _depth = value ; } }

        /// <inheritdoc />
        public Func < ResourceNodeInfo , Func < object , object , ResourceNodeInfo > ,
            IEnumerable < ResourceNodeInfo > > GetChildrenFunc
        {
            get { return _getChildrenFunc ; }
            set { _getChildrenFunc = value ; }
        }

        /// <summary>
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return $"{new string ( ' ' , Depth )}Key: {_key}; Data: {_data}" ;
        }

        /// <summary>
        /// </summary>
        /// <param name="propertyName"></param>
        [ NotifyPropertyChangedInvocator ]
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}