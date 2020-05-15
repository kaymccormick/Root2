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

namespace AnalysisAppLib.Span
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SpanObject < T > : ISpanViewModel , ISpanObject < T >
    {
        private T _instance ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="span"></param>
        /// <param name="instance"></param>
        protected SpanObject ( TextSpan span , T instance )
        {
            Span      = span ;
            _instance = instance ;
        }


        /// <summary>
        /// 
        /// </summary>
        public T Instance { get { return _instance ; } set { _instance = value ; } }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetInstance ( ) { return Instance ; }

        /// <summary>
        /// 
        /// </summary>
        public TextSpan Span { get ; }
    }
}