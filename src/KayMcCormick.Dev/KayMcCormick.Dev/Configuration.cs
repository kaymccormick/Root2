using JetBrains.Annotations;

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
        
        AppConfig,
        /// <summary>
        /// 
        /// </summary>
        
        EnvironmentVariable,
        /// <summary>
        /// 
        /// </summary>
        
        Win32Registry,
        /// <summary>
        /// 
        /// </summary>
        
        Compilation,
    }

    /// <summary>
    /// 
    /// </summary>
    
    public enum ConfigurationSetting
    {
        /// <summary>
        /// 
        /// </summary>
        
        LogsRootDirectory,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        
        void LoadConfiguration();
    }

#if USEREGISTRY
    /// <summary>
    /// 
    /// </summary>
    public class Win32RegistryConfiguration : IConfiguration
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 
        /// </summary>
        public void LoadConfiguration()
        {
            var x = new[] { Registry.LocalMachine, Registry.CurrentUser };
            foreach (var q in x)
            {
                var mainSubKey = q.OpenSubKey(@"SOFTWARE");
                Logger.Debug("{key}", mainSubKey);
                var e = mainSubKey?.OpenSubKey("Kay McCormick");
                Logger.Debug("k: {k}", e);
            }
        }
    }
#endif
}