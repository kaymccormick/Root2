using System.Collections.Generic ;

namespace KayMcCormick.Dev.StackTrace
{
    /// <summary>
    /// </summary>
    public sealed class StackTraceEntry
    {
        /// <summary>
        /// </summary>
        public StackTraceToken Frame { get ; set ; }

        /// <summary>
        /// </summary>
        public StackTraceToken Type { get ; set ; }

        /// <summary>
        /// </summary>
        public StackTraceToken Method { get ; set ; }

        /// <summary>
        /// </summary>
        public StackTraceToken ParameterList { get ; set ; }

        /// <summary>
        /// </summary>
        public List < StackTraceParameter > Parameters { get ; set ; }

        /// <summary>
        /// </summary>
        public StackTraceToken File { get ; set ; }

        /// <summary>
        /// </summary>
        public StackTraceToken Line { get ; set ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return
                $"{{ Frame = {Frame}, Type = {Type}, Method = {Method}, ParameterList = {ParameterList}, Parameters = {Parameters}, File = {File}, Line = {Line} }}" ;
        }
    }
}