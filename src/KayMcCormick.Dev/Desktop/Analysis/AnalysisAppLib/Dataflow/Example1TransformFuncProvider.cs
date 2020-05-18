#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// Example1TransformFuncProvider.cs
// 
// 2020-04-13-11:16 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Threading.Tasks ;
using System.Threading.Tasks.Dataflow ;
using FindLogUsages ;
using JetBrains.Annotations ;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp;


namespace AnalysisAppLib.Dataflow
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Example1TransformFuncProvider : DataflowTransformFuncProvider <
            Document , Example1Out >
      , IHaveRejectBlock
    {
        private readonly Func < Document , Task < IEnumerable < Example1Out> > >
            _transformFunc ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocationFactory"></param>
        // ReSharper disable once UnusedParameter.Local
        public Example1TransformFuncProvider ( Func < Example1Out> invocationFactory )
        {


            RejectBlock    = new BufferBlock < RejectedItem > ( ) ;
            _transformFunc = document => Transform( document , RejectBlock ) ;
        }

        [ ItemNotNull ]
        private async Task< IEnumerable < Example1Out > > Transform (
            [ NotNull ] Document                     document
            // ReSharper disable once UnusedParameter.Local
          , BufferBlock < RejectedItem > rejectBlock
        )
        {

            // ReSharper disable once UnusedVariable
            var opt = await document.GetOptionsAsync ( ) ;
            var model = await document.GetSemanticModelAsync ( ) ;
            var tree = await document.GetSyntaxTreeAsync ( ) ;
            if ( tree == null ) { throw new AppInvalidOperationException("tree is null");}

            var x1 = await tree.GetRootAsync ( ) ;
            var return1 = new List < Example1Out > ( ) ;

            foreach ( var descendantNode in x1.DescendantNodes (
                                                                x => x.Kind ( )
                                                                     == SyntaxKind
                                                                        .CompilationUnit
                                                                     || x.Kind ( )
                                                                     == SyntaxKind.NamespaceDeclaration
                                                                        
                                                               ) )
            {
                if ( model == null )
                {
                    continue ;
                }

                var s1 = model.GetDeclaredSymbol ( descendantNode ) ;
                return1.Add ( new Example1Out { Symbol = s1 } ) ;
            }


            return return1 ;
        }


        /// <summary>
        /// 
        /// </summary>
        public BufferBlock < RejectedItem > RejectBlock { get ; }

        #region Implementation of IHaveRejectBlock
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ISourceBlock < RejectedItem > GetRejectBlock ( ) { return RejectBlock ; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Func < Document , Task < IEnumerable < Example1Out > > >
            GetAsyncTransformFunction ( )
        {
            return _transformFunc ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class Example1Out
    {
        private ISymbol _symbol ;
        /// <summary>
        /// 
        /// </summary>
        public ISymbol Symbol { get { return _symbol ; } set { _symbol = value ; } }
    }
}