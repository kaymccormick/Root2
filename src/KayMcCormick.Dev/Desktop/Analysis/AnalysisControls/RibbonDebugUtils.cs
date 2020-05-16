using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using KayMcCormick.Dev;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Graph;
using NLog;
using NLog.Fluent;

namespace AnalysisControls
{
    public class RibbonDebugUtils
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public static void OnPropertyChanged(string LoggingIdentifier, DependencyObject o,
            DependencyPropertyChangedEventArgs e)
        {
            new LogBuilder(Logger).LoggerName(LoggingIdentifier + ".OnPropertyChanged." + e.Property.Name)
                .Level(LogLevel.Info)
                .Message($"{LoggingIdentifier} {e.Property.Name} OldValue = {e.OldValue}; NewValue = {e.NewValue}")
                .Write();
        }

        public static void DumpPropertySource(DependencyObject @do, DependencyProperty prop)
        {
            var s = DependencyPropertyHelper.GetValueSource(@do, prop);
            var value = @do.GetValue(prop);
            var lv = @do.ReadLocalValue(prop);
            DebugUtils.WriteLine(
                $"Property on {@do}, name {prop.Name} (Value {value} [{lv}]): {s.BaseValueSource}; {s.IsCoerced}; {s.IsCurrent}; {s.IsExpression};");
            if (s.IsExpression)
            {
                if (@do is FrameworkElement fe)
                {
                    DebugUtils.WriteLine($"datacontext = {fe.DataContext}");
                }
                
                var binding = BindingOperations.GetBinding(@do, prop);
                DumpBinding(@do, prop, binding);
            }
        }

        private static void DumpBinding(object target, DependencyProperty prop, Binding binding)
        {
            foreach (var propertyInfo in binding.GetType().GetProperties())
            {
                var value = propertyInfo.GetValue(binding);
                // string convval;
                // if (value != null)
                // {
                    // var conv = TypeDescriptor.GetConverter(value.GetType());
                    // convval = (bool)conv?.CanConvertTo(typeof(string))
                        // ? conv.ConvertTo(value, typeof(string))?.ToString() ?? "null (converted)"
                        // : value.ToString();
                // }
                // else
                // {
                    // convval = "<null>" + propertyInfo.PropertyType.FullName;
                // }
                // var sb = new StringBuilder();
                // HashSet<object> seen = new HashSet<object>();
                // UiElementTypeConverter.DoConvertToString(value, sb, false, seen);
                string sb;
                if (value is PropertyPath pp)
                {
                    sb = $"{pp.Path}, {string.Join(";", pp.PathParameters)}";
                }
                else
                {
                    sb = value?.ToString() ?? "null";
                }
                DebugUtils.WriteLine($"{target} {prop.Name} {propertyInfo.Name} {sb}");
            }
        }
    }
}