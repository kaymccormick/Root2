﻿using System.Collections ;
using System.Collections.ObjectModel ;
using System.Windows ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>Methods for collecting resources from WPF elements</summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ResourcesUtil
    // ReSharper disable once UnusedType.Global
    public static class ResourcesUtil
    {
        // ReSharper disable once UnusedMember.Local
        private static void CollectResources (
            FrameworkElement                      haveResources
          , ObservableCollection < ResourceInfo > resourcesCollection
        )
        {
            var res = haveResources.Resources ;
            AddResourceInfos ( res , resourcesCollection ) ;

            foreach ( var child in LogicalTreeHelper.GetChildren ( haveResources ) )
            {
                if ( child is FrameworkElement e )
                {
                    CollectResources ( e , resourcesCollection ) ;
                }
            }
        }

        private static void AddResourceInfos (
            ResourceDictionary                    res
          , ObservableCollection < ResourceInfo > resourcesCollection
        )
        {
            var resourcesSource = res.Source ;
            foreach ( DictionaryEntry haveResourcesResource in res )
            {
                resourcesCollection.Add (
                                         new ResourceInfo (
                                                           resourcesSource
                                                         , haveResourcesResource.Key
                                                         , haveResourcesResource.Value
                                                          )
                                        ) ;
            }

            foreach ( var r in res.MergedDictionaries )
            {
                AddResourceInfos ( r , resourcesCollection ) ;
            }
        }
    }
}