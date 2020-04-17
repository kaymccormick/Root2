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
using System.Collections.Generic ;

namespace KayMcCormick.Dev
{
    /// <summary>
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
        ///     Depth of node. 0 for a top-level node.
        /// </summary>
        int Depth { get ; set ; }
    }
}