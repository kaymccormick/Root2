#if TERMUI
#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ListView2.cs
// 
// 2020-04-03-5:48 AM
// 
// ---
#endregion
using System.Collections.Generic ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Terminal.Gui ;

namespace ConsoleApp1
{
    internal sealed class ListView2 : ListView2Base < ResourceNodeInfo >
    {
        public ListView2 ( Rect rect , [ NotNull ] List < ReEsourceNodeInfo > list ) :
            base ( rect , list )
        {
        }
    }
}
#endif