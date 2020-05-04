using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.VisualBasic.Symbols;

namespace AnalysisControls.Converters
{
    public class SymbolInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ISymbol symbol = (ISymbol) value;
            if (value == null)
            {
                return null;
            }

            if ((string) parameter == "kind")
            {

                return symbol.Kind.ToString();
            }

            switch (symbol)
            {
                case IAliasSymbol aliasSymbol:
                    break;
                case IArrayTypeSymbol arrayTypeSymbol:
                    break;
                case ISourceAssemblySymbol sourceAssemblySymbol:
                    break;
                case IAssemblySymbol assemblySymbol:
                    break;
                case IDiscardSymbol discardSymbol:
                    break;
                case IDynamicTypeSymbol dynamicTypeSymbol:
                    break;
                case IErrorTypeSymbol errorTypeSymbol:
                    break;
                case IEventSymbol eventSymbol:
                    break;
                case IFieldSymbol fieldSymbol:
                    break;
                case ILabelSymbol labelSymbol:
                    break;
                case ILocalSymbol localSymbol:
                    break;
                case IMethodSymbol methodSymbol:
                    break;
                case IModuleSymbol moduleSymbol:
                    break;
                case INamedTypeSymbol namedTypeSymbol:
                    
                    break;
                case INamespaceSymbol namespaceSymbol:
                    break;
                case IPointerTypeSymbol pointerTypeSymbol:
                    break;
                case ITypeParameterSymbol typeParameterSymbol:
                    break;
                case ITypeSymbol typeSymbol:
                    break;
                case INamespaceOrTypeSymbol namespaceOrTypeSymbol:
                    break;
                case IParameterSymbol parameterSymbol:
                    break;
                case IPreprocessingSymbol preprocessingSymbol:
                    break;
                case IPropertySymbol propertySymbol:
                    break;
                case IRangeVariableSymbol rangeVariableSymbol:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(symbol));
            }

            return symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
            
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}