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

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class Token : UIElement
    {
        private readonly bool _newLine ;

        private readonly string _text ;

        private readonly Typeface      _typeface ;
        private readonly Size          _desiredSize ;
        private readonly FormattedText _formattedText ;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawKind"></param>
        /// <param name="text"></param>
        /// <param name="solidColorBrush"></param>
        /// <param name="newLine"></param>
        public Token ( int rawKind , string text , SolidColorBrush solidColorBrush , bool newLine )
        {
            _text     = text ;
            _newLine  = newLine ;
            _typeface = new Typeface ( "Courier New" ) ;
            _formattedText = new FormattedText (
                                                _text
                                              , CultureInfo.CurrentCulture
                                              , FlowDirection.LeftToRight
                                              , _typeface
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