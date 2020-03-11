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
using Microsoft.CodeAnalysis.Text ;
using ProjLib.Interfaces ;

namespace ProjLib
{
    public class SpanObject < T > : ISpanViewModel, ISpanObject <T>
    {
        private T                                     _instance ;
        public SpanObject (
            TextSpan                              span
          , T                                     instance
        )
        {
            Span               = span ;
            _instance          = instance ;
            
        }

        

        public T Instance { get => _instance ; set => _instance = value ; }

        public object getInstance ( ) { return Instance ; }

        public TextSpan Span { get ; set ; }
    }
}