using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;

namespace ProjInterface
{
    public class Triple
    {
        public SyntaxTree Tree { get ; }

        public SemanticModel Model { get ; }

        public CompilationUnitSyntax Root { get ; }

        public void Deconstruct(out SyntaxTree tree, out SemanticModel model, out CompilationUnitSyntax root)
        {
            tree  = Tree ;
            model = Model ;
            root  = Root ;
        }

        public Triple ( SyntaxTree tree , SemanticModel model , CompilationUnitSyntax root )
        {
            Tree  = tree ;
            Model = model ;
            Root  = root ;
        }
    }
}