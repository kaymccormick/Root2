#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// SpanObject.cs
// 
// 2020-02-26-10:00 PM
// 
// ---
#endregion
using System ;
using System.Windows ;
using System.Windows.Controls ;
using Microsoft.CodeAnalysis.Text ;

namespace ProjLib
{
    public class SpanObject < T > : ISpanObject
    {
        public  T                                     _instance ;
        private Func < SpanObject < T > , UIElement > _getTooltipContent ;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public SpanObject (
            TextSpan                              span
          , T                                     instance
          , Func < SpanObject < T > , UIElement > getTooltipContent = null
        )
        {
            Span               = span ;
            _instance          = instance ;
            _getTooltipContent = getTooltipContent ;
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>

        public T Instance { get => _instance ; set => _instance = value ; }

        public object getInstance ( ) { return Instance ; }

        public TextSpan Span { get ; set ; }

        public virtual UIElement GetToolTipContent ( )
        {
            return _getTooltipContent?.Invoke ( this ) ?? new Grid ( ) ;
        }
    }
}