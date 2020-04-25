#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ModelTests
// InClassName.cs
// 
// 2020-04-24-11:59 PM
// 
// ---
#endregion
using System.Threading.Tasks.Dataflow ;
using AnalysisAppLib.Dataflow ;
using Microsoft.CodeAnalysis ;

namespace ModelTests
{
    public class InClassName < TT >
    {
        private IDataflowTransformFuncProvider < Document , TT > _funcProvider ;
        private TransformManyBlock < Document , TT >             _tmanyBlock ;

        public InClassName (
            IDataflowTransformFuncProvider < Document , TT > dataflowTransformFuncProvider
          , TransformManyBlock < Document , TT >             tManyBlock
        )
        {
            FuncProvider = dataflowTransformFuncProvider ;
            TManyBlock   = tManyBlock ;
        }

        public IDataflowTransformFuncProvider < Document , TT > FuncProvider
        {
            get { return _funcProvider ; }
            set { _funcProvider = value ; }
        }


        public TransformManyBlock < Document , TT > TManyBlock { get ; private set ; }
    }
}