using System ;
using System.ComponentModel ;
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public sealed class ResourceTreeViewItemTemplateSelector : DataTemplateSelector

    {
#if TRACE_TEMPLATES
        private static Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif


        #region Overrides of DataTemplateSelector
        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override DataTemplate SelectTemplate (
            object                       item
          , [ NotNull ] DependencyObject container
        )
        {
            if ( container == null )
            {
                throw new ArgumentNullException ( nameof ( container ) ) ;
            }



            Logger.Debug ( "item is {item}" , item ) ;



            var fe = ( FrameworkElement ) container ;

            if ( item is ResourceNodeInfo node )
            {
                if ( node.Data != null )
                {
                    var dType = node.Data.GetType ( ) ;
                    var dataTemplateKey = new DataTemplateKey ( dType ) ;
                    DebugUtils.WriteLine ( $"trying key {dataTemplateKey}" ) ;
                    var o = fe.TryFindResource ( dataTemplateKey ) ;
                    if ( o != null
                         && o is DataTemplate dt1 )
                    {
                        DebugUtils.WriteLine ( $"found key {dataTemplateKey}" ) ;
                        return dt1 ;
                    }

                    var dt = dType.GetInterfaces ( )
                                  .Select (
                                           x => Tuple.Create (
                                                              x
                                                            , fe
                                                            , fe.TryFindResource (
                                                                                  new
                                                                                      DataTemplateKey (
                                                                                                       x
                                                                                                      )
                                                                                 )
                                                             )
                                          )
                                  .Where ( Predicate2 )
                                  .Where ( Predicate3 )
                                  .FirstOrDefault ( ) ;
                    if ( dt != null )
                    {
                        DebugUtils.WriteLine ( $"using key {dt.Item1}" ) ;
                        return dt.Item3 as DataTemplate ;
                    }
                }
            }

            DebugUtils.WriteLine ( "using key ResourceNodeInfo;" ) ;
            var tryFindResource =
                fe.TryFindResource ( new DataTemplateKey ( typeof ( ResourceNodeInfo ) ) ) ;
            var dt2 = tryFindResource as DataTemplate ;
            return dt2 ;
        }


        private bool Predicate3 ( [ NotNull ] Tuple < Type , FrameworkElement , object > arg , int i )
        {
            var (item1 , _ , item3) = arg ;
            if ( item3 != null )
            {
                DebugUtils.WriteLine ( $"{nameof ( Predicate3 )}: {item3.GetType ( ).FullName}" ) ;
            }

            var r = item3 is DataTemplate ;
            if ( r )
            {
                Logger.Info ( "[{i}] Found Key {key}" , i , item1.FullName ) ;
            }

            return r ;
        }

        private static bool Predicate2 ( [ NotNull ] Tuple < Type , FrameworkElement , object > arg )
        {
            var (item1 , item2 , item3) = arg ;
            if ( item1         == typeof ( ISupportInitialize )
                 || item1.Name == "ISealable" )
            {
                return false ;
            }

            DebugUtils.WriteLine ( item1.FullName ) ;
            Logger.Debug (
                          "[ {type} ]\t\t\t{fe} {resource}"
                        , item1
                        , item2.ToString ( )
                        , item3?.ToString ( )
                         ) ;
            return true ;
        }

        // ReSharper disable once UnusedMember.Local
        private bool Predicate ( [ NotNull ] DataTemplate arg )
        {
            Logger.Debug ( arg.ToString ) ;

            return true ;
        }
        #endregion
    }
}