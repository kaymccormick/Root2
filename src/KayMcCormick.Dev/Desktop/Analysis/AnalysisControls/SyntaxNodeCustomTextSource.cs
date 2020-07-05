using System.Threading;
using JetBrains.Annotations;
using RoslynCodeControls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxNodeCustomTextSource : CustomTextSource4
    {
        public SyntaxNodeCustomTextSource(double pixelsPerDip, FontRendering fontRendering, GenericTextRunProperties genericTextRunProperties, [NotNull] SynchronizationContext synchContext) : base(pixelsPerDip, fontRendering, genericTextRunProperties, synchContext)
        {
        }
    }
}