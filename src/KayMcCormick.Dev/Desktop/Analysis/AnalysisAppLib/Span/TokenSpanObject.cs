﻿#region header
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

namespace AnalysisAppLib.Span
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class TokenSpanObject : SpanObject < SyntaxToken >
    {
        // ReSharper disable once NotAccessedField.Local
        private readonly string _instanceRawKind ;

        // ReSharper disable once NotAccessedField.Local
        private readonly string _instanceValueText ;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="span"></param>
        /// <param name="instance"></param>
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
                panel.Children.Add ( new Label ( ) { Content = "Value" } ) ;
                panel.Children.Add ( new TextBlock ( ) { Text = _instanceValueText } ) ;
            }

            return panel ;
        }
#endif
    }
}