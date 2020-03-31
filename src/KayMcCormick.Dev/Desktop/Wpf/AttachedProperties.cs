﻿using System ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Markup ;
using Autofac ;
using JetBrains.Annotations ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>Static class containing shared attached properties.
    /// </summary>
    public static class AttachedProperties
    {
        /// <summary>Event triggered upon change of the lifetime scope property.</summary>
        /// <seealso cref="LifetimeScopeProperty"/>
        public static readonly RoutedEvent LifetimeScopeChangedEvent =
            EventManager.RegisterRoutedEvent (
                                              "LifetimeScopeChanged"
                                            , RoutingStrategy.Bubble
                                            , typeof ( RoutedPropertyChangedEventHandler <
                                                  ILifetimeScope > )
                                            , typeof ( AttachedProperties )
                                             ) ;

        /// <summary>The lifetime scope property</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for LifetimeScopeProperty
        public static readonly DependencyProperty LifetimeScopeProperty =
            DependencyProperty.RegisterAttached (
                                                 "LifetimeScope"
                                               , typeof ( ILifetimeScope )
                                               , typeof ( AttachedProperties )
                                               , new FrameworkPropertyMetadata (
                                                                                null
                                                                              , FrameworkPropertyMetadataOptions
                                                                                   .Inherits
                                                                              , OnLifetimeScopeChanged
                                                                              , CoerceLifetimeScope
                                                                              , false
                                                                              , UpdateSourceTrigger
                                                                                   .PropertyChanged
                                                                               )
                                                ) ;


        /// <summary>The rendered type changed event</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for RenderedTypeChangedEvent
        public static readonly RoutedEvent RenderedTypeChangedEvent =
            EventManager.RegisterRoutedEvent (
                                              "RenderedTypeChanged"
                                            , RoutingStrategy.Direct
                                            , typeof ( RoutedPropertyChangedEventHandler < Type > )
                                            , typeof ( AttachedProperties )
                                             ) ;

        /// <summary>The rendered type property</summary>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for RenderedTypeProperty
        public static readonly DependencyProperty RenderedTypeProperty =
            DependencyProperty.RegisterAttached (
                                                 "RenderedType"
                                               , typeof ( Type )
                                               , typeof ( AttachedProperties )
                                               , new FrameworkPropertyMetadata (
                                                                                null
                                                                              , FrameworkPropertyMetadataOptions
                                                                                   .Inherits
                                                                              , OnRenderedTypeChanged
                                                                              , CoerceRenderedType
                                                                              , false
                                                                              , UpdateSourceTrigger
                                                                                   .PropertyChanged
                                                                               )
                                                ) ;

        private static object CoerceLifetimeScope (
            [ NotNull ] DependencyObject d
          , object                       baseValue
        )
        {
            if ( d == null )
            {
                throw new ArgumentNullException ( nameof ( d ) ) ;
            }

            return baseValue ;
        }

        private static void OnLifetimeScopeChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
            var evt = LifetimeScopeChangedEvent ;
            var ev = new RoutedPropertyChangedEventArgs < ILifetimeScope > (
                                                                            ( ILifetimeScope ) e
                                                                               .OldValue
                                                                          , ( ILifetimeScope ) e
                                                                               .NewValue
                                                                          , evt
                                                                           ) ;
            switch ( d )
            {
                case UIElement uie :
#if TRACE_EVENTS
                Logger.Trace ( $"Raising event on UIElement {d.GetType ()}  {evt.Name}" ) ;
#endif
                    uie.RaiseEvent ( ev ) ;
                    break ;
                case ContentElement ce :
#if TRACE_EVENTS
                Logger.Trace ( $"Raising event on ContentElement {evt.Name}" ) ;
#endif
                    ce.RaiseEvent ( ev ) ;
                    break ;
                default :
#if TRACE_EVENTS
                Logger.Trace ( $"Raising event on incompatible type {evt.Name}" ) ;
#endif
                    break ;
            }
        }

        /// <summary>Adds the lifetime scope changed event handler.</summary>
        /// <param name="d">The d.</param>
        /// <param name="handler">The handler.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for AddLifetimeScopeChangedEventHandler
        public static void AddLifetimeScopeChangedEventHandler (
            DependencyObject   d
          , RoutedEventHandler handler
        )
        {
            switch ( d )
            {
                case UIElement uie :
                    uie.AddHandler ( LifetimeScopeChangedEvent , handler ) ;
                    break ;
                case ContentElement ce :
                    ce.AddHandler ( LifetimeScopeChangedEvent , handler ) ;
                    break ;
            }
        }

        /// <summary>Removes the lifetime scope changed event handler.</summary>
        /// <param name="d">The d.</param>
        /// <param name="handler">The handler.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for RemoveLifetimeScopeChangedEventHandler
        public static void RemoveLifetimeScopeChangedEventHandler (
            DependencyObject   d
          , RoutedEventHandler handler
        )
        {
            switch ( d )
            {
                case UIElement uie :
                    uie.AddHandler ( LifetimeScopeChangedEvent , handler ) ;
                    break ;
                case ContentElement ce :
                    ce.AddHandler ( LifetimeScopeChangedEvent , handler ) ;
                    break ;
            }
        }

        /// <summary>Gets the lifetime scope.</summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetLifetimeScope
        [ AttachedPropertyBrowsableForType ( typeof ( Window ) ) ]
        [ AttachedPropertyBrowsableForType ( typeof ( UserControl ) ) ]
        [ AttachedPropertyBrowsableForType ( typeof ( FrameworkElement ) ) ]
        [ Ambient ]
        public static ILifetimeScope GetLifetimeScope ( [ NotNull ] DependencyObject target )
        {
            if ( target == null )
            {
                throw new ArgumentNullException ( nameof ( target ) ) ;
            }

            return ( ILifetimeScope ) target.GetValue ( LifetimeScopeProperty ) ;
        }

        /// <summary>Sets the lifetime scope.</summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for SetLifetimeScope
        public static void SetLifetimeScope (
            [ NotNull ] DependencyObject target
          , ILifetimeScope               value
        )
        {
            if ( target == null )
            {
                throw new ArgumentNullException ( nameof ( target ) ) ;
            }
#if TRACE_EVENTS
            Logger.Trace ( $"{nameof ( SetMenuItemListCollectionView )} {target}, {value}" ) ;
#endif

            target.SetValue ( LifetimeScopeProperty , value ) ;
        }

        private static object CoerceRenderedType ( DependencyObject d , object baseValue )
        {
            return baseValue ;
        }


        private static void OnRenderedTypeChanged (
            DependencyObject                   d
          , DependencyPropertyChangedEventArgs e
        )
        {
            var evt = RenderedTypeChangedEvent ;
            var ev = new RoutedPropertyChangedEventArgs < Type > (
                                                                  ( Type ) e.OldValue
                                                                , ( Type ) e.NewValue
                                                                , evt
                                                                 ) ;
            switch ( d )
            {
                case UIElement uie :
#if TRACE_EVENTS
                Logger.Trace ( $"Raising event on UIElement {evt.Name}" ) ;
#endif
                    uie.RaiseEvent ( ev ) ;
                    break ;
                case ContentElement ce :
#if TRACE_EVENTS
                Logger.Trace ( $"Raising event on ContentElement {evt.Name}" ) ;
#endif
                    ce.RaiseEvent ( ev ) ;
                    break ;
                default :
#if TRACE_EVENTS
                Logger.Trace ( $"Raising event on incompatible type {evt.Name}" ) ;
#endif
                    break ;
            }
        }


        /// <summary>Adds the rendered type changed event handler.</summary>
        /// <param name="d">The d.</param>
        /// <param name="handler">The handler.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for AddRenderedTypeChangedEventHandler
        public static void AddRenderedTypeChangedEventHandler (
            DependencyObject   d
          , RoutedEventHandler handler
        )
        {
            switch ( d )
            {
                case UIElement uie :
                    uie.AddHandler ( RenderedTypeChangedEvent , handler ) ;
                    break ;
                case ContentElement ce :
                    ce.AddHandler ( RenderedTypeChangedEvent , handler ) ;
                    break ;
            }
        }

        /// <summary>Removes the rendered type changed event handler.</summary>
        /// <param name="d">The d.</param>
        /// <param name="handler">The handler.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for RemoveRenderedTypeChangedEventHandler
        public static void RemoveRenderedTypeChangedEventHandler (
            DependencyObject   d
          , RoutedEventHandler handler
        )
        {
            switch ( d )
            {
                case UIElement uie :
                    uie.AddHandler ( RenderedTypeChangedEvent , handler ) ;
                    break ;
                case ContentElement ce :
                    ce.AddHandler ( RenderedTypeChangedEvent , handler ) ;
                    break ;
            }
        }

        /// <summary>Gets the type of the rendered.</summary>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetRenderedType
        [ AttachedPropertyBrowsableForType ( typeof ( Window ) ) ]
        public static Type GetRenderedType ( [ NotNull ] DependencyObject target )
        {
            if ( target == null )
            {
                throw new ArgumentNullException ( nameof ( target ) ) ;
            }

            return ( Type ) target.GetValue ( RenderedTypeProperty ) ;
        }

        /// <summary>Sets the type of the rendered.</summary>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for SetRenderedType
        public static void SetRenderedType ( [ NotNull ] DependencyObject target , Type value )
        {
            if ( target == null )
            {
                throw new ArgumentNullException ( nameof ( target ) ) ;
            }
#if TRACE_EVENTS
            Logger.Trace ( $"{nameof ( SetMenuItemListCollectionView )} {target}, {value}" ) ;
#endif

            target.SetValue ( RenderedTypeProperty , value ) ;
        }
    }
}