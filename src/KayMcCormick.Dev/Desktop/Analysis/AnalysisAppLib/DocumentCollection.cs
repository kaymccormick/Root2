using System ;
using System.Collections ;

namespace AnalysisAppLib
{
    /// <summary>
    /// Collection of documents for XAML serialization purposes.
    /// </summary>
    public sealed class DocumentCollection : IList , ICollection , IEnumerable

    {
        private readonly IList _list ;

        /// <summary>
        /// </summary>
        /// <param name="list"></param>
        // ReSharper disable once UnusedMember.Global
        public DocumentCollection ( IList list ) { _list = list ; }

        /// <summary>
        /// </summary>
        public DocumentCollection ( ) { _list = new ArrayList ( ) ; }

        #region Implementation of IEnumerable
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator ( ) { return _list.GetEnumerator ( ) ; }
        #endregion
        #region Implementation of ICollection
        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo ( Array array , int index ) { _list.CopyTo ( array , index ) ; }

        /// <summary>
        /// </summary>
        public int Count { get { return _list.Count ; } }

        /// <summary>
        /// </summary>
        public object SyncRoot { get { return _list.SyncRoot ; } }

        /// <summary>
        /// </summary>
        public bool IsSynchronized { get { return _list.IsSynchronized ; } }
        #endregion
        #region Implementation of IList
        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Add ( object value ) { return _list.Add ( value ) ; }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains ( object value ) { return _list.Contains ( value ) ; }

        /// <summary>
        /// </summary>
        public void Clear ( ) { _list.Clear ( ) ; }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int IndexOf ( object value ) { return _list.IndexOf ( value ) ; }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void Insert ( int index , object value ) { _list.Insert ( index , value ) ; }

        /// <summary>
        /// </summary>
        /// <param name="value"></param>
        public void Remove ( object value ) { _list.Remove ( value ) ; }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt ( int index ) { _list.RemoveAt ( index ) ; }

        /// <summary>
        /// </summary>
        /// <param name="index"></param>
        public object this [ int index ]
        {
            get { return _list[ index ] ; }
            set { _list[ index ] = value ; }
        }

        /// <summary>
        /// </summary>
        public bool IsReadOnly { get { return _list.IsReadOnly ; } }

        /// <summary>
        /// </summary>
        public bool IsFixedSize { get { return _list.IsFixedSize ; } }
        #endregion
    }
}