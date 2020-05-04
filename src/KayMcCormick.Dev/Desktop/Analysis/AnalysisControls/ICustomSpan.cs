using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    public interface ICustomSpan
    {
        TextSpan Span {
            get;
        }
    }
}