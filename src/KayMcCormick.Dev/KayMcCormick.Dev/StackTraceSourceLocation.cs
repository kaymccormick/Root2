namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class StackTraceSourceLocation
    {
        /// <summary>
        /// 
        /// </summary>
        public StackTraceToken File { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public StackTraceToken Line { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString ( ) { return $"{{ File = {File}, Line = {Line} }}" ; }
    }
}