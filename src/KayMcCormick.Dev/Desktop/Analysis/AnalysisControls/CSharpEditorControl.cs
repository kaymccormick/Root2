using System;
using System.Threading.Tasks;
using AnalysisControls;
using AnalysisControls.ViewModel;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControlsCore
{
    internal class CSharpEditorControl : AppCommand
    {
        private IDocumentHost _host;
        private readonly IContentSelector _contentSelector;

        /// <inheritdoc />
        public CSharpEditorControl(IDocumentHost host, IContentSelector contentSelector) : base("Code")
        {
            _host = host;
            _contentSelector = contentSelector;
        }

        /// <inheritdoc />
        public override object Argument { get; set; }
            
        /// <inheritdoc />
        public override Task<IAppCommandResult> ExecuteAsync(object parameter)
        {
            var doc = new CodeDocument();
            doc.Title = "Code";
            // doc.CodeControl = new FormattedTextControl3()
            _host.AddDocument(doc);
            _contentSelector.SetActiveContent(doc);
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