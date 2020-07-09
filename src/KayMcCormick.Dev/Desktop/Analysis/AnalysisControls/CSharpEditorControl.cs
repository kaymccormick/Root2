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
        private readonly Func<CodeDocument> _fcd;

        /// <inheritdoc />
        public CSharpEditorControl(IDocumentHost host, IContentSelector contentSelector, Func<CodeDocument> fcd) : base("Code")
        {
            _host = host;
            _contentSelector = contentSelector;
            _fcd = fcd;
        }

        /// <inheritdoc />
        public override object Argument { get; set; }
            
        /// <inheritdoc />
        public override Task<IAppCommandResult> ExecuteAsync(object parameter)
        {
            var doc = _fcd();
            doc.Title = "Code";
            // doc.CodeControl = new RoslynCodeControl()
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