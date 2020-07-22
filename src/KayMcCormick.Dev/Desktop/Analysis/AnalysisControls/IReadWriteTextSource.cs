using System.Collections;
using Microsoft.CodeAnalysis.Text;
using RoslynCodeControls;

namespace AnalysisControls
{
    public interface IReadWriteTextSource : ICustomTextSource
    {
        void TextInput(int insertionPoint, InputRequest inputRequest);
    }
}