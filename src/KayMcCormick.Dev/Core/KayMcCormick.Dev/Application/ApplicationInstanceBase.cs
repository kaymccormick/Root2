#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Dev
// ApplicationInstanceBase.cs
// 
// 2020-03-20-12:59 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using Autofac ;
using Autofac.Core ;
using JetBrains.Annotations ;
using KayMcCormick.Dev.Logging ;

namespace KayMcCormick.Dev.Application
{
    /// <summary>
    /// </summary>
    public abstract class ApplicationInstanceBase
    {
        /// <summary>
        /// </summary>
        /// <param name="logMethod"></param>
        protected ApplicationInstanceBase ( ApplicationInstance.LogMethodDelegate logMethod )
        {
            InstanceRunGuid = Guid.NewGuid ( ) ;
        }

        /// <summary>
        /// </summary>
        public Guid InstanceRunGuid { get ; }

        /// <summary>
        /// </summary>
        protected List < object > ConfigSettings { get ; } = new List < object > ( ) ;

        /// <summary>
        /// </summary>
        public virtual event EventHandler < AppStartupEventArgs > AppStartup ;

        /// <summary>
        /// </summary>
        public virtual void Initialize ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="appModule"></param>
        public abstract void AddModule ( IModule appModule ) ;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public virtual ILifetimeScope GetLifetimeScope ( )
        {
            throw new NotImplementedException ( ) ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        protected abstract IContainer BuildContainer ( ) ;

        /// <summary>
        /// </summary>
        public virtual void Startup ( ) { OnAppStartup ( new AppStartupEventArgs ( ) ) ; }

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        private void OnAppStartup ( AppStartupEventArgs e ) { AppStartup?.Invoke ( this , e ) ; }

        /// <summary>
        /// </summary>
        /// todo call from wpf
        public virtual void Shutdown ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="logMethod2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected abstract IEnumerable LoadConfiguration (
            [ NotNull ] Action < string > logMethod2
        ) ;

        /// <summary>
        /// </summary>
        public abstract void Dispose ( ) ;
    }
}