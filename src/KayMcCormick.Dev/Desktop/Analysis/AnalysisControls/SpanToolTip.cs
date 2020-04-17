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
using AnalysisAppLib.ViewModel ;
using AnalysisControls.Interfaces ;
using JetBrains.Annotations ;

namespace AnalysisControls
{
    // ReSharper disable once ClassNeverInstantiated.Global
    internal class SpanTT : ToolTip
    {
        public SpanTT ( SpanToolTip content ) { Content = CustomToolTip = content ; }

        public SpanToolTip CustomToolTip { get ; }

        public ISpanToolTipViewModel ViewModel { get ; set ; }
    }

    /// <summary>
    /// </summary>
    public sealed partial class SpanToolTip : UserControl , ISpanToolTip
    {
        /// <summary>
        /// </summary>
        public SpanToolTip ( )
        {
            InitializeComponent ( ) ;
            Content = Panel = new StackPanel { Orientation = Orientation.Vertical } ;
            // Panel.SetValue ( TextElement.FontSizeProperty , 24.0f ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        public StackPanel Panel { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        // ReSharper disable once UnusedMember.Global
        public void Add ( [ CanBeNull ] UIElement o )
        {
            if ( o != null )
            {
                o.SetCurrentValue ( BorderBrushProperty , Brushes.Black ) ;

                o.SetCurrentValue ( BorderThicknessProperty , new Thickness ( 0 , 0 , 0 , 3 ) ) ;
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