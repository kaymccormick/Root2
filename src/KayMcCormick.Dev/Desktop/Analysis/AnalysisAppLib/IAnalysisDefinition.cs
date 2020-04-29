using System;

namespace AnalysisAppLib
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TOutput"></typeparam>
    // ReSharper disable once UnusedTypeParameter
    internal interface IAnalysisDefinition < TOutput >
    {
        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        Type DataflowOutputType { get ; set ; }
    }
}