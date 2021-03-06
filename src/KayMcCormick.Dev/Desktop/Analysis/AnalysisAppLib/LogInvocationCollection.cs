﻿#region header
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
#if FINDLOGUSAGES
using System ;
using System.Collections.ObjectModel ;
using FindLogUsages ;
using JetBrains.Annotations ;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogInvocationCollection : ObservableCollection < ILogInvocation >
    {
        #region Overrides of ObservableCollection<ILogInvocation>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException"></exception>
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void InsertItem ( int index , [ JetBrains.Annotations.NotNull ] ILogInvocation item )
        {
            if ( item == null )
            {
                throw new ArgumentNullException ( nameof ( item ) ) ;
            }

            base.InsertItem ( index , item ) ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException"></exception>
        // ReSharper disable once AnnotationConflictInHierarchy
        protected override void SetItem ( int index , [ JetBrains.Annotations.NotNull ] ILogInvocation item )
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
#endif