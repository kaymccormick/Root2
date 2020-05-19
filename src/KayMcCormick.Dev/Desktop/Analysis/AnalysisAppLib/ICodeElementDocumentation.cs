using System;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeElementDocumentation
    {
        /// <summary>
        /// </summary>
        string ElementId { get; set; }

        /// <summary>
        /// </summary>
// ReSharper disable once MemberCanBeProtected.Global
        Type Type { get; set; }

        /// <summary>
        /// Flag indicating that this documentation element needs attention.
        /// </summary>
        bool NeedsAttention { get; set; }
    }
}