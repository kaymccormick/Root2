using System ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.ComponentModel ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using AnalysisAppLib.Dataflow ;
using AnalysisAppLib.Explorer ;
using Autofac ;
using Autofac.Features.Metadata ;
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
    public sealed class DockWindowViewModel : IViewModel , INotifyPropertyChanged
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly IEnumerable < Meta < Lazy < IViewWithTitle > > > _views ;

        private string _defaultInputPath =
            Environment.GetFolderPath ( Environment.SpecialFolder.MyDocuments ) ;

        private readonly ObservableCollection < AppExplorerItem > _rootCollection =
            new ObservableCollection < AppExplorerItem > ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="views"></param>
        /// <param name="blocks"></param>
        /// <param name="lifetime"></param>
        public DockWindowViewModel (
            IEnumerable < Meta < Lazy < IViewWithTitle > > > views
          , [ NotNull ] IEnumerable<IAnalysisBlockProvider1 > blocks
            , ILifetimeScope lifetime
        )
        {
            _views     = views ;
            foreach ( var analysisBlockProvider1 in blocks )
            {
                DebugUtils.WriteLine(analysisBlockProvider1.ToString());
            }

        }


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable < Meta < Lazy < IViewWithTitle > > > Views { get { return _views ; } }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection < AppExplorerItem > RootCollection
        {
            get { return _rootCollection ; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string DefaultInputPath
        {
            get { return _defaultInputPath ; }
            // ReSharper disable once UnusedMember.Global
            set { _defaultInputPath = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged ;

        #region Implementation of ISerializable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
        [ NotifyPropertyChangedInvocator ]
        // ReSharper disable once UnusedMember.Local
        private void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }
    }
}