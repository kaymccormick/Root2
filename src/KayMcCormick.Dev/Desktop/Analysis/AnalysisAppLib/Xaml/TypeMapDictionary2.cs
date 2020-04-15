#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// TypeMapDictionary2.cs
// 
// 2020-04-15-5:46 AM
// 
// ---
#endregion
using System ;
using System.Collections ;
using System.Collections.Generic ;
using AnalysisAppLib.Syntax ;
using JetBrains.Annotations ;

namespace AnalysisAppLib.Xaml
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TypeMapDictionary2 : IDictionary , ICollection , IEnumerable
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly Dictionary < string , AppTypeInfo > dict = new Dictionary < string, AppTypeInfo > ( ) ;

        private readonly IDictionary _dict ;
        /// <summary>
        /// 
        /// </summary>
        public TypeMapDictionary2 ( ) { _dict = dict ; }
        #region Implementation of IEnumerable
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Contains ( object key ) { return _dict.Contains ( key ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add ( object key , object value ) { _dict.Add ( key , value ) ; }

        /// <summary>
        /// 
        /// </summary>
        public void Clear ( ) { _dict.Clear ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDictionaryEnumerator GetEnumerator ( ) { return _dict.GetEnumerator ( ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Remove ( object key ) { _dict.Remove ( key ) ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public object this [ object key ]
        {
            get { return _dict[ key ] ; }
            set { _dict[ key ] = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection Keys { get { return _dict.Keys ; } }

        /// <summary>
        /// 
        /// </summary>
        public ICollection Values { get { return _dict.Values ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly { get { return _dict.IsReadOnly ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFixedSize { get { return _dict.IsFixedSize ; } }

        IEnumerator IEnumerable.GetEnumerator ( )
        {
            return ( ( IEnumerable ) _dict ).GetEnumerator ( ) ;
        }
        #endregion
        #region Implementation of ICollection
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo ( Array array , int index ) { _dict.CopyTo ( array , index ) ; }

        /// <summary>
        /// 
        /// </summary>
        public int Count { get { return _dict.Count ; } }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot { get { return _dict.SyncRoot ; } }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSynchronized { get { return _dict.IsSynchronized ; } }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeKey"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public AppTypeInfo GetAppTypeInfoForType ( [ NotNull ] AppTypeInfoKey typeKey )
        {
            if (! dict.ContainsKey ( typeKey.StringValue ) )
            {
                throw new InvalidOperationException ( "No such type" ) ;
            }

            return dict[ typeKey.StringValue ] ;
        }
    }
}