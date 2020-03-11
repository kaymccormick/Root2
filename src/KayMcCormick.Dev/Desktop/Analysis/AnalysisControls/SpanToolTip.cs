#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// SpanToolTip.cs
// 
// 2020-02-26-10:46 PM
// 
// ---
#endregion
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Media ;
using ProjLib ;
using ProjLib.Interfaces ;

namespace AnalysisControls
{
    internal class SpanTT : ToolTip
    {
        public SpanTT ( SpanToolTip content ) { Content = CustomToolTip = content ; }

        public SpanToolTip CustomToolTip { get ; set ; }

        public ISpanToolTipViewModel ViewModel { get ; set ; }
    }

    public partial class SpanToolTip : UserControl , ISpanToolTip
    {
        public StackPanel Panel { get ; set ; }

        public SpanToolTip ( )
        {
            InitializeComponent ( ) ;
            Content = Panel = new StackPanel ( ) { Orientation = Orientation.Vertical , } ;
            // Panel.SetValue ( TextElement.FontSizeProperty , 24.0f ) ;
        }

        public void Add ( UIElement o )
        {
            if ( o != null )
            {
                o.SetValue ( BorderBrushProperty , Brushes.Black ) ;

                o.SetValue ( BorderThicknessProperty , new Thickness ( 0 , 0 , 0 , 3 ) ) ;
                Panel.Children.Add ( o ) ;
                // var element = new Line ( ) ;
                // element.VerticalAlignment   = VerticalAlignment.Stretch ;
                // element.HorizontalAlignment = HorizontalAlignment.Center ;
                // element.Stroke              = Brushes.Black ;
                // element.Height              = 5 ;
                // element.SetBinding ( WidthProperty , new Binding ( "Width" ) { Source = Panel } ) ;
                // Panel.Children.Add ( element ) ;
            }
        }
    }
}