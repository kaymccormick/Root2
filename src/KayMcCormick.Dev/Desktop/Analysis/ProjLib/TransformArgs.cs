#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// TransformArgs.cs
// 
// 2020-02-27-4:45 AM
// 
// ---
#endregion
using ProjLib.Interfaces ;

namespace ProjLib
{
    public class TransformScope
    {
        public string SourceCode { get ; }
        public IFormattedCode FormattedCodeControl { get ; }
        public Visitor2 Visitor2 { get ; }
        public TransformScope ( string sourceCode , IFormattedCode formattedCodeControl , Visitor2 visitor2 )
        {
            SourceCode = sourceCode ;
            FormattedCodeControl = formattedCodeControl ;
            Visitor2 = visitor2 ;
        }

        public override string ToString ( )
        {
            return $"{nameof ( SourceCode )}: {SourceCode}, {nameof ( FormattedCodeControl )}: {FormattedCodeControl}, {nameof ( Visitor2 )}: {Visitor2}" ;
        }
    }
}
