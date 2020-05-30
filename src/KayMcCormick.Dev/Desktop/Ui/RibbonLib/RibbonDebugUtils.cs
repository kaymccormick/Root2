using System.Diagnostics;
using System.Windows;
using System.Windows.Data;

namespace RibbonLib
{
    /// <summary>
    /// 
    /// </summary>
    public class RibbonDebugUtils
    {
        public static void OnPropertyChanged(object modelItem, string propertyName)
        {
            WriteLine($"{modelItem}.{nameof(OnPropertyChanged)}{propertyName}");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="LoggingIdentifier"></param>
        /// <param name="o"></param>
        /// <param name="e"></param>
        public static void OnPropertyChanged(string LoggingIdentifier, DependencyObject o,
            DependencyPropertyChangedEventArgs e)
        {
            WriteLine($"{LoggingIdentifier} {e.Property.Name} OldValue = {e.OldValue}; NewValue = {e.NewValue}");
            // new LogBuilder(Logger).LoggerName(LoggingIdentifier + ".OnPropertyChanged." + e.Property.Name)
                // .Level(LogLevel.Info)
                // .Message($"{LoggingIdentifier} {e.Property.Name} OldValue = {e.OldValue}; NewValue = {e.NewValue}")
                // .Write();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="do"></param>
        /// <param name="prop"></param>
        public static void DumpPropertySource(DependencyObject @do, DependencyProperty prop)
        {
            var s = DependencyPropertyHelper.GetValueSource(@do, prop);
            var value = @do.GetValue(prop);
            var lv = @do.ReadLocalValue(prop);
            Debug.WriteLine(
                $"Property on {@do}, name {prop.Name} (Value {value} [{lv}]): {s.BaseValueSource}; {s.IsCoerced}; {s.IsCurrent}; {s.IsExpression};");
            if (s.IsExpression)
            {
                if (@do is FrameworkElement fe)
                {
                    Debug.WriteLine($"Data Context = {fe.DataContext}");
                }
                
                var binding = BindingOperations.GetBinding(@do, prop);
                // DumpBinding(@do, prop, binding);
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
                Debug.WriteLine($"{target} {prop.Name} {propertyInfo.Name} {sb}");
            }
        }

        public static void WriteLine(string s)
        {
            Debug.WriteLine(s);
        }
    }
}