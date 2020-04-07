#region header
// Kay McCormick (mccor)
// 
// Proj
// AnalysisControls
// Converter1Param.cs
// 
// 2020-03-03-7:22 PM
// 
// ---
#endregion
namespace AnalysisControls.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public enum SyntaxNodeInfo
    {
        /// <summary>The ancestors</summary>
        Ancestors
      , AncestorsAndSelf
      , GetFirstToken
      , GetLocation
      , GetLastToken
      , GetReference
      , GetText
      , ToFullString
      , ToString
      , Kind
      , ChildNodesAndTokens
      , ChildNodes
      , ChildTokens
      , DescendantNodes
      , DescendantNodesAndSelf
      , DescendantNodesAndTokens
      , DescendantNodesAndTokensAndSelf
      , DescendantTokens
      , DescendantTrivia
      , GetLeadingTrivia
      , Diagnostics
    }
}