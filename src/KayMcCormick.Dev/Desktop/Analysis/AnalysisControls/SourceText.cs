using System;
using System.IO;
using System.Text;
using System.Windows.Markup;
using Microsoft.CodeAnalysis.Text;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SourceTextExtension : MarkupExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string FileSource { get; set; }

        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Text != null) return SourceText.From(Text);
            
            using (var reader = new FileStream(FileSource, FileMode.Open))
            {
                return SourceText.From(reader, Encoding.UTF8, SourceHashAlgorithm.Sha256, true, false);
            }

        }
    }
}