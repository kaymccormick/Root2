using System.Configuration ;
using KayMcCormick.Dev.Attributes ;

namespace KayMcCormick.Dev
{
    /// <summary>Configuration section handler for container helper settings.</summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [ConfigTarget( typeof ( LoggerSettings ))]
    public class LoggingSection : ConfigurationSection
    {
    }
}