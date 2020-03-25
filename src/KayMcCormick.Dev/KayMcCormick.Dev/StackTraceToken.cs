namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public class StackTraceToken
    {
        /// <summary>
        /// 
        /// </summary>
        public int Index { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return $"{{ Index = {Index}, Length = {Length}, Text = {Text} }}" ;
        }
    }
}