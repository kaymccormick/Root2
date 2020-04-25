namespace KayMcCormick.Dev.StackTrace
{
    /// <summary>
    /// </summary>
    public sealed class StackTraceToken
    {
        /// <summary>
        /// </summary>
        internal int Index { get ; set ; }

        /// <summary>
        /// </summary>
        internal int Length { get ; set ; }

        /// <summary>
        /// </summary>
        public string Text { get ; set ; }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return $"{{ Index = {Index}, Length = {Length}, Text = {Text} }}" ;
        }
    }
}