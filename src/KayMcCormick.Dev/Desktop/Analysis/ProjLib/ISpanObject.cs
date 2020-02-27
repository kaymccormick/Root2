#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ISpanObject.cs
// 
// 2020-02-26-10:01 PM
// 
// ---
#endregion
using System.Windows ;
using Microsoft.CodeAnalysis.Text ;

namespace ProjLib
{
    public interface ISpanViewModel
    {
        object getInstance();
        TextSpan Span { get; }

    }
}