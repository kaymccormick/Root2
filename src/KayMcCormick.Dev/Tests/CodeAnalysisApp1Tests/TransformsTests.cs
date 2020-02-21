using System ;
using System.Collections.Immutable ;
using System.Linq ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.VisualBasic.Syntax ;
using NLog ;
using static CodeAnalysisApp1.Transforms;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using Microsoft.CodeAnalysis.MSBuild ;
using KayMcCormick.Dev.Logging;
using KayMcCormick.Dev.TestLib ;
using KayMcCormick.Dev.TestLib.Fixtures ;
using Xunit ;
using Xunit.Abstractions ;
// ReSharper disable AssignNullToNotNullAttribute


namespace CodeAnalysisApp1Tests
{
    [Collection("General Purpose")]
    [BeforeAfterLogger]
    [LogTestMethod()]
    public class TransformsTests : IClassFixture <LoggingFixture>, IDisposable
    {
        private readonly LoggingFixture _logging ;
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public TransformsTests (LoggingFixture logging, ITestOutputHelper output )
        {
            _logging = logging ;
            _logging.SetOutputHelper ( output ) ;
        }

        [Fact()]
        public void TransformExprTest_Null()
        {
            Assert.Throws < ArgumentNullException > ( ( ) => TransformExpr ( null ) ) ;
        }

        [Fact()]
        public void TransformExprTest_InvocationExpressionSyntax()
        {
            var expr = InvocationExpression (
                                             MemberAccessExpression (
                                                                     SyntaxKind
                                                                        .SimpleMemberAccessExpression
                                                                   , IdentifierName ( "instance" )
                                                                   , IdentifierName ( "methodName" )
                                                                    )
                                            ) ;
            Assert.NotNull(expr);
            Logger.Debug ( "{expr}" , expr ) ;
            var result = TransformExpr ( expr ) ;
            Assert.NotNull ( result ) ;
            Logger.Debug ( "{result}" , result ) ;

        }

        [Fact()]
        public void TransformOperatorTokenTest()
        {
            var @out = TransformOperatorToken ( Token ( SyntaxKind.DotToken ) ) ;
            Logger.Debug ( "{out}" , @out ) ;
            Assert.NotNull ( @out ) ;
        }

        [ Fact ( ) ]
        public void TransformParameterTest ( )
        {

            // var parameterSyntax = Parameter ( Literal ( "poop" ) ) ;
            // var tree = SyntaxTree ( parameterSyntax ) ;
            // var test = new TestWorkpace ( new AdhocWorkspace ( ) ) ;
        }



        public void TransformParameterTest2()
        {
            var tree = CSharpSyntaxTree.ParseText (
                                        @"using System;
using System.Collections.Generic;
using System.Linq;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(""Hello World!"");
        }
    }
}"
                                       ) ;


            var x = ClassDeclaration ( "foo" ).WithTrailingTrivia(Space)
                                              .WithMembers (
                                                            new SyntaxList < MemberDeclarationSyntax > (
                                                                                                        MethodDeclaration (
                                                                                                                           IdentifierName (
                                                                                                                                           "test"
                                                                                                                                          )
                                                                                                                         , "bar"
                                                                                                                          )
                                                                                                       )
                                                           ) ;
            Logger.Info ( "{code}" , x.ToFullString ( ) ) ;
        }

        [Fact()]
        public void TransformIdentifierTest()
        {
            
        }

        [Fact()]
        public void TransformTypeSyntaxTest()
        {
            
        }

        [Fact()]
        public void TransformNameSyntaxTest()
        {
            
        }

        [Fact()]
        public void TransformSimpleNameSyntaxTest()
        {
            
        }

        [Fact()]
        public void TransformIdentifierNameSyntaxTest()
        {
            
        }

        [Fact()]
        public void TransformGenericNameSyntaxTest()
        {
            
        }

        [Fact()]
        public void TransformGenericNameTypeArgumentTest()
        {
            
        }

        [Fact()]
        public void TransformStatementTest()
        {
            
        }

        [Fact()]
        public void TransformKeywordTest()
        {
            
        }

        [Fact()]
        public void TransformInterpolatedTest()
        {
            
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose ( ) { _logging.SetOutputHelper ( null ) ; }
    }
}