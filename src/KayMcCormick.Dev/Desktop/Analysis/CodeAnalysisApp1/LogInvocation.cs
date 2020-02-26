#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocation.cs
// 
// 2020-02-25-9:21 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using MessageTemplates ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Newtonsoft.Json ;
using ProjLib ;

namespace CodeAnalysisApp1
{
    public class CodeAnalyseContext
    {
        [JsonProperty(PropertyName = null)]
        protected SemanticModel _currentModel ;
        protected CompilationUnitSyntax _currentRoot ;
        protected StatementSyntax _statement ;
        protected SyntaxNode node;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public CodeAnalyseContext ( SemanticModel currentModel , CompilationUnitSyntax currentRoot , StatementSyntax statement, SyntaxNode node , ICodeSource document )
        {
            _currentModel = currentModel ;
            _currentRoot = currentRoot ;
            _statement = statement ;
            this.Node = node;
            Document = document ;
        }

        public StatementSyntax Statement { get => _statement ; set => _statement = value ; }
        public SemanticModel CurrentModel { get => _currentModel ; set => _currentModel = value ; }

        public CompilationUnitSyntax CurrentRoot
        {
            get => _currentRoot ;
            set => _currentRoot = value ;
        }

        public  SyntaxNode Node { get => node ; set => node = value ; }

        public ICodeSource Document { get ; }
    }

    public class LogInvocation : CodeAnalyseContext
    {
        private LogMessageRepr          _msgval ;
        private string          sourceLocation ;
        private IMethodSymbol   methodSymbol ;
        

        public LogInvocation (
            string                sourceLocation
          , IMethodSymbol         methodSymbol
          , LogMessageRepr        msgval
          , StatementSyntax       statement
          , SemanticModel         currentModel
          , CompilationUnitSyntax currentRoot
            , ICodeSource document
        ) : base(currentModel, currentRoot, statement, statement, document)
        {
            CurrentModel = currentModel ;
            CurrentRoot = currentRoot ;
            Statement = statement ;
            Msgval         = msgval ;
            SourceLocation = sourceLocation ;
            MethodSymbol   = methodSymbol ;

        }

        public string SourceLocation { get => sourceLocation ; set => sourceLocation = value ; }

        public IMethodSymbol MethodSymbol { get => methodSymbol ; set => methodSymbol = value ; }


        public LogMessageRepr Msgval { get => _msgval ; set => _msgval = value ; }

        public IList< LogInvocationArgument > Arguments { get ; set ; }

        public string SourceContext { get ; set ; }

        public string FollowingCode { get ; set ; }

        public string PrecedingCode { get ; set ; }

        public string Code { get ; set ; }
    }
    public class LogMessageRepr 
    {
        private readonly string _message ;

        public LogMessageRepr(string message) { _message = message ; }
        public LogMessageRepr ( ) {  }

        public MessageTemplate MessageTemplate { get ; set ; }

        public bool IsMessageTemplate { get ; set ; }

        public object MessageExprPojo { get ; set ; }

        public object PrimaryMessage => IsMessageTemplate ? MessageTemplate.Text : MessageExprPojo ;
    }
}