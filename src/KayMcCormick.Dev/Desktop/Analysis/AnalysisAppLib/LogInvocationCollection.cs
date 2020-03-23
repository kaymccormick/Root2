#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// LogInvocationCollection.cs
// 
// 2020-03-17-1:36 PM
// 
// ---
#endregion
using System ;
using System.Collections.ObjectModel ;
using JetBrains.Annotations ;

namespace AnalysisAppLib
{
    public class LogInvocationCollection : ObservableCollection<ILogInvocation>
    {
        #region Overrides of ObservableCollection<ILogInvocation>
        protected override void InsertItem ( int index , [ NotNull ] ILogInvocation item )
        {
            if ( item == null )
            {
                throw new ArgumentNullException ( nameof ( item ) ) ;
            }

            base.InsertItem ( index , item ) ;
        }

        protected override void SetItem ( int index , [ NotNull ] ILogInvocation item )
        {
            if ( item == null )
            {
                throw new ArgumentNullException ( nameof ( item ) ) ;
            }

            base.SetItem ( index , item ) ;
        }
        #endregion
    }
}