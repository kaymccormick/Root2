using JetBrains.Annotations ;
using Microsoft.Win32 ;
using NLog ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public enum ConfigurationSource
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        AppConfig , 
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        EnvironmentVariable , 
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        Win32Registry , 
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        Compilation ,
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public enum ConfigurationSetting
    {
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        LogsRootDirectory ,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        [ UsedImplicitly ]
        void LoadConfiguration ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    public class Win32RegistryConfiguration : IConfiguration
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        /// 
        /// </summary>
        public void LoadConfiguration ( )
        {
            var x = new[] { Registry.LocalMachine , Registry.CurrentUser } ;
            foreach ( var q in x )
            {
                var mainSubKey = q.OpenSubKey ( @"SOFTWARE" ) ;
                Logger.Debug ( "{key}" , mainSubKey ) ;
                var e = mainSubKey?.OpenSubKey ( "Kay McCormick" ) ;
                Logger.Debug ( "k: {k}" , e ) ;
            }
        }
    }
}