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

namespace AnalysisAppLib.Span
{
    /// <summary>
    /// </summary>
    public sealed class
        ActiveSpans : IDictionary < object , IDictionary < object , ISpanViewModel > >
    {
        private readonly IDictionary < TextSpan , IDictionary < object , ISpanViewModel > > _dict2 =
            new Dictionary < TextSpan , IDictionary < object , ISpanViewModel > > ( ) ;

        private readonly IDictionary < object , IDictionary < object , ISpanViewModel > >
            _dictionaryImplementation =
                new Dictionary < object , IDictionary < object , ISpanViewModel > > ( ) ;

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void RemoveAll ( [ JetBrains.Annotations.NotNull ] object node )
        {
            if ( node == null )
            {
                throw new ArgumentNullException ( nameof ( node ) ) ;
            }

            Remove ( node ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="node"></param>
        /// <param name="key"></param>
        /// <param name="spanObject"></param>
        public void AddSpan ( [ JetBrains.Annotations.NotNull ] object node , TextSpan key , ISpanViewModel spanObject )
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

        /// <summary>
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public IEnumerable < object > OverlapsWith ( TextSpan span )
        {
            return _dict2.Where ( ( pair , i ) => pair.Key.OverlapsWith ( span ) )
                         .SelectMany ( pair => pair.Value.Values ) ;
        }

        #region Implementation of IEnumerable
        /// <summary>
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        public void Add ( KeyValuePair < object , IDictionary < object , ISpanViewModel > > item )
        {
            _dictionaryImplementation.Add ( item ) ;
        }

        /// <summary>
        /// </summary>
        public void Clear ( ) { _dictionaryImplementation.Clear ( ) ; }

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains (
            KeyValuePair < object , IDictionary < object , ISpanViewModel > > item
        )
        {
            return _dictionaryImplementation.Contains ( item ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo (
            KeyValuePair < object , IDictionary < object , ISpanViewModel > >[] array
          , int                                                                 arrayIndex
        )
        {
            _dictionaryImplementation.CopyTo ( array , arrayIndex ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove (
            KeyValuePair < object , IDictionary < object , ISpanViewModel > > item
        )
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return _dictionaryImplementation.Remove ( item ) ;
        }

        /// <summary>
        /// </summary>
        public int Count { get { return _dictionaryImplementation.Count ; } }

        /// <summary>
        /// </summary>
        public bool IsReadOnly { get { return _dictionaryImplementation.IsReadOnly ; } }
        #endregion
        #region Implementation of IDictionary<object,IDictionary<object,ISpanViewModel>>
        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey ( object key )
        {
            return _dictionaryImplementation.ContainsKey ( key ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add ( object key , IDictionary < object , ISpanViewModel > value )
        {
            _dictionaryImplementation.Add ( key , value ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove ( object key ) { return _dictionaryImplementation.Remove ( key ) ; }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue ( object key , out IDictionary < object , ISpanViewModel > value )
        {
            return _dictionaryImplementation.TryGetValue ( key , out value ) ;
        }

        /// <summary>
        /// </summary>
        /// <param name="key"></param>
        public IDictionary < object , ISpanViewModel > this [ object key ]
        {
            get { return _dictionaryImplementation[ key ] ; }
            set { _dictionaryImplementation[ key ] = value ; }
        }

        /// <summary>
        /// </summary>
        public ICollection < object > Keys { get { return _dictionaryImplementation.Keys ; } }

        /// <summary>
        /// </summary>
        public ICollection < IDictionary < object , ISpanViewModel > > Values
        {
            get { return _dictionaryImplementation.Values ; }
        }
        #endregion
    }
}