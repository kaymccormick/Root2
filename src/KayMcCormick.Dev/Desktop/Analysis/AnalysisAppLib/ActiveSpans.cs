#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// ActionSpans.cs
// 
// 2020-02-27-12:51 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using JetBrains.Annotations ;
using Microsoft.CodeAnalysis.Text ;

namespace AnalysisAppLib
{
    public class ActiveSpans : IDictionary < object , IDictionary < object , ISpanViewModel > >
    {
        private readonly IDictionary < TextSpan , IDictionary < object , ISpanViewModel > > _dict2 =
            new Dictionary < TextSpan , IDictionary < object , ISpanViewModel > > ( ) ;

        private readonly IDictionary < object , IDictionary < object , ISpanViewModel > >
            _dictionaryImplementation =
                new Dictionary < object , IDictionary < object , ISpanViewModel > > ( ) ;

        public void RemoveAll ( [ NotNull ] object node )
        {
            if ( node == null )
            {
                throw new ArgumentNullException ( nameof ( node ) ) ;
            }

            Remove ( node ) ;
        }
        #region Implementation of IEnumerable
        public IEnumerator < KeyValuePair < object , IDictionary < object , ISpanViewModel > > >
            GetEnumerator ( )
        {
            return _dictionaryImplementation.GetEnumerator ( ) ;
        }

        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return ( ( IEnumerable ) _dictionaryImplementation ).GetEnumerator ( ) ;
        }
        #endregion
        #region Implementation of ICollection<KeyValuePair<object,IDictionary<object,ISpanViewModel>>>
        public void Add ( KeyValuePair < object , IDictionary < object , ISpanViewModel > > item )
        {
            _dictionaryImplementation.Add ( item ) ;
        }

        public void Clear ( ) { _dictionaryImplementation.Clear ( ) ; }

        public bool Contains (
            KeyValuePair < object , IDictionary < object , ISpanViewModel > > item
        )
        {
            return _dictionaryImplementation.Contains ( item ) ;
        }

        public void CopyTo (
            KeyValuePair < object , IDictionary < object , ISpanViewModel > >[] array
          , int                                                                 arrayIndex
        )
        {
            _dictionaryImplementation.CopyTo ( array , arrayIndex ) ;
        }

        public bool Remove (
            KeyValuePair < object , IDictionary < object , ISpanViewModel > > item
        )
        {
            
            return _dictionaryImplementation.Remove ( item ) ;
        }

        public int Count { get { return _dictionaryImplementation.Count ; } }

        public bool IsReadOnly { get { return _dictionaryImplementation.IsReadOnly ; } }
        #endregion
        #region Implementation of IDictionary<object,IDictionary<object,ISpanViewModel>>
        public bool ContainsKey ( object key )
        {
            return _dictionaryImplementation.ContainsKey ( key ) ;
        }

        public void Add ( object key , IDictionary < object , ISpanViewModel > value )
        {
            _dictionaryImplementation.Add ( key , value ) ;
        }

        public bool Remove ( object key ) { return _dictionaryImplementation.Remove ( key ) ; }

        public bool TryGetValue ( object key , out IDictionary < object , ISpanViewModel > value )
        {
            return _dictionaryImplementation.TryGetValue ( key , out value ) ;
        }

        public IDictionary < object , ISpanViewModel > this [ object key ]
        {
            get { return _dictionaryImplementation[ key ] ; }
            set { _dictionaryImplementation[ key ] = value ; }
        }

        public ICollection < object > Keys { get { return _dictionaryImplementation.Keys ; } }

        public ICollection < IDictionary < object , ISpanViewModel > > Values
        {
            get { return _dictionaryImplementation.Values ; }
        }
        #endregion

        public void AddSpan ( [ NotNull ] object node , TextSpan key , ISpanViewModel spanObject )
        {
            if ( ! TryGetValue ( node , out var x ) )
            {
                this[ node ] = x = new Dictionary < object , ISpanViewModel > ( ) ;
            }

            if ( x != null )
            {
                x[ key ] = spanObject ;
            }

            if ( ! _dict2.TryGetValue ( key , out var xx ) )
            {
                _dict2[ key ] = xx = new Dictionary < object , ISpanViewModel > ( ) ;
            }

            xx[ node ] = spanObject ;
        }

        [ NotNull ]
        public IEnumerable < object > OverlapsWith ( TextSpan span )
        {
            return _dict2.Where ( ( pair , i ) => pair.Key.OverlapsWith ( span ) )
                         .SelectMany ( pair => pair.Value.Values ) ;
        }
    }
}