using System;
using System.Threading.Tasks;
using AnalysisControls;
using AnalysisControls.ViewModel;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControlsCore
{
    public class CSharpEditorControl : AppCommand
    {
        private IDocumentHost _host;
        /// <inheritdoc />
        public CSharpEditorControl(IDocumentHost host) : base("Code")
        {
            _host = host;
        }

        /// <inheritdoc />
        public override object Argument { get; set; }
            
        /// <inheritdoc />
        public override Task<IAppCommandResult> ExecuteAsync(object parameter)
        {
            var doc = new CodeDocument();
            doc.Title = "Code";
            doc.CodeControl = new FormattedTextControl3()

                ;
            _host.AddDocument(doc);

            return Task.FromResult(AppCommandResult.Success);
        }

        /// <inheritdoc />
        public override void OnFault(AggregateException exception)
        {
        }

        /// <inheritdoc />
        public override object LargeImageSourceKey { get; set; }

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}