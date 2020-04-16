﻿using System;
using System.Collections;
using System.Collections.Generic;

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
            => _list.Insert ( ( Int32 ) index , ( PocoSyntaxToken ) value ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public void Remove ( Object value ) => _list.Remove ( ( PocoSyntaxToken ) value ) ;

        // System.Collections.IList
        /// <inheritdoc />
        public void RemoveAt ( Int32 index ) => _list.RemoveAt ( ( Int32 ) index ) ;

        /// <inheritdoc />
        public Boolean IsReadOnly => _list.IsReadOnly ;

        /// <inheritdoc />
        public Boolean IsFixedSize => _list.IsFixedSize ;

        /// <inheritdoc />
        public Object this [ Int32 index ]
        {
            get => _list[ index ] ;
            set => _list[ index ] = value ;
        }

        // System.Collections.ICollection
        /// <inheritdoc />
        public void CopyTo ( Array array , Int32 index )
            => _list.CopyTo ( ( Array ) array , ( Int32 ) index ) ;

        /// <inheritdoc />
        public Int32 Count => _list.Count ;

        /// <inheritdoc />
        public Object SyncRoot => _list.SyncRoot ;

        /// <inheritdoc />
        public Boolean IsSynchronized => _list.IsSynchronized ;

        // System.Collections.IEnumerable
        /// <inheritdoc />
        public IEnumerator GetEnumerator ( ) => _list.GetEnumerator ( ) ;

        /// <inheritdoc />
        IList _list = new List < PocoSyntaxToken > ( ) ;
    }

    /// <summary>
    /// 
    /// </summary>
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
