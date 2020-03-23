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

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ApplicationInstanceBase
    {
        /// <summary>
        /// </summary>
        /// <param name="logMethod"></param>
        protected ApplicationInstanceBase ( LogDelegates.LogMethod logMethod )
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
        // ReSharper disable once EventNeverSubscribedTo.Global
        public virtual event EventHandler < AppStartupEventArgs > AppStartup ;

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public virtual void Initialize ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="appModule"></param>
        // ReSharper disable once UnusedMember.Global
        public abstract void AddModule ( IModule appModule ) ;

        /// <summary>
        /// </summary>
        /// <returns></returns>
        [ UsedImplicitly ]
        public virtual ILifetimeScope GetLifetimeScope ( )
        {
            throw new NotImplementedException();
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
        // ReSharper disable once UnusedMember.Global
        public virtual void Shutdown ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="logMethod2"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        protected abstract IEnumerable LoadConfiguration (
            [ NotNull ] LogDelegates.LogMethod logMethod2
        ) ;

        /// <summary>
        /// </summary>
        public abstract void Dispose ( ) ;
    }
}