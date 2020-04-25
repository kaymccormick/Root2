#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Dev
// IHierarchicalNode.cs
// 
// 2020-04-14-8:11 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;

namespace KayMcCormick.Dev
{
    public interface IHierarchicalContainmentNode : IHierarchicalNodeElement
    {
        /// <summary>
        /// </summary>
        List < ResourceNodeInfo > Children { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        string ToString(bool v);
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IHierarchicalNodeElement
    {
        /// <summary>
        ///     Depth of node. 0 for a top-level node.
        /// </summary>
        int Depth { get ; set ; }

        /// <summary>
        /// </summary>
        object Key { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        IHierarchicalContainmentNode Parent { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        Guid Id { get ; set ; }
    }

    /// <summary>
    /// </summary>
    public interface IHierarchicalNode : IHierarchicalContainmentNode
    {
        /// <summary>
        /// </summary>
        bool ? IsExpanded
        {
            // ReSharper disable once UnusedMember.Global
            get ;
            set ;
        }

        int Ordinal { get ; set ; }
    }
}