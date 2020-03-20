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
using System.Diagnostics ;
using System.Linq ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
#pragma warning disable DV2002 // Unmapped types
    public class ResourceTemplateSelector : DataTemplateSelector
#pragma warning restore DV2002 // Unmapped types
    {
#if TRACE_TEMPLATES
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
#else
        private static readonly Logger Logger = LogManager.CreateNullLogger ( ) ;
#endif


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
            object resourceKey = null ;
            DataTemplate returnVal = null ;

            var resourceKeys = new List < object > ( ) ;
            if ( item is ControlWrap < Window > )
            {
                resourceKeys.Add ( "WindowTemplate" ) ;
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
                resourceKeys.Add (
                                  $"{TemplatePartName}{item.GetType ( ).Name.Replace ( "." , "_" )}"
                                 ) ;
            }

            if ( resourceKeys.Any ( ) )
            {
                returnVal = resourceKeys
                           .Select < object , DataTemplate > (
                                                              ( k ) => TryFindDataTemplate (
                                                                                            fe
                                                                                          , k
                                                                                           )
                                                             )
                           .FirstOrDefault ( template => template != null ) ;
                if ( returnVal != null )
                {
                    Logger.Debug ( "OBtained template" ) ;
                    Debug.WriteLine ( XamlWriter.Save ( returnVal ) ) ;
                }
            }

            if ( returnVal == null )
            {
                Logger.Info ( "Calling base method for template" ) ;
                returnVal = base.SelectTemplate ( item , container ) ;
                if ( returnVal != null )
                {
                    Logger.Info ( "Got template from base method" ) ;
                }
                else
                {
                    Logger.Info ( "no template from base method" ) ;
                }
            }

            return returnVal ;
        }

        public virtual string TemplatePartName { get ; set ; }


        private DataTemplate TryFindDataTemplate ( FrameworkElement fe , object resourceKey )
        {
            Logger.Debug (
                          "Trying to find data template with resource key {resourceKey}"
                        , resourceKey
                         ) ;
            var resource = fe.TryFindResource ( resourceKey ) ;
            if ( resource != null )
            {
                Logger.Debug ( "Found resource of type {resourceType}" , resource.GetType ( ) ) ;
                if ( resource is HierarchicalDataTemplate )
                {
                    Logger.Debug ( "suppressing heirarchicala data template" ) ;
                    return null ;
                }

                return resource as DataTemplate ;
            }

            return null ;
        }
    }
}