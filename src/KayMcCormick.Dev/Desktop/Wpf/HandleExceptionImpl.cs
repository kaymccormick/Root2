#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisControls
// HandleExceptionImpl.cs
// 
// 2020-03-22-7:19 AM
// 
// ---
#endregion
using System ;
using System.Windows ;

namespace KayMcCormick.Lib.Wpf
{
    public class HandleExceptionImpl : IHandleException
    {
        #region Implementation of IHandleException
        public void HandleException ( Exception exception )
        {
            MessageBox.Show ( exception.ToString ( ) , "Command exception" ) ;
        }
        #endregion
    }
}