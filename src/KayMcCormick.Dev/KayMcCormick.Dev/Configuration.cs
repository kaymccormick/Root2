using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text ;
using System.Threading.Tasks ;
using Microsoft.Win32 ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public enum ConfigurationSource
    {
        AppConfig , EnvironmentVariable , Win32Registry , Compilation ,
    }

    public enum ConfigurationSetting { LogsRootDirectory , }

    public interface IConfiguration
    {
        void LoadConfiguration ( ) ;
    }

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

        public Configuration ( ) { GetConfigurationSources ( ) ; }

        private static void GetConfigurationSources ( ) { }

        //"http://xx1.mynetgear.com/LogService/ReceiveLogs.svc"
    }
}