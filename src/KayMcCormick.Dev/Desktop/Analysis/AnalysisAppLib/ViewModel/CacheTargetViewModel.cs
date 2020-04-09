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
using System.Diagnostics ;
using System.Linq ;
using System.Reactive.Concurrency ;
using System.Reactive.Linq ;
using System.Text.Json ;
using System.Windows.Threading ;
using KayMcCormick.Dev ;
using KayMcCormick.Dev.Logging ;

namespace AnalysisAppLib.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
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
                                             try
                                             {
                                                 var i = JsonSerializer
                                                    .Deserialize < LogEventInstance > (
                                                                                       json
                                                                                     , new
                                                                                           JsonSerializerOptions ( )
                                                                                      ) ;

                                                 Events.Add ( i ) ;
                                             }
                                             catch ( Exception ex )
                                             {
                                                 throw ;
                                                 DebugUtils.WriteLine ( ex.ToString ( ) ) ;
                                             }
                                         }
                                     }
                                    ) ;
        }
    }
}