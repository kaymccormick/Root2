using System.Windows;
using Microsoft.Graph;
using NLog;
using NLog.Fluent;

namespace AnalysisControls
{
    public class RibbonDebugUtils
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();
        public static void OnPropertyChanged(string LoggingIdentifier, DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            new LogBuilder(Logger).LoggerName(LoggingIdentifier + ".OnPropertyChanged." + e.Property.Name)
                .Level(LogLevel.Info).Message($"OldValue = {e.OldValue}; NewValue = {e.NewValue}").Write();
        }
    }
}