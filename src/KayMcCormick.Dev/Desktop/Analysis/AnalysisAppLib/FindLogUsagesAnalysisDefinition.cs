#if FINDLOGUSAGES
using System;
using FindLogUsages;
using KayMcCormick.Dev.Attributes;

namespace AnalysisAppLib
{
    /// <summary>
    /// </summary>
    [ TitleMetadata ( "Find and analyze usages of NLog logging." ) ]
    // ReSharper disable once UnusedType.Global
    public sealed class FindLogUsagesAnalysisDefinition : IAnalysisDefinition < ILogInvocation >
    {
        private Type _dataflowOutputType = typeof ( ILogInvocation ) ;

        /// <summary>
        /// </summary>
        public Type DataflowOutputType
        {
            get { return _dataflowOutputType ; }
            set { _dataflowOutputType = value ; }
        }
    }
}
#endif