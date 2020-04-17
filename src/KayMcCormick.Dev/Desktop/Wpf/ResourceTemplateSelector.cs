#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// ResourceTemplateSelector.cs
// 
// 2020-03-19-9:33 PM
// 
// ---
#endregion
using System.Collections.Generic ;
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// </summary>
    public class ResourceTemplateSelector : DataTemplateSelector

    {
#if TRACE_TEMPLATES
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif


        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public override DataTemplate SelectTemplate ( object item , DependencyObject container )
        {
            if ( item == null )
            {
                Logger.Warn ( "Selecting detail template for NuLL item" ) ;
            }
            else
            {
                Logger.Debug (
                              "Selecting detail template for {item} {itemType}"
                            , item.ToString ( )
                            , item.GetType ( )
                             ) ;
            }

            var fe = ( FrameworkElement ) container ;
            DataTemplate returnVal = null ;

            var resourceKeys = new List < object > ( ) ;
            if ( item is ControlWrap < Window > )
            {
                resourceKeys.Add ( "WindowTemplate" ) ;
            }

            if ( item != null
                 && item.GetType ( ).IsGenericType
                 && item.GetType ( ).GetGenericTypeDefinition ( ) == typeof ( ControlWrap <> ) )
            {
                if ( item is IWrap1 w1 )
                {
                    var unwrappped = w1.ControlObject ;
                    resourceKeys.Add ( $"{TemplatePartName}{unwrappped.GetType ( ).Name}" ) ;
                }
            }

            if ( item is ResourceNodeInfo r )
            {
                if ( r.TemplateKey != null )
                {
                    resourceKeys.Add ( r.TemplateKey ) ;
                }
            }

            if ( item != null )
            {
                var try1 = $"{TemplatePartName}{item.GetType ( ).Name.Replace ( "." , "_" )}" ;
                DebugUtils.WriteLine ( try1 ) ;
                resourceKeys.Add ( try1 ) ;
            }

            if ( resourceKeys.Any ( ) )
            {
                returnVal = resourceKeys.Select ( k => TryFindDataTemplate ( fe , k ) )
                                        .FirstOrDefault ( template => template != null ) ;
                if ( returnVal != null )
                {
                    Logger.Debug ( "OBtained template" ) ;
                    DebugUtils.WriteLine ( XamlWriter.Save ( returnVal ) ) ;
                }
            }

            if ( returnVal != null )
            {
                return returnVal ;
            }

            Logger.Info ( "Calling base method for template" ) ;
            returnVal = base.SelectTemplate ( item , container ) ;
            Logger.Info (
                         returnVal != null
                             ? "Got template from base method"
                             : "no template from base method"
                        ) ;

            return returnVal ;
        }

        /// <summary>
        /// </summary>
        protected virtual string TemplatePartName { get ; set ; }


        [ CanBeNull ]
        private static DataTemplate TryFindDataTemplate ( [ NotNull ] FrameworkElement fe , [ NotNull ] object resourceKey )
        {
            Logger.Debug (
                          "Trying to find data template with resource key {resourceKey}"
                        , resourceKey
                         ) ;
            var resource = fe.TryFindResource ( resourceKey ) ;
            if ( resource == null )
            {
                return null ;
            }

            Logger.Debug ( "Found resource of type {resourceType}" , resource.GetType ( ) ) ;
            if ( resource is HierarchicalDataTemplate )
            {
                Logger.Debug ( "suppressing heirarchicala data template" ) ;
                return null ;
            }

            return resource as DataTemplate ;

        }
    }
}