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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis ;

namespace AnalysisControls.Converters
{
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