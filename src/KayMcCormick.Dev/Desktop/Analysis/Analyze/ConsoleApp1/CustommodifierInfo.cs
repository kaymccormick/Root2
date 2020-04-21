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
namespace ConsoleApp1
{
    public sealed class CustomModifierInfo
    {
        public bool IsOptional { get ; }

        public string DisplayString { get ; }

        public CustomModifierInfo ( bool isOptional , string displayString )
        {
            IsOptional    = isOptional ;
            DisplayString = displayString ;
        }
    }
}