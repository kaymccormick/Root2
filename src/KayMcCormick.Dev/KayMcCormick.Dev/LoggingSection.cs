
using System.Configuration ;
using System.Diagnostics ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Attributes ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>Configuration section handler for container helper settings.</summary>
    /// <seealso cref="System.Configuration.ConfigurationSection" />
    [ConfigTarget( typeof ( LoggerSettings ))]
    [ UsedImplicitly ]
    public class LoggingSection : ConfigurationSection

    {
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty("IsEnabledConsoleTarget", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool? IsEnabledConsoleTarget { get => (bool?)this[nameof(IsEnabledConsoleTarget)]; set => this[nameof(IsEnabledConsoleTarget)] = value; }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty( "IsEnabledCacheTarget", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool? IsEnabledCacheTarget { get => (bool?)this[nameof(IsEnabledCacheTarget)]; set => this[nameof(IsEnabledCacheTarget)] = value; }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty( "LogThrowExceptions", DefaultValue = false, IsRequired = false, IsKey = false)]
        public bool? LogThrowExceptions { get => (bool?)this[nameof(LogThrowExceptions)]; set => this[nameof(LogThrowExceptions)] = value; }
        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty( "MinLogLevel", DefaultValue = "Trace", IsRequired = false, IsKey = false)]
        public LogLevel MinLogLevel
        {
            get => 
                (LogLevel)this[nameof(MinLogLevel)];
            set
            {
                var level = LogLevel.FromString ( value.ToString ( ) ) ;
                Debug.WriteLine ( $"Setting MinLogLevel to {level}" ) ;
                this[ nameof ( MinLogLevel ) ] = level ;
            }
        }

    
    }
}