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
using System.Windows.Controls ;
using AnalysisFramework ;


namespace ProjLib
{
    public class TransformScope
    {
        public string SourceCode { get ; }
        public FormattedCode FormattedCodeControl { get ; }
        public Visitor2 Visitor2 { get ; }
        public TransformScope ( string sourceCode , FormattedCode formattedCodeControl , Visitor2 visitor2 )
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
    public class TransformArgs
    {
        public delegate TransformArgs TransformArgsFactory (
            CodeAnalyseContext context
          , Visitor2           visitor2
        ) ;

        public TransformArgs ( FormattedCode formattedCode , Visitor2 visitor2 )
        {
            FormattedCode = formattedCode ;
            Visitor2      = visitor2 ;
        }

        public FormattedCode FormattedCode { get ; set ; }

        public Visitor2 Visitor2 { get ; }

        public CodeAnalyseContext Value { get ; }

        public StackPanel RootPanel { get ; set ; }

        protected TransformArgs (
            StackPanel         rootPanel
          , CodeAnalyseContext value
          , FormattedCode      formattedCode
          , Visitor2           visitor2
        )
        {
            RootPanel     = rootPanel ;
            Value         = value ;
            FormattedCode = formattedCode ;
            Visitor2      = visitor2 ;
        }

        public void Deconstruct (
            out StackPanel         rootPanel
          , out CodeAnalyseContext context
          , out FormattedCode      formattedCode
          , out Visitor2           visitor2
        )
        {
            rootPanel     = this.RootPanel ;
            context       = this.Value ;
            formattedCode = this.FormattedCode ;
            visitor2      = Visitor2 ;
        }
    }
}
