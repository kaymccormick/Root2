#region header
// Kay McCormick (mccor)
// 
// Analysis
// ConsoleApp1
// ExampleDict.cs
// 
// 2020-04-08-5:31 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using Microsoft.CodeAnalysis.CSharp ;

namespace ConsoleApp1
{
    public sealed class ExampleDict : IDictionary , ICollection , IEnumerable
    {
        private readonly IDictionary _dictionaryImplementation =
            new Dictionary < SyntaxKind , ArrayList > ( ) ;

        #region Implementation of IEnumerable
        public bool Contains ( object key ) { return _dictionaryImplementation.Contains ( key ) ; }

        public void Add ( object key , object value )
        {
            _dictionaryImplementation.Add ( key , value ) ;
        }

        public void Clear ( ) { _dictionaryImplementation.Clear ( ) ; }

        public IDictionaryEnumerator GetEnumerator ( )
        {
            return _dictionaryImplementation.GetEnumerator ( ) ;
        }

        public void Remove ( object key ) { _dictionaryImplementation.Remove ( key ) ; }

        public object this [ object key ]
        {
            get { return _dictionaryImplementation[ key ] ; }
            set { _dictionaryImplementation[ key ] = value ; }
        }

        public ICollection Keys { get { return _dictionaryImplementation.Keys ; } }

        public ICollection Values { get { return _dictionaryImplementation.Values ; } }

        public bool IsReadOnly { get { return _dictionaryImplementation.IsReadOnly ; } }

        public bool IsFixedSize { get { return _dictionaryImplementation.IsFixedSize ; } }

        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return ( ( IEnumerable ) _dictionaryImplementation ).GetEnumerator ( ) ;
        }
        #endregion
        #region Implementation of ICollection
        public void CopyTo ( Array array , int index )
        {
            _dictionaryImplementation.CopyTo ( array , index ) ;
        }

        public int Count { get { return _dictionaryImplementation.Count ; } }

        public object SyncRoot { get { return _dictionaryImplementation.SyncRoot ; } }

        public bool IsSynchronized { get { return _dictionaryImplementation.IsSynchronized ; } }
        #endregion
    }
}