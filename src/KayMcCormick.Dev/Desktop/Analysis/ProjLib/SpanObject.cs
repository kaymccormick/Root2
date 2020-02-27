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
    public class SpanObject < T > : ISpanViewModel, ISpanObject <T>
    {
        public  T                                     _instance ;
        public SpanObject (
            TextSpan                              span
          , T                                     instance
        )
        {
            Span               = span ;
            _instance          = instance ;
            
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>

        public T Instance { get => _instance ; set => _instance = value ; }

        public object getInstance ( ) { return Instance ; }

        public TextSpan Span { get ; set ; }
    }
}