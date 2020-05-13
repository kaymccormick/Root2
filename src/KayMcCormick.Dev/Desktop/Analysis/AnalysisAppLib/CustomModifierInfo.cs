#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// CustommodifierInfo.cs
// 
// 2020-04-21-2:46 PM
// 
// ---
#endregion
namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CustomModifierInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsOptional { get ; }

        /// <summary>
        /// 
        /// </summary>
        public string DisplayString { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isOptional"></param>
        /// <param name="displayString"></param>
        public CustomModifierInfo ( bool isOptional , string displayString )
        {
            IsOptional    = isOptional ;
            DisplayString = displayString ;
        }
    }
}