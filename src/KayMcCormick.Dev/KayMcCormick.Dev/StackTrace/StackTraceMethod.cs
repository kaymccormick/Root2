namespace KayMcCormick.Dev.StackTrace
{
    /// <summary>
    /// </summary>
    public sealed class StackTraceMethod
    {
        /// <summary>
        /// </summary>
        public StackTraceToken Type { get ; set ; }

        /// <summary>
        /// </summary>
        public StackTraceToken Method { get ; set ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( ) { return $"{{ Type = {Type}, Method = {Method} }}" ; }
    }
}