using Microsoft.Win32 ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConfigurationSource
    {
        /// <summary>
        /// 
        /// </summary>
        AppConfig , 
        /// <summary>
        /// 
        /// </summary>
        EnvironmentVariable , 
        /// <summary>
        /// 
        /// </summary>
        Win32Registry , 
        /// <summary>
        /// 
        /// </summary>
        Compilation ,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConfigurationSetting { LogsRootDirectory , }

    /// <summary>
    /// 
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        void LoadConfiguration ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    public class Win32RegistryConfiguration : IConfiguration
    {
        private Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public void LoadConfiguration ( )
        {
            var x = new[] { Registry.LocalMachine , Registry.CurrentUser } ;
            foreach ( var q in x )
            {
                var mainSubKey = q.OpenSubKey ( @"SOFTWARE" ) ;
                Logger.Debug ( "{key}" , mainSubKey ) ;
                var e = mainSubKey.OpenSubKey ( "Kay McCormick" ) ;
                Logger.Debug ( "k: {k}" , e ) ;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// 
        /// </summary>
        public const string LOGGING_WEBSERVICE_ENDPOINT = "LOGGING_WEBSERVICE_ENDPOINT" ;

        /// <summary>
        /// 
        /// </summary>
        public Configuration ( ) { GetConfigurationSources ( ) ; }

        private static void GetConfigurationSources ( ) { }

        //"http://xx1.mynetgear.com/LogService/ReceiveLogs.svc"
    }
}