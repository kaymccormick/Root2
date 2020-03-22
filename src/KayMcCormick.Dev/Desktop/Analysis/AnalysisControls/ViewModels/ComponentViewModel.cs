#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// ComponentViewModel.cs
// 
// 2020-03-11-7:10 PM
// 
// ---
#endregion
using System.Runtime.Serialization ;
using AnalysisControls.Interfaces ;
using AnalysisFramework ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls.ViewModels
{
    public class ComponentViewModel : IComponentViewModel
    {
        public SyntaxTree Tree { get ; }

        public ICompilationUnitRootContext RootContext { get ; }

        public ComponentViewModel ( ICompilationUnitRootContext rootContext , [ CanBeNull ] SyntaxTree syntaxTree = null)
        {
            this.RootContext = rootContext ;
            this.Tree        = syntaxTree ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}