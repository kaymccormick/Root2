using System;
using System.Linq;
using System.Text.Json;
using AnalysisAppLib;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace AnalysisControls
{
    internal sealed class SyntaxWalker2 : CSharpSyntaxWalker
    {
        private readonly SemanticModel _model ;

        #region Overrides of CSharpSyntaxVisitor
        internal SyntaxWalker2 (
            SemanticModel     model
            , SyntaxWalkerDepth depth = SyntaxWalkerDepth.Node
        ) : base ( depth )
        {
            _model = model ;
        }

        public override void VisitNamespaceDeclaration ( NamespaceDeclarationSyntax node )
        {
            // ReSharper disable once UnusedVariable
            var nsSymbol = _model.GetDeclaredSymbol ( node )
                           ?? throw new ArgumentNullException ( ) ;
            base.VisitNamespaceDeclaration ( node ) ;
        }

        public override void VisitClassDeclaration ( ClassDeclarationSyntax node )
        {
            var classSymbol = _model.GetDeclaredSymbol ( node ) ;
            if ( classSymbol == null )
            {
                throw new InvalidOperationException ( "No class symbol" ) ;
            }

            foreach ( var member in classSymbol.GetMembers ( ) )
            {
                switch ( member )
                {
                    case IMethodSymbol methodSymbol :
                        var m = CreateMethodInfo ( methodSymbol ) ;
                        var json = JsonSerializer.Serialize ( m ) ;
                        DebugUtils.WriteLine ( json ) ;
                        break ;
                }
            }

            base.VisitClassDeclaration ( node ) ;
        }

        [ NotNull ]
        private static MethodInfo CreateMethodInfo ( [ NotNull ] IMethodSymbol methodSymbol )
        {
            var m = new MethodInfo (
                methodSymbol.Name
                , methodSymbol.Parameters.Select (
                    p => new ParameterInfo (
                        p.Name
                        , p.Type
                        , p
                            .CustomModifiers
                            .Select (
                                md
                                    => new
                                        CustomModifierInfo (
                                            md
                                                .IsOptional
                                            , md
                                                .Modifier
                                                .ToDisplayString ( )
                                        )
                            )
                        , p
                            .Type
                            .ToDisplayString ( )
                    )
                )
            ) ;
            return m ;
        }

        public override void VisitStructDeclaration ( StructDeclarationSyntax node )
        {
            base.VisitStructDeclaration ( node ) ;
        }

        public override void VisitInterfaceDeclaration ( InterfaceDeclarationSyntax node )
        {
            base.VisitInterfaceDeclaration ( node ) ;
        }

        public override void VisitEnumDeclaration ( EnumDeclarationSyntax node )
        {
            base.VisitEnumDeclaration ( node ) ;
        }

        public override void VisitDelegateDeclaration ( DelegateDeclarationSyntax node )
        {
            base.VisitDelegateDeclaration ( node ) ;
        }

        public override void VisitFieldDeclaration ( FieldDeclarationSyntax node )
        {
            base.VisitFieldDeclaration ( node ) ;
        }

        public override void VisitEventFieldDeclaration ( EventFieldDeclarationSyntax node )
        {
            base.VisitEventFieldDeclaration ( node ) ;
        }

        public override void VisitExplicitInterfaceSpecifier (
            ExplicitInterfaceSpecifierSyntax node
        )
        {
            base.VisitExplicitInterfaceSpecifier ( node ) ;
        }

        public override void VisitMethodDeclaration ( MethodDeclarationSyntax node )
        {
            var symbol = _model.GetDeclaredSymbol ( node ) ;
            if ( symbol               != null
                 && symbol.MethodKind != MethodKind.Ordinary
                 && symbol.MethodKind != MethodKind.ExplicitInterfaceImplementation )
            {
                throw new InvalidOperationException ( symbol.MethodKind.ToString ( ) ) ;
            }

            // ReSharper disable once PossibleNullReferenceException
            var rt = symbol.ReturnType ;
            // ReSharper disable once UnusedVariable
            var origDef = rt.OriginalDefinition ;
            // ReSharper disable once UnusedVariable
            var displayString = rt.ToDisplayString ( ) ;
            base.VisitMethodDeclaration ( node ) ;
        }

        public override void VisitParameterList ( ParameterListSyntax node )
        {
            base.VisitParameterList ( node ) ;
        }

        public override void VisitParameter ( ParameterSyntax node )
        {
            var symbol = _model.GetDeclaredSymbol ( node ) ;
            if ( symbol != null )
            {
                foreach ( var symbolDisplayPart in symbol.ToDisplayParts ( ) )
                {
                    // ReSharper disable once UnusedVariable
                    var k = ( int ) symbolDisplayPart.Kind ;
                    var s = symbolDisplayPart.Symbol ;
                    var interfaces = "" ;
                    if ( s != null )
                    {
                        interfaces = string.Join (
                            ", "
                            , s.GetType ( )
                                .GetInterfaces ( )
                                .Select ( i => i.FullName )
                        ) ;
                    }

                    DebugUtils.WriteLine (
                        $"{symbolDisplayPart} {s?.Kind} {s?.GetType ( ).FullName} {interfaces}"
                    ) ;
                }
            }

            base.VisitParameter ( node ) ;
        }

        public override void VisitOperatorDeclaration ( OperatorDeclarationSyntax node )
        {
            base.VisitOperatorDeclaration ( node ) ;
        }

        public override void VisitConversionOperatorDeclaration (
            ConversionOperatorDeclarationSyntax node
        )
        {
            base.VisitConversionOperatorDeclaration ( node ) ;
        }

        public override void VisitConstructorDeclaration ( ConstructorDeclarationSyntax node )
        {
            base.VisitConstructorDeclaration ( node ) ;
        }

        public override void VisitDestructorDeclaration ( DestructorDeclarationSyntax node )
        {
            base.VisitDestructorDeclaration ( node ) ;
        }

        public override void VisitPropertyDeclaration ( PropertyDeclarationSyntax node )
        {
            base.VisitPropertyDeclaration ( node ) ;
        }

        public override void VisitEventDeclaration ( EventDeclarationSyntax node )
        {
            base.VisitEventDeclaration ( node ) ;
        }

        public override void VisitIndexerDeclaration ( IndexerDeclarationSyntax node )
        {
            base.VisitIndexerDeclaration ( node ) ;
        }
        #endregion
    }
}