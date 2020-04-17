using System.Collections.Generic ;
using System.Runtime.Serialization ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using KayMcCormick.Lib.Wpf ;
using NLog ;
using Logger = NLog.Logger ;

namespace ProjInterface
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public sealed class DockWindowViewModel : IViewModel
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        public DockWindowViewModel ([ NotNull ] ILifetimeScope scope )
        {
             var views = scope.Resolve < IEnumerable < IControlView > > ( ) ;
            foreach ( var controlView in views )
            {
                DebugUtils.WriteLine(controlView.ToString());

            }
        }

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}