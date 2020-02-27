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
using System.Windows.Documents ;
using System.Windows.Media ;

namespace ProjLib
{
    public partial class SpanToolTip : UserControl , ISpanToolTip
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Windows.Controls.UserControl" /> class.</summary>
        ///
        public StackPanel Panel { get ; set ; }
        
        public SpanToolTip ( )
        {
            InitializeComponent();
            Content = Panel = new StackPanel ( )
                              { 
                                  Orientation = Orientation.Vertical ,
                              } ;
            // Panel.SetValue ( TextElement.FontSizeProperty , 24.0f ) ;
        }

        public void Add ( UIElement o )
        {
            if ( o != null )
            {
                o.SetValue ( BorderBrushProperty , Brushes.Black ) ;

                o.SetValue ( BorderThicknessProperty , new Thickness ( 0, 0, 0, 3 ) ) ;
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