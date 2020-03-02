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
    public class Token : UIElement
    {
        private readonly string   _text ;
        private readonly bool _newLine ;
        private          Typeface _typeface ;
        private FormattedText _formattedText ;
        private Size _desiredSize ;

        public Token ( int rawKind , string text , SolidColorBrush solidColorBrush , bool newLine )
        {
            _text     = text ;
            _newLine = newLine ;
            _typeface = new Typeface ( "Courier New" ) ;
            _formattedText = new FormattedText (
                                                _text
                                              , CultureInfo.CurrentCulture
                                              , FlowDirection.LeftToRight
                                              , _typeface
                                              , 20
                                              , solidColorBrush ?? Brushes.Black) ;
            _desiredSize = new Size(_formattedText.Width, _formattedText.Height);
        }


        protected override Size MeasureCore ( Size availableSize )
        {
            return _desiredSize ;
        }

        #region Overrides of UIElement
        protected override void OnRender ( DrawingContext drawingContext )
        {
            base.OnRender ( drawingContext ) ;
            drawingContext.DrawText ( _formattedText, new Point(0, 0));

                                                         
                                                        
        }
        #endregion
    }
}