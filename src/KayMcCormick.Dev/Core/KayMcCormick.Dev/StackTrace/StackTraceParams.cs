using System.Collections.Generic ;

namespace KayMcCormick.Dev.StackTrace
{
    /// <summary>
    /// </summary>
    public class StackTraceParams
    {
        /// <summary>
        /// </summary>
        public StackTraceToken List { get ; set ; }

        /// <summary>
        /// </summary>
        public IEnumerable < StackTraceParameter > Parameters { get ; set ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return $"{{ List = {List}, Parameters = {Parameters} }}" ;
        }
    }
}