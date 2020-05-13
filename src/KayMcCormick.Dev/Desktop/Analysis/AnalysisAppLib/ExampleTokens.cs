#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ExampleTokens.cs
// 
// 2020-04-08-5:31 AM
// 
// ---
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ExampleTokens : IList , ICollection , IEnumerable
    {
        private readonly IList _listImplementation = new List < SToken > ( ) ;
        #region Implementation of IEnumerable

        /// <inheritdoc />
        public IEnumerator GetEnumerator ( ) { return _listImplementation.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection

        /// <inheritdoc />
        public void CopyTo ( Array array , int index )
        {
            _listImplementation.CopyTo ( array , index ) ;
        }

        public int Count { get { return _listImplementation.Count ; } }

        public object SyncRoot { get { return _listImplementation.SyncRoot ; } }

        public bool IsSynchronized { get { return _listImplementation.IsSynchronized ; } }
        #endregion
        #region Implementation of IList
        public int Add ( object value ) { return _listImplementation.Add ( value ) ; }

        public bool Contains ( object value ) { return _listImplementation.Contains ( value ) ; }

        public void Clear ( ) { _listImplementation.Clear ( ) ; }

        public int IndexOf ( object value ) { return _listImplementation.IndexOf ( value ) ; }

        public void Insert ( int index , object value )
        {
            _listImplementation.Insert ( index , value ) ;
        }

        public void Remove ( object value ) { _listImplementation.Remove ( value ) ; }

        public void RemoveAt ( int index ) { _listImplementation.RemoveAt ( index ) ; }

        public object this [ int index ]
        {
            get { return _listImplementation[ index ] ; }
            set { _listImplementation[ index ] = value ; }
        }

        public bool IsReadOnly { get { return _listImplementation.IsReadOnly ; } }

        public bool IsFixedSize { get { return _listImplementation.IsFixedSize ; } }
        #endregion
    }
}