using Microsoft.CodeAnalysis ;

namespace AnalysisAppLib
{
    public interface ISyntaxTreeContext
    {
        SyntaxTree SyntaxTree { get ; }
    }
}