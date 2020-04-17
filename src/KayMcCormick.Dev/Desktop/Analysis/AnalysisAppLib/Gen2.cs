using System;
using System.Collections;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace PocoSyntax
{
    /// <summary>
    /// 
    /// </summary>
    public class PocoSyntaxTokenList : IList , IEnumerable , ICollection
    {
        // System.Collections.IList
        /// <inheritdoc />
        public Int32 Add ( Object value ) => _list.Add ( ( PocoSyntaxToken ) value ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public Boolean Contains ( Object value ) => _list.Contains ( ( PocoSyntaxToken ) value ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public void Clear ( ) => _list.Clear ( ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public Int32 IndexOf ( Object value ) => _list.IndexOf ( ( PocoSyntaxToken ) value ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public void Insert ( Int32 index , Object value )
            => _list.Insert ( index , ( PocoSyntaxToken ) value ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public void Remove ( Object value ) => _list.Remove ( ( PocoSyntaxToken ) value ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public void RemoveAt ( Int32 index ) => _list.RemoveAt ( index ) ;

        /// <inheritdoc />
        public Boolean IsReadOnly
        {
            get { return _list.IsReadOnly ; }
        }

        /// <inheritdoc />
        public Boolean IsFixedSize
        {
            get { return _list.IsFixedSize ; }
        }

        /// <inheritdoc />
        public Object this [ Int32 index ]
        {
            get { return _list[ index ] ; }
            set { _list[ index ] = value ; }
        }

        // System.Collections.ICollection
        /// <inheritdoc />
        public void CopyTo ( Array array , Int32 index )
            => _list.CopyTo ( array , index ) ;

        /// <inheritdoc />
        public Int32 Count
        {
            get { return _list.Count ; }
        }

        /// <inheritdoc />
        public Object SyncRoot
        {
            get { return _list.SyncRoot ; }
        }

        /// <inheritdoc />
        public Boolean IsSynchronized
        {
            get { return _list.IsSynchronized ; }
        }

        // System.Collections.IEnumerable
        /// <inheritdoc />
        public IEnumerator GetEnumerator ( ) => _list.GetEnumerator ( ) ;

        private readonly IList _list = new List < PocoSyntaxToken > ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PocoSyntaxToken
    {
        /// <summary>
        /// 
        /// </summary>
        public int RawKind { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public string Kind { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public object Value { get ; set ; }

        /// <summary>
        /// 
        /// </summary>
        public string ValueText { get ; set ; }
    }


}
