#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// Util.cs
// 
// 2020-04-08-6:16 AM
// 
// ---
#endregion
using System.Threading.Tasks ;
using KayMcCormick.Lib.Wpf.Command ;

namespace ConsoleApp1
{
    internal class Util
    {
        public  delegate Task AsyncCommandDelegate(IAppCommand command, AppContext context);
    }
}