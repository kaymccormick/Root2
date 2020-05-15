#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// FindLogUsages.cs
// 
// 2020-03-25-1:52 AM
// 
// ---
#endregion
using System ;

namespace FindLogUsages
{
    public sealed class EmptyDisposable : IDisposable
    {
        #region IDisposable
        public void Dispose ( ) { }
        #endregion
    }
}