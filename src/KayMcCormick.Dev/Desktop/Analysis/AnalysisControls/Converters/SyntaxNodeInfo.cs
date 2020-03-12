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
    public enum SyntaxNodeInfo
    {
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