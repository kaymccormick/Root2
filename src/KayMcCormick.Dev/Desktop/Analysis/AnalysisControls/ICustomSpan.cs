using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICustomSpan
    {
        /// <summary>
        /// 
        /// </summary>
        TextSpan Span {
            get;
        }
    }
}