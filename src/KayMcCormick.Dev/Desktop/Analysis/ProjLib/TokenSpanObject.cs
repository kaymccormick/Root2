#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// TokenSpanObject.cs
// 
// 2020-02-26-11:09 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;
using Microsoft.CodeAnalysis.Text ;

namespace ProjLib
{
    public class TokenSpanObject : SpanObject < SyntaxToken >
    {
        
        private readonly string _instanceRawKind ;
        
        private          readonly string _instanceValueText ;

        
        public TokenSpanObject ( TextSpan span , SyntaxToken instance ) : base ( span , instance )
        {
            _instanceRawKind   = Instance.Kind ( ) + " (" + Instance.RawKind + ")" ;
            _instanceValueText = Instance.ValueText ;
        }

#if false
        public override UIElement GetToolTipContent ( )
        {
            var panel = new StackPanel ( ) { Orientation = Orientation.Horizontal } ;
            var toolTipContent = new TextBlock ( )
                                 {
                                     Text = _instanceRawKind
                                 } ;
            panel.Children.Add ( toolTipContent ) ;
            if ( Instance.Value != null )
            {
                panel.Children.Add ( new Label ( ) { Content  = "Value" } ) ;
                panel.Children.Add ( new TextBlock ( ) { Text = _instanceValueText } ) ;
            }

            return panel ;
        }
#endif
    }
}