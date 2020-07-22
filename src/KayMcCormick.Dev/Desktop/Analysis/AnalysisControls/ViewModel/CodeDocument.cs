using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.CodeAnalysis;
using RoslynCodeControls;

namespace AnalysisControls.ViewModel
{
    public class CodeDocument : DocModel
    {
        private readonly IFontSettingsSource _fs;
        private bool _isActive;

        /// <inheritdoc />
        public CodeDocument(IFontSettingsSource fs)
        {
            _fs = fs;
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
            // c.Filename = filename;
            CodeControl = c;

        }

        private void CreateCodeControl(Document docDocument)
        {
            var c = DoCodeDiagnostics ? CreateCodeDiagnostics() : CreateFormattedTextControl();

            c.SetValue(RoslynCodeControls.RoslynProperties.DocumentProperty, docDocument);
            CodeControl = c;
        }

        private void CreateCodeControl(string sourceCode = "", SyntaxTree syntaxTree=null , Compilation compilation=null, SemanticModel model=null)
        {
            var c = DoCodeDiagnostics ? CreateCodeDiagnostics() : CreateFormattedTextControl();

            if (sourceCode != null) c.SetValue(RoslynCodeControls.RoslynProperties.SourceTextProperty, sourceCode);
                
            if (syntaxTree != null) c.SetValue(RoslynCodeControls.RoslynProperties.SyntaxTreeProperty, syntaxTree);
            if (compilation != null) c.SetValue(RoslynCodeControls.RoslynProperties.CompilationProperty, compilation);
            if (model != null) c.SetValue(RoslynCodeControls.RoslynProperties.SemanticModelProperty, model);
            CodeControl = c;
        }

        public bool DoCodeDiagnostics { get; set; } = true;

        private Control CreateCodeDiagnostics()
        {
            return new CodeDiagnostics(){FontSource=_fs};
        }

        private static Control CreateFormattedTextControl()
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

        public Control CodeControl { get; set; }

        public SyntaxTree SyntaxTree { get; set; }
        public Compilation Compilation { get; set; }
        public SemanticModel Model { get; set; }
    }
}