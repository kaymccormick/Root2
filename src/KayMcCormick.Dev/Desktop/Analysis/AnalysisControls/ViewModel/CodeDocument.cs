using System.Collections;
using System.Windows.Input;
using Microsoft.CodeAnalysis;
using RoslynCodeControls;

namespace AnalysisControls.ViewModel
{
    internal class CodeDocument : DocModel
    {
        private bool _isActive;

        /// <inheritdoc />
        public CodeDocument()
        {
            CreateCodeControl();
        }

        /// <inheritdoc />
        public CodeDocument(SyntaxTree syntaxTree, Compilation compilation)
        {
            SyntaxTree = syntaxTree;
            Compilation = compilation;
            var model = Compilation?
                .GetSemanticModel(SyntaxTree);
            CreateCodeControl(null, syntaxTree, compilation, model);
            
        }

        public CodeDocument(Document docDocument)
        {
            CreateCodeControl(docDocument);
        }

        public CodeDocument(string docDocument)
        {
            CreateCodeControlFromFile(docDocument);
        }

        private void CreateCodeControlFromFile(string filename)
        {
            var c = DoCodeDiagnostics ? CreateCodeDiagnostics() : CreateFormattedTextControl();
            c.Filename = filename;
            CodeControl = c;

        }

        private void CreateCodeControl(Document docDocument)
        {
            var c = DoCodeDiagnostics ? CreateCodeDiagnostics() : CreateFormattedTextControl();
            c.Document = docDocument;
            CodeControl = c;
        }

        private void CreateCodeControl(string sourceCode = "", SyntaxTree syntaxTree=null , Compilation compilation=null, SemanticModel model=null)
        {
            var c = DoCodeDiagnostics ? CreateCodeDiagnostics() : CreateFormattedTextControl();
            if (sourceCode != null) c.SourceText = sourceCode;
            if (syntaxTree != null) c.SyntaxTree = syntaxTree;
            if (compilation != null) c.Compilation = compilation;
            if (model != null) c.Model = model;
            CodeControl = c;
        }

        public bool DoCodeDiagnostics { get; set; } = true;

        private SyntaxNodeControl CreateCodeDiagnostics()
        {
            return new CodeDiagnostics();
        }

        private static RoslynCodeControl CreateFormattedTextControl()
        {
            var c = new RoslynCodeControl();
            return c;
        }

        /// <inheritdoc />
        public override IEnumerable ContextualTabGroupHeaders =>
            new[] {"Code Analysis"};

        /// <inheritdoc />
        public override bool IsActive
        {
            get { return _isActive; }
            set
            {
                if (value == _isActive) return;
                _isActive = value;
                if (_isActive)
                {
                    if (CodeControl != null) Keyboard.Focus(CodeControl);
                }
                OnPropertyChanged();
            }
        }

        /// <inheritdoc />
        public override object Content => CodeControl;

        public SyntaxNodeControl CodeControl { get; set; }

        public SyntaxTree SyntaxTree { get; set; }
        public Compilation Compilation { get; set; }
        public SemanticModel Model { get; set; }
    }
}