using System;
using System.IO;
using System.Text;
using System.Windows.Markup;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Text;
using Microsoft.Graph;

namespace AnalysisControls
{
    public class SourceTextExtension : MarkupExtension
    {
        public string Text { get; set; }

        public string FileSource { get; set; }

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