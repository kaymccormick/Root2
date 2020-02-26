﻿#region header
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
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using MessageTemplates ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Control = System.Windows.Forms.Control ;

namespace ProjLib
{
    public class LogInvocationBase
    {
        protected SemanticModel _currentModel ;
        protected CompilationUnitSyntax _currentRoot ;
        protected StatementSyntax _statement ;
        protected SyntaxNode node;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public LogInvocationBase ( SemanticModel currentModel , CompilationUnitSyntax currentRoot , StatementSyntax statement, SyntaxNode node )
        {
            _currentModel = currentModel ;
            _currentRoot = currentRoot ;
            _statement = statement ;
            this.Node = node;
        }

        public StatementSyntax Statement { get => _statement ; set => _statement = value ; }
        public SemanticModel CurrentModel { get => _currentModel ; set => _currentModel = value ; }

        public CompilationUnitSyntax CurrentRoot
        {
            get => _currentRoot ;
            set => _currentRoot = value ;
        }

        public  SyntaxNode Node { get => node ; set => node = value ; }
    }

    public class LogInvocation : LogInvocationBase
    {
        private LogMessageRepr          _msgval ;
        private string          sourceLocation ;
        private IMethodSymbol   methodSymbol ;
        private TextBlock  _formattedCode ;

        public LogInvocation (
            string                sourceLocation
          , IMethodSymbol         methodSymbol
          , LogMessageRepr        msgval
          , StatementSyntax       statement
          , SemanticModel         currentModel
          , CompilationUnitSyntax currentRoot
        ) : base(currentModel, currentRoot, statement, statement)
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