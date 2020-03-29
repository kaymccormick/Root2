﻿using System ;
using System.Collections ;
using System.Collections.ObjectModel ;
using System.Windows ;
using JetBrains.Annotations ;
using NLog ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>Methods for collecting resources from WPF elements</summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ResourcesUtil
    public static class ResourcesUtil
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        /// <summary>
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="haveResources"></param>
        /// <param name="resourcesCollection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void CollectResources (
            Type                                  ownerType
          , [ NotNull ] FrameworkElement          haveResources
          , ObservableCollection < ResourceInfo > resourcesCollection
        )
        {
            if ( haveResources == null )
            {
                throw new ArgumentNullException ( nameof ( haveResources ) ) ;
            }

            var res = haveResources.Resources ;
            AddResourceInfos ( ownerType , res , resourcesCollection ) ;

            foreach ( var child in LogicalTreeHelper.GetChildren ( haveResources ) )
            {
                if ( child is FrameworkElement e )
                {
                    CollectResources ( child.GetType ( ) , e , resourcesCollection ) ;
                }
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <param name="res"></param>
        /// <param name="resourcesCollection"></param>
        /// <param name="parent"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddResourceInfos (
            [ NotNull ] Type                                  type
          , [ NotNull ] ResourceDictionary                    res
          , [ NotNull ] ObservableCollection < ResourceInfo > resourcesCollection
          , ResourceDictionary                                parent = null
        )
        {
            if ( type == null )
            {
                throw new ArgumentNullException ( nameof ( type ) ) ;
            }

            if ( res == null )
            {
                throw new ArgumentNullException ( nameof ( res ) ) ;
            }

            if ( resourcesCollection == null )
            {
                throw new ArgumentNullException ( nameof ( resourcesCollection ) ) ;
            }

            var resourcesSource = res.Source ;

            // Logger.Info ( "Resources source is {res.Source}, parent is {parent}", res.Source.ToString(), parent.ToString() ) ;
            foreach ( DictionaryEntry haveResourcesResource in res )
            {
                if ( haveResourcesResource.Key != null )
                {
                    if ( haveResourcesResource.Value != null )
                    {
                        var resourceInfo = new ResourceInfo (
                                                             resourcesSource
                                                           , haveResourcesResource.Key
                                                           , haveResourcesResource.Value
                                                           , type
                                                           , parent
                                                            ) ;
                        // Logger.Info (
                        // "resource has {key} and {value}"
                        // , haveResourcesResource.Key
                        // , haveResourcesResource.Value
                        // ) ;
                        resourcesCollection.Add ( resourceInfo ) ;
                    }
                }
            }

            Logger.Info ( "processing meregd dictionaries" ) ;
            foreach ( var r in res.MergedDictionaries )
            {
                if ( r != null )
                {
                    AddResourceInfos ( type , r , resourcesCollection , res ) ;
                }
                else
                {
                    Logger.Warn ( "null element in merged dictionaries" ) ;
                }
            }
        }
    }
}