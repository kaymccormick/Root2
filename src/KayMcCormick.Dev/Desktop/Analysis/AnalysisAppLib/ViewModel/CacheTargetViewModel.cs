#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// CacheTargetViewModel.cs
// 
// 2020-03-23-4:42 AM
// 
// ---
#endregion
using System ;
using System.Linq ;
using System.Reactive.Concurrency ;
using System.Reactive.Linq ;
using System.Text.Json ;
using System.Windows.Threading ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public sealed class CacheTargetViewModel
    {
        private readonly MyCacheTarget2 _cacheTarget ;

        private readonly LogEventInstanceObservableCollection _events =
            new LogEventInstanceObservableCollection ( ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheTarget"></param>
        public CacheTargetViewModel ( MyCacheTarget2 cacheTarget ) { _cacheTarget = cacheTarget ; }

        /// <summary>
        /// 
        /// </summary>
        public LogEventInstanceObservableCollection Events { get { return _events ; } }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void Attach ( )
        {
            _cacheTarget?.Cache.SubscribeOn ( Scheduler.Default )
                         .Buffer ( TimeSpan.FromMilliseconds ( 100 ) )
                         .Where ( x => x.Any ( ) )
                         .ObserveOnDispatcher ( DispatcherPriority.Background )
                         .Subscribe (
                                     infos => {
                                         foreach ( var json in infos )
                                         {
                                             var i = JsonSerializer
                                                .Deserialize < LogEventInstance > (
                                                                                   json
                                                                                 , new
                                                                                       JsonSerializerOptions ( )
                                                                                  ) ;

                                             Events.Add ( i ) ;
                                         }
                                     }
                                    ) ;
        }
    }
}