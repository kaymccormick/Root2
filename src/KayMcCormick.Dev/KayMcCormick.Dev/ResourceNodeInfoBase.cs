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
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Text.Json.Serialization ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class 
        ResourceNodeInfoBase : IHierarchicalNode, INotifyPropertyChanged
    {
        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public virtual object Data { get ; set ; }

        /// <summary>
        /// </summary>
        [ JsonIgnore ]
        public virtual object Key { get ; set ; }

        /// <summary>
        /// </summary>
        public virtual object TemplateKey { get ; set ; }

        /// <summary>
        /// </summary>
        [ UsedImplicitly ] public virtual object StyleKey { get ; set ; }

        /// <summary>
        /// </summary>
        public virtual bool? IsValueChildren { get ; set ; }

        /// <inheritdoc />
        public virtual List < ResourceNodeInfo > Children { get ; set ; } = new List < ResourceNodeInfo > ();

        /// <summary>
        /// </summary>
        public virtual bool? IsExpanded { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int Ordinal { get ; set ; }

        /// <summary>
        ///     Depth of node. 0 for a top-level node.
        /// </summary>
        public virtual int Depth { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        [ JsonIgnore ]
        public virtual IHierarchicalContainmentNode Parent { get ; set ; } = null ;

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid Id { get ; set ; }
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [ NotifyPropertyChangedInvocator ]
        protected virtual void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodeInfo"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void AddChild ( [ JetBrains.Annotations.NotNull ] IHierarchicalNode nodeInfo )
        {
            if ( nodeInfo == null )
            {
                throw new ArgumentNullException ( nameof ( nodeInfo ) ) ;
            }

            nodeInfo.Parent = this ;
            Children.Add ( (ResourceNodeInfo)nodeInfo ) ;
            nodeInfo.Ordinal = Children.Count - 1 ;
        }

        /// <summary>
        /// <param name="b"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public string ToString ( bool b )
        {
            var me = $"{{{Id}}}: Key: {Key}; Data: {Data}" ;
            if ( b == false )
            {
                return me ;
            }

            return
                $"{Depth}[{Ordinal}];Parent={Parent?.ToString(false)};{new string ( ' ' , Depth )}: {me}" ;
        }

        #region Overrides of Object
        /// <inheritdoc />
        public override string ToString() { return ToString(true); }
        #endregion

    }
}