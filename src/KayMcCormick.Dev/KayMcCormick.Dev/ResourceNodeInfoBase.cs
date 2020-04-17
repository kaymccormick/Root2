#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// ResourceNodeInfoBase.cs
// 
// 2020-04-17-1:41 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceNodeInfoBase : IHierarchicalNode
    {
        private object _data ;
        private object _key ;
        private object _templateKey ;
        private object _styleKey ;
        private bool? _isValueChildren ;
        private List < ResourceNodeInfo > _children= new List < ResourceNodeInfo > ();
        private int _depth ;

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public object Data { get { return _data ; } set { _data = value ; } }

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public object Key {
            get
            {
                if ( _key is IProvidesKey pk )
                {
                    return pk.GetKey ( ) ;
                }
                return _key ;
            } set { _key = value ; } }

        /// <summary>
        /// </summary>
        public object TemplateKey
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

        /// <inheritdoc />
        public List < ResourceNodeInfo > Children { get { return _children ; } set { _children = value ; } }

        /// <summary>
        /// </summary>
        public bool IsExpanded { get ; set ; }

        /// <summary>
        ///     Depth of node. 0 for a top-level node.
        /// </summary>
        public int Depth { get { return _depth ; } set { _depth = value ; } }
    }
}