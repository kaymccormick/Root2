#region header
// Kay McCormick (mccor)
// 
// ManagedProd
// AnalysisControls
// Converter3.cs
// 
// 2020-03-03-12:44 PM
// 
// ---
#endregion
using System ;
using System.Globalization ;
using System.Windows.Data ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using NLog ;

namespace AnalysisControls.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class SyntaxNodeConverter : IValueConverter
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Implementation of IValueConverter
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public object Convert (
            object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            if ( value == null )
            {
                return null ;
            }

            Logger.Debug (
                          "{type} {type2} {parameter}"
                        , value.GetType ( ).FullName
                        , targetType.FullName
                        , parameter
                         ) ;
            if ( value is SyntaxNode s )
            {
                if ( parameter is SyntaxNodeInfo parm )
                {
                    switch ( parm )
                    {
                        case SyntaxNodeInfo.Ancestors :        return s.Ancestors ( ) ;
                        case SyntaxNodeInfo.AncestorsAndSelf : return s.AncestorsAndSelf ( ) ;
                        case SyntaxNodeInfo.GetFirstToken :    return s.GetFirstToken ( ) ;
                        case SyntaxNodeInfo.GetLocation :      return s.GetLocation ( ) ;
                        case SyntaxNodeInfo.GetLastToken :     return s.GetLastToken ( ) ;
                        case SyntaxNodeInfo.GetReference :     return s.GetReference ( ) ;
                        case SyntaxNodeInfo.GetText :          return s.GetText ( ) ;
                        case SyntaxNodeInfo.ToFullString :     return s.ToFullString ( ) ;
                        case SyntaxNodeInfo.ToString :         return s.ToString ( ) ;
                        case SyntaxNodeInfo.Kind :
                            return ( ( CSharpSyntaxNode ) s ).Kind ( ) ;
                        case SyntaxNodeInfo.ChildNodesAndTokens : return s.ChildNodesAndTokens ( ) ;
                        case SyntaxNodeInfo.ChildNodes :          return s.ChildNodes ( ) ;
                        case SyntaxNodeInfo.ChildTokens :         return s.ChildTokens ( ) ;
                        case SyntaxNodeInfo.DescendantNodes :     return s.DescendantNodes ( ) ;
                        case SyntaxNodeInfo.DescendantNodesAndSelf :
                            return s.DescendantNodesAndSelf ( ) ;
                        case SyntaxNodeInfo.DescendantNodesAndTokens :
                            return s.DescendantNodesAndTokens ( ) ;
                        case SyntaxNodeInfo.DescendantNodesAndTokensAndSelf :
                            return s.DescendantNodesAndTokensAndSelf ( ) ;
                        case SyntaxNodeInfo.DescendantTokens :
                            return s.DescendantTokens ( node => true , true ) ;
                        case SyntaxNodeInfo.Diagnostics :      return s.GetDiagnostics ( ) ;
                        case SyntaxNodeInfo.DescendantTrivia : return s.DescendantTrivia ( ) ;
                        case SyntaxNodeInfo.GetLeadingTrivia : return s.GetLeadingTrivia ( ) ;
                        default :
                            throw new ArgumentOutOfRangeException ( ) ;
                    }
                }

                Logger.Debug ( "returning null for {s} {t}" , s , targetType.FullName ) ;
                return null ;
            }

            Logger.Debug ( "returning null for {s} {t}" , value , targetType.FullName ) ;
            return null ;
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