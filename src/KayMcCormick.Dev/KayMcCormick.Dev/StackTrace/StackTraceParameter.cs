namespace KayMcCormick.Dev.StackTrace
{
    /// <summary>
    /// </summary>
    public sealed class StackTraceParameter
    {
        /// <summary>
        /// </summary>
        public StackTraceToken Type { get ; set ; }

        /// <summary>
        /// </summary>
        public StackTraceToken Name { get ; set ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( ) { return $"{{ Type = {Type}, Name = {Name} }}" ; }
    }
}