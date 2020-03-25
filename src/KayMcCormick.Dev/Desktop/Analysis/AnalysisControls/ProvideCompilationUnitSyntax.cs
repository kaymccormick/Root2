using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisControls
{
    public class ProvideCompilationUnitSyntax : MarkupExtension
    {
        #region Overrides of MarkupExtension
        public override object ProvideValue ( IServiceProvider serviceProvider )
        {
            return SyntaxFactory.ParseCompilationUnit (ProjLib.LibResources.Program_Parse ) ;
        }
        #endregion
    }
}
