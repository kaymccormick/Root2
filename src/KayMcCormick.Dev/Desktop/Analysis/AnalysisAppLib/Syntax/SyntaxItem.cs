#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// SyntaxItem.cs
// 
// 2020-03-11-7:00 PM
// 
// ---
#endregion
using Microsoft.CodeAnalysis ;
using Microsoft.CodeAnalysis.CSharp ;

namespace AnalysisAppLib.XmlDoc.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SyntaxItem
    {
        private ushort        _rawKind ;
        private SyntaxToken ? _token ;

        /// <summary>
        /// 
        /// </summary>
        public SyntaxKind SyntaxKind { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public SyntaxToken ? Token { get { return _token ; } set { _token = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public ushort RawKind { get { return _rawKind ; } set { _rawKind = value ; } }
    }
}