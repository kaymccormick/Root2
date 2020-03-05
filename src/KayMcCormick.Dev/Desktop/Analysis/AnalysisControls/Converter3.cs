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
using System.Collections ;
using System.Globalization ;
using System.Windows.Data ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.CSharp.Syntax ;
using Microsoft.CodeAnalysis.VisualBasic ;
using NLog ;

namespace AnalysisControls
{
    public class Converter3 : IValueConverter
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Implementation of IValueConverter
        public object Convert (
            [ NotNull ] object      value
          , Type        targetType
          , object      parameter
          , CultureInfo culture
        )
        {
            if ( value == null )
            {
                return null ;
                throw new ArgumentNullException ( nameof ( value ) ) ;
            }

            Logger.Debug (
                          "{type} {type2} {parameter}"
                        , value?.GetType ( ).FullName
                        , targetType.FullName, parameter
                         ) ;
            if ( value         == null
                 && targetType == typeof ( String ) )
            {
                return "(null)" ;
            }
            if ( value is SyntaxNode s )
            {
                if ( parameter is Converter1Param parm )
                {
                    switch(parm)
                    {
                        case Converter1Param.Ancestors : return s.Ancestors();
                        case Converter1Param.AncestorsAndSelf : return s.AncestorsAndSelf() ;
                        case Converter1Param.GetFirstToken : return s.GetFirstToken();
                        case Converter1Param.GetLocation : return s.GetLocation();
                        case Converter1Param.GetLastToken : return s.GetLastToken() ;
                        case Converter1Param.GetReference : return s.GetReference();
                        case Converter1Param.GetText : return s.GetText() ;
                        case Converter1Param.ToFullString : return s.ToFullString() ;
                        case Converter1Param.ToString : return s.ToString() ;
                        case Converter1Param.Kind : return ((CSharpSyntaxNode)s).Kind ( ) ;
                        case Converter1Param.ChildNodesAndTokens :return s.ChildNodesAndTokens() ;
                        case Converter1Param.ChildNodes : return s.ChildNodes();
                        case Converter1Param.ChildTokens : return s.ChildNodes() ;
                        case Converter1Param.DescendantNodes : return s.DescendantNodes();
                        case Converter1Param.DescendantNodesAndSelf : return s.DescendantNodesAndSelf() ;
                        case Converter1Param.DescendantNodesAndTokens : return s.DescendantNodesAndTokens() ;
                        case Converter1Param.DescendantNodesAndTokensAndSelf : return s.DescendantNodesAndTokensAndSelf();
                        case Converter1Param.DescendantTokens : return s.DescendantTokens (node => true, true
                                                                                           ) ;
                        case Converter1Param.Diagnostics : return s.GetDiagnostics() ;
                        case Converter1Param.DescendantTrivia :return s.DescendantTrivia();
                        case Converter1Param.GetLeadingTrivia : return s.GetLeadingTrivia() ;
                        default : throw new ArgumentOutOfRangeException ( ) ;
                    }
                }
                Logger.Debug ( "returning null for {s} {t}" , s , targetType.FullName ) ;
                return null ;
            }

            Logger.Debug ( "returning null for {s} {t}" , value , targetType.FullName ) ;
            return null ;
        }

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