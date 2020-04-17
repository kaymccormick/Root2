#region header
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
using System ;
using System.Windows ;
using System.Windows.Data ;
using System.Windows.Input ;
using JetBrains.Annotations ;

namespace AnalysisControls
{
    /// <summary>
    ///     <para>Helper class for wpf explorer</para>
    /// </summary>
    public static class InputBindingsManager
    {
        /// <summary>
        /// </summary>
        public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty =
            DependencyProperty.RegisterAttached (
                                                 "UpdatePropertySourceWhenEnterPressed"
                                               , typeof ( DependencyProperty )
                                               , typeof ( InputBindingsManager )
                                               , new PropertyMetadata (
                                                                       null
                                                                     , OnUpdatePropertySourceWhenEnterPressedPropertyChanged
                                                                      )
                                                ) ;

        static InputBindingsManager ( ) { }

        /// <summary>
        /// </summary>
        /// <param name="dp"></param>
        /// <param name="value"></param>
        public static void SetUpdatePropertySourceWhenEnterPressed (
            [ NotNull ] DependencyObject dp
          , DependencyProperty           value
        )
        {
            dp.SetValue ( UpdatePropertySourceWhenEnterPressedProperty , value ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="dp"></param>
        /// <returns></returns>
        public static DependencyProperty GetUpdatePropertySourceWhenEnterPressed (
            [ NotNull ] DependencyObject dp
        )
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
            if ( ! ( dp is UIElement element ) )
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

        private static void HandlePreviewKeyDown ( object sender , [ NotNull ] KeyEventArgs e )
        {
            if ( e.Key == Key.Enter )
            {
                DoUpdateSource ( e.Source ) ;
            }
        }

        private static void DoUpdateSource ( [ NotNull ] object source )
        {
            var property =
                GetUpdatePropertySourceWhenEnterPressed (
                                                         source as DependencyObject
                                                         ?? throw new InvalidOperationException ( )
                                                        ) ;
            if ( property == null )
            {
                return ;
            }

            if ( ! ( source is UIElement elt ) )
            {
                return ;
            }

            BindingOperations.GetBindingExpression ( elt , property )?.UpdateSource ( ) ;
        }
    }
}