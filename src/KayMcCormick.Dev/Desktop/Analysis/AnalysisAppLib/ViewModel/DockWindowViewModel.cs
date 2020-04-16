using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using AnalysisAppLib.Dataflow ;
using AnalysisAppLib.Explorer ;
using Autofac ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using NLog ;
using Logger = NLog.Logger ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    [ UsedImplicitly ]
    public sealed class DockWindowViewModel : IViewModel
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}