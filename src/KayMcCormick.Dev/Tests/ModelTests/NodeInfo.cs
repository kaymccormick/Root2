#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ModelTests
// NodeInfo.cs
// 
// 2020-04-25-12:56 AM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;

namespace ModelTests
{
    public class NodeInfo
    {
        private SyntaxNode _node ;
        public  SyntaxNode Node { get { return _node ; } set { _node = value ; } }
        #region Overrides of Object
        public override string ToString ( )
        {
            return Node?.Kind ( ).ToString ( ) ?? "";
        }
        #endregion
    }
}