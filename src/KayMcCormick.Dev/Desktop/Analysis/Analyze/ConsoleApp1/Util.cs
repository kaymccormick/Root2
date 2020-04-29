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
using AnalysisAppLib;
using KayMcCormick.Dev ;

namespace ConsoleApp1
{
    internal static class Util
    {
        public  delegate Task AsyncCommandDelegate(IBaseLibCommand command, AppContext context);
    }
}