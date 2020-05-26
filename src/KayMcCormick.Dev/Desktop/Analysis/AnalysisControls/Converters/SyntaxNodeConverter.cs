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
    public class OperationInfoConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IOperation operation = (IOperation) value;
            if (value == null)
            {
                return null;
            }

            switch (operation.Kind)
            {
                case OperationKind.None:
                    break;
                case OperationKind.Invalid:
                    break;
                case OperationKind.Block:
                    break;
                case OperationKind.VariableDeclarationGroup:
                    break;
                case OperationKind.Switch:
                    break;
                case OperationKind.Loop:
                    break;
                case OperationKind.Labeled:
                    break;
                case OperationKind.Branch:
                    break;
                case OperationKind.Empty:
                    break;
                case OperationKind.Return:
                    break;
                case OperationKind.YieldBreak:
                    break;
                case OperationKind.Lock:
                    break;
                case OperationKind.Try:
                    break;
                case OperationKind.Using:
                    break;
                case OperationKind.YieldReturn:
                    break;
                case OperationKind.ExpressionStatement:
                    break;
                case OperationKind.LocalFunction:
                    break;
                case OperationKind.Stop:
                    break;
                case OperationKind.End:
                    break;
                case OperationKind.RaiseEvent:
                    break;
                case OperationKind.Literal:
                    break;
                case OperationKind.Conversion:
                    break;
                case OperationKind.Invocation:
                    break;
                case OperationKind.ArrayElementReference:
                    break;
                case OperationKind.LocalReference:
                    break;
                case OperationKind.ParameterReference:
                    break;
                case OperationKind.FieldReference:
                    break;
                case OperationKind.MethodReference:
                    break;
                case OperationKind.PropertyReference:
                    break;
                case OperationKind.EventReference:
                    break;
                case OperationKind.Unary:
                    break;
                case OperationKind.Binary:
                    break;
                case OperationKind.Conditional:
                    break;
                case OperationKind.Coalesce:
                    break;
                case OperationKind.AnonymousFunction:
                    break;
                case OperationKind.ObjectCreation:
                    break;
                case OperationKind.TypeParameterObjectCreation:
                    break;
                case OperationKind.ArrayCreation:
                    break;
                case OperationKind.InstanceReference:
                    break;
                case OperationKind.IsType:
                    break;
                case OperationKind.Await:
                    break;
                case OperationKind.SimpleAssignment:
                    break;
                case OperationKind.CompoundAssignment:
                    break;
                case OperationKind.Parenthesized:
                    break;
                case OperationKind.EventAssignment:
                    break;
                case OperationKind.ConditionalAccess:
                    break;
                case OperationKind.ConditionalAccessInstance:
                    break;
                case OperationKind.InterpolatedString:
                    break;
                case OperationKind.AnonymousObjectCreation:
                    break;
                case OperationKind.ObjectOrCollectionInitializer:
                    break;
                case OperationKind.MemberInitializer:
                    break;
                case OperationKind.NameOf:
                    break;
                case OperationKind.Tuple:
                    break;
                case OperationKind.DynamicObjectCreation:
                    break;
                case OperationKind.DynamicMemberReference:
                    break;
                case OperationKind.DynamicInvocation:
                    break;
                case OperationKind.DynamicIndexerAccess:
                    break;
                case OperationKind.TranslatedQuery:
                    break;
                case OperationKind.DelegateCreation:
                    break;
                case OperationKind.DefaultValue:
                    break;
                case OperationKind.TypeOf:
                    break;
                case OperationKind.SizeOf:
                    break;
                case OperationKind.AddressOf:
                    break;
                case OperationKind.IsPattern:
                    break;
                case OperationKind.Increment:
                    break;
                case OperationKind.Throw:
                    break;
                case OperationKind.Decrement:
                    break;
                case OperationKind.DeconstructionAssignment:
                    break;
                case OperationKind.DeclarationExpression:
                    break;
                case OperationKind.OmittedArgument:
                    break;
                case OperationKind.FieldInitializer:
                    break;
                case OperationKind.VariableInitializer:
                    break;
                case OperationKind.PropertyInitializer:
                    break;
                case OperationKind.ParameterInitializer:
                    break;
                case OperationKind.ArrayInitializer:
                    break;
                case OperationKind.VariableDeclarator:
                    break;
                case OperationKind.VariableDeclaration:
                    break;
                case OperationKind.Argument:
                    break;
                case OperationKind.CatchClause:
                    break;
                case OperationKind.SwitchCase:
                    break;
                case OperationKind.CaseClause:
                    break;
                case OperationKind.InterpolatedStringText:
                    break;
                case OperationKind.Interpolation:
                    break;
                case OperationKind.ConstantPattern:
                    break;
                case OperationKind.DeclarationPattern:
                    break;
                case OperationKind.TupleBinary:
                    break;
                case OperationKind.MethodBody:
                    break;
                case OperationKind.ConstructorBody:
                    break;
                case OperationKind.Discard:
                    break;
                case OperationKind.FlowCapture:
                    break;
                case OperationKind.FlowCaptureReference:
                    break;
                case OperationKind.IsNull:
                    break;
                case OperationKind.CaughtException:
                    break;
                case OperationKind.StaticLocalInitializationSemaphore:
                    break;
                case OperationKind.FlowAnonymousFunction:
                    break;
                case OperationKind.CoalesceAssignment:
                    break;
                case OperationKind.Range:
                    break;
                case OperationKind.ReDim:
                    break;
                case OperationKind.ReDimClause:
                    break;
                case OperationKind.RecursivePattern:
                    break;
                case OperationKind.DiscardPattern:
                    break;
                case OperationKind.SwitchExpression:
                    break;
                case OperationKind.SwitchExpressionArm:
                    break;
                case OperationKind.PropertySubpattern:
                    break;
                case OperationKind.UsingDeclaration:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}