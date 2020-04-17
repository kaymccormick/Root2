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
    public sealed class ResourceNodeInfo : ResourceNodeInfoBase
      , INotifyPropertyChanged
      , IEnumerable < ResourceNodeInfo >
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
        /// 
        /// </summary>
        public bool ? IsChildrenLoaded
        {
            get { return _isChildrenLoaded ; }
            set { _isChildrenLoaded = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Func < ResourceNodeInfo , Func < object , object , ResourceNodeInfo > ,
            IEnumerable < ResourceNodeInfo > > GetChildrenFunc
        {
            get { return _getChildrenFunc ; }
            set { _getChildrenFunc = value ; }
        }

        /// <summary>
        /// </summary>
        [ JsonIgnore ] [ NotNull ] public List < ResourceNodeInfo > Children
        {
            get
            {
                if ( ! _isChildrenLoaded.HasValue
                     || _isChildrenLoaded.Value != false )
                {
                    return _children ;
                }

                DebugUtils.WriteLine ( "Expanding children" ) ;
                _children = Enumerable.ToList < ResourceNodeInfo > (
                                                                    _getChildrenFunc?.Invoke ( this , ( o , o1 ) => {
                                                                                                  DebugUtils.WriteLine ( $"creating node for {o} {o1}" ) ;
                                                                                                  var r = CreateNodeFunc (
                                                                                                                          this
                                                                                                                        , o
                                                                                                                        , o1
                                                                                                                        , false
                                                                                                                 ,        false
                                                                                                                         ) ;

                                                                                                  return r ;
                                                                                              }
                                                                                             )
                                                                   ) ;
                // ReSharper disable once PossibleNullReferenceException
                foreach ( var resourceNodeInfo in _children )
                {
                    DebugUtils.WriteLine($"{resourceNodeInfo}");
                }

                // ReSharper disable once AssignNullToNotNullAttribute
                return _children ;
            }
            set { _children = value ; }
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

    /// <summary>
    /// 
    /// </summary>
    public interface IProvidesKey
    {
        /// <summary>
        ///
        /// 
        /// </summary>
        /// <returns></returns>
        object GetKey ( ) ;
    }
}