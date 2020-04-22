#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// Token.cs
// 
// 2020-03-02-9:12 AM
// 
// ---
#endregion
using System.Globalization ;
using System.Windows ;
using System.Windows.Media ;
using JetBrains.Annotations ;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class Token : UIElement
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly bool _newLine ;

        private readonly Size          _desiredSize ;
        private readonly FormattedText _formattedText ;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawKind"></param>
        /// <param name="text"></param>
        /// <param name="solidColorBrush"></param>
        /// <param name="newLine"></param>
        // ReSharper disable once UnusedParameter.Local
        // ReSharper disable once MemberCanBeInternal
        internal Token ( int rawKind , string text , [ CanBeNull ] SolidColorBrush solidColorBrush , bool newLine )
        {
            _newLine  = newLine ;
            var typeface = new Typeface ( "Courier New" ) ;
            _formattedText = new FormattedText (
                                                text
                                              , CultureInfo.CurrentCulture
                                              , FlowDirection.LeftToRight
                                              , typeface
                                              , 20
                                              , solidColorBrush ?? Brushes.Black
                                              , new NumberSubstitution (
                                                                        NumberCultureSource.User
                                                                      , CultureInfo.CurrentUICulture
                                                                      , NumberSubstitutionMethod
                                                                           .AsCulture
                                                                       )
                                              , TextFormattingMode.Display
                                              , 1.25
                                               ) ;
            _desiredSize = new Size ( _formattedText.Width , _formattedText.Height ) ;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        protected override Size MeasureCore ( Size availableSize ) { return _desiredSize ; }

        #region Overrides of UIElement
        /// <summary>
        /// 
        /// </summary>
        /// <param name="drawingContext"></param>
        protected override void OnRender ( DrawingContext drawingContext )
        {
            base.OnRender ( drawingContext ) ;
            drawingContext.DrawText ( _formattedText , new Point ( 0 , 0 ) ) ;
        }
        #endregion
    }
}