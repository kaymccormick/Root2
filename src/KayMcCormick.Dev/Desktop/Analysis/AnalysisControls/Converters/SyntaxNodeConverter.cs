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
using System.Collections.Generic;
using System.Globalization ;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data ;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.CodeAnalysis.VisualBasic;
using NLog ;

namespace AnalysisControls.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxNodeConverter : IValueConverter
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
                            if (s is CSharpSyntaxNode csn)
                            {
                                return csn.Kind();
                            } else if (s is VisualBasicSyntaxNode vbn)
                                return vbn.Kind();
                            return null;
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
            if (value != null)
            {
                var convertBack = Convert(value, targetType, parameter, culture);
                if (targetType == typeof(string))
                {
                    return convertBack?.ToString() ?? "";
                }
                return convertBack;
            }
            return null ;
        }
        #endregion
    }

    public class OperationTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item == null)
                return null;
            IOperation op = item as IOperation;
            // foreach (var @interface in item.GetType().GetInterfaces())
            // {
            //     if(typeof(IOperation).IsAssignableFrom(@interface))
            //     {
            //         var ancestors = TypeAncestors(@interface).ToList();
            //         DebugUtils.WriteLine(String.Join(", ", ancestors));
            //     }
            // }
            // var enumerable = item.GetType().GetInterfaces().Where(x => typeof(IOperation).IsAssignableFrom(x))
            //     .Select(y => Tuple.Create(y, TypeAncestors(y).ToList())).OrderByDescending(types => types.Item2.Count());
            // foreach (var (item1, item2) in enumerable)
            // {
            //     var @join = item2 != null ? String.Join(", ", item2.Select(z=>z?.FullName)) : "";
            //
            //     DebugUtils.WriteLine($"{item1} - {@join}");
            // }
            // var firstOrDefault = enumerable
            //     .FirstOrDefault();
            // DebugUtils.WriteLine(firstOrDefault?.Item1.FullName);
            DebugUtils.WriteLine(op.Kind.ToString());
            var tryFindResource = ((FrameworkElement) container).TryFindResource(op.Kind);
            if(tryFindResource != null)
            {

            }
            DebugUtils.WriteLine(tryFindResource?.ToString());
            return tryFindResource as DataTemplate;

            return base.SelectTemplate(item, container);
        }

        private IEnumerable<Type> TypeAncestors(Type type)
        {
            return type == null ? Enumerable.Empty<Type>() : TypeAncestors(type.BaseType).Concat(Enumerable.Repeat(type, 1));
        }
    }
}