using System ;
using System.Collections ;
using System.Globalization ;
using System.Windows.Data ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using NLog ;

namespace AnalysisControls.Converters
{
    /// <summary>
    /// 
    /// </summary>
    [ ValueConversion ( typeof ( CSharpSyntaxNode ) , typeof ( string ) ) ]
    [ ValueConversion ( typeof ( CSharpSyntaxNode ) , typeof ( IEnumerable ) ) ]
    public sealed class HierarchyConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly HierarchyConverter Default = new HierarchyConverter ( ) ;

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public HierarchyConverter()
        {
        }
        #region Implementation of IValueConverter
        /// <summary>Converts the specified value.</summary>
        /// <param name="value">The value.</param>
        /// <param name="targetType">Type of the target.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">targetType</exception>
        /// <exception cref="InvalidOperationException"></exception>
        public object Convert (
            object           value
          , [ NotNull ] Type targetType
          , object           parameter
          , CultureInfo      culture
        )
        {
            if ( value == null )
            {
                if ( targetType == typeof ( string ) )
                {
                    return "<null>" ;
                }

                return Array.Empty < object > ( ) ;
            }

            if ( targetType == null )
            {
                throw new ArgumentNullException ( nameof ( targetType ) ) ;
            }

            Logger.Debug ( "{type} {type2}" , value.GetType ( ).FullName , targetType.FullName ) ;
            var cs = ( CSharpSyntaxNode ) value ;
            {
                if ( targetType == typeof ( string ) )
                {
                    switch ( cs )
                    {
                        case FieldDeclarationSyntax fieldDeclarationSyntax :
                            return string.Join (
                                                ", "
                                              , fieldDeclarationSyntax.Declaration.Variables
                                               ) ;
                        case GenericNameSyntax genericNameSyntax :
                            return genericNameSyntax.Identifier.ValueText ;
                        case LiteralExpressionSyntax literalExpressionSyntax :
                            return literalExpressionSyntax.Token.ValueText ;
                        case MethodDeclarationSyntax methodDeclarationSyntax :
                            return methodDeclarationSyntax.Identifier.ValueText ;
                        case AccessorDeclarationSyntax accessorDeclarationSyntax :
                            return accessorDeclarationSyntax.Keyword ;

                        case IdentifierNameSyntax identifierNameSyntax :
                            return identifierNameSyntax.Identifier.ValueText ;

                        case QualifiedNameSyntax qualifiedNameSyntax :
                            return qualifiedNameSyntax.ToString ( ) ;

                        case VariableDeclarationSyntax variableDeclarationSyntax :
                            return variableDeclarationSyntax.Variables.Count == 1
                                       ? variableDeclarationSyntax
                                        .Variables[ 0 ]
                                        .Identifier.ValueText
                                       : variableDeclarationSyntax.Type.ToString ( ) ;
                    }

                    return value.GetType ( ).Name + " (" + cs.Kind ( ) + ")" ;
                }

                LogManager.GetCurrentClassLogger ( ).Debug ( targetType.FullName ) ;
                if ( targetType == typeof ( IEnumerable ) )
                {
                    switch ( cs )
                    {
                        case AccessorDeclarationSyntax accessorDeclarationSyntax :
                            return accessorDeclarationSyntax.Body != null
                                       ?
                                       ( IEnumerable ) accessorDeclarationSyntax.Body.Statements
                                       : accessorDeclarationSyntax.ExpressionBody != null
                                           ? new[] { accessorDeclarationSyntax.ExpressionBody }
                                           : Array.Empty < SyntaxNode > ( ) ;
                    }

                    return cs.ChildNodes ( ) ;
                }

                Logger.Debug ( "returning null for {s} {t}" , cs , targetType.FullName ) ;
                return null ;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            return null ;
        }
        #endregion
    }
}