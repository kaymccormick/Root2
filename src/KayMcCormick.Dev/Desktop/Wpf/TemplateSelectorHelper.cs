#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// KayMcCormick.Lib.Wpf
// TemplateSelectorHelper.cs
// 
// 2020-03-22-7:52 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.ComponentModel ;
using System.Diagnostics ;
using System.Linq ;
using System.Windows ;
using System.Windows.Markup ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>Helper class for template selection. Not yet optimized but has a variety of template selection methods to facilitate a wide range of applications.</summary>
    public static class TemplateSelectorHelper

    {
        public static DataTemplate HelpSelectDataTemplate (
            object                                            item
          , DependencyObject                                  container
          , Func < object , DependencyObject , DataTemplate > baseMethod
          , Predicate < DataTemplate >                        predicate = null
        )
        {
            if ( item == null )
            {
                Debug.WriteLine ( "Selecting template for NuLL item" ) ;
            }
            else
            {
                Debug.WriteLine ( $"Selecting template for {item} {item.GetType ( )}" ) ;
            }

            var fe = ( FrameworkElement ) container ;
            DataTemplate returnVal = null ;

            var resourceKeys = new List < object > ( ) ;

            if ( item != null )
            {
                var t = item.GetType ( ) ;
                while ( t != typeof ( object ) )
                {
                    resourceKeys.Add ( new DataTemplateKey ( t ) ) ;
                    if ( t != null )
                    {
                        t = t.BaseType ;
                    }
                }

                resourceKeys.AddRange (
                                       item.GetType ( )
                                           .GetInterfaces ( )
                                           .Select ( iface => new DataTemplateKey ( iface ) )
                                      ) ;
            }

            if ( resourceKeys.Any ( ) )
            {
                var tuple1 = resourceKeys
                            .Select (
                                     ( k ) => Tuple.Create (
                                                            k
                                                          , fe
                                                          , TryFindDataTemplate (
                                                                                 fe
                                                                               , k
                                                                               , predicate
                                                                                )
                                                           )
                                    )
                            .Where ( Predicate2 )
                            .FirstOrDefault ( ) ;
                if ( tuple1 != null )
                {
                    returnVal = tuple1.Item3 ;
                    Debug.WriteLine ( "Obtained template" ) ;
                    try
                    {
                        Debug.WriteLine ( XamlWriter.Save ( returnVal ) ) ;
                    }
                    catch ( Exception ex )
                    {
                        Debug.WriteLine ( ex.ToString ( ) ) ;
                    }
                }
            }

            if ( returnVal == null )
            {
                Debug.WriteLine ( "Calling base method for template" ) ;

                returnVal = baseMethod?.Invoke ( item , container ) ;
                if ( returnVal != null )
                {
                    Debug.WriteLine ( "Got template from base method" ) ;
                }
                else
                {
                    Debug.WriteLine ( "no template from base method" ) ;
                }
            }

            return returnVal ;
        }

        private static bool Predicate2 ( Tuple < object , FrameworkElement , DataTemplate > arg )
        {
            var (item1 , item2 , item3) = arg ;
            Debug.WriteLine ( item1 ) ;
            Debug.WriteLine ( $"[ {item1} ]\t\t\t{item2} {item3}" ) ;
            return item3 != null ;
        }


        private static DataTemplate TryFindDataTemplate (
            FrameworkElement           fe
          , object                     resourceKey
          , Predicate < DataTemplate > predicate
        )
        {
            Debug.WriteLine ( $"Trying to find data template with resource key {resourceKey}" ) ;

            var resource = fe.TryFindResource ( resourceKey ) ;
            if ( resource != null )
            {
                Debug.WriteLine ( $"Found resource of type {resource.GetType ( )}" ) ;
                var dt = ( DataTemplate ) resource ;
                if ( predicate != null
                     && ! predicate ( dt ) )
                {
                    Debug.WriteLine ( "rejecting data template based on predicate" ) ;
                    return null ;
                }

                return resource as DataTemplate ;
            }

            return null ;
        }
    }
}