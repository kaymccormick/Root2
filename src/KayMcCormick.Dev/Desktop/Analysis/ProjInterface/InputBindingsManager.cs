﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// InputBindingsManager.cs
// 
// 2020-03-20-7:53 PM
// 
// ---
#endregion
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Input ;

namespace ProjInterface
{
    public static class InputBindingsManager
    {
        public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty =
            DependencyProperty.RegisterAttached (
                                                 "UpdatePropertySourceWhenEnterPressed"
                                               , typeof ( DependencyProperty )
                                               , typeof ( InputBindingsManager )
                                               , new PropertyMetadata (
                                                                       null
#pragma warning disable WPF0005 // Name of PropertyChangedCallback should match registered name.
                                                                     , OnUpdatePropertySourceWhenEnterPressedPropertyChanged
#pragma warning restore WPF0005 // Name of PropertyChangedCallback should match registered name.
                                                                      )
                                                ) ;

        static InputBindingsManager ( ) { }

        public static void SetUpdatePropertySourceWhenEnterPressed (
            DependencyObject   dp
          , DependencyProperty value
        )
        {
            dp.SetValue ( UpdatePropertySourceWhenEnterPressedProperty , value ) ;
        }

        public static DependencyProperty
            GetUpdatePropertySourceWhenEnterPressed ( DependencyObject dp )
        {
            return ( DependencyProperty ) dp.GetValue (
                                                       UpdatePropertySourceWhenEnterPressedProperty
                                                      ) ;
        }

        private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged (
            DependencyObject                   dp
          , DependencyPropertyChangedEventArgs e
        )
        {
            var element = dp as UIElement ;

            if ( element == null )
            {
                return ;
            }

            if ( e.OldValue != null )
            {
                element.PreviewKeyDown -= HandlePreviewKeyDown ;
            }

            if ( e.NewValue != null )
            {
                element.PreviewKeyDown += HandlePreviewKeyDown ;
            }
        }

        private static void HandlePreviewKeyDown ( object sender , KeyEventArgs e )
        {
            if ( e.Key == Key.Enter )
            {
                DoUpdateSource ( e.Source ) ;
            }
        }

        private static void DoUpdateSource ( object source )
        {
            var property = GetUpdatePropertySourceWhenEnterPressed ( source as DependencyObject ) ;
            if ( property == null )
            {
                return ;
            }

            var elt = source as UIElement ;
            if ( elt == null )
            {
                return ;
            }

            BindingOperations.GetBindingExpression ( elt , property )?.UpdateSource ( ) ;
        }
    }
}