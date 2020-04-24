﻿using System ;
using System.Windows ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ResourceInfo
    public sealed class ResourceInfo
    {
        private ResourceDictionary _parent ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="object" />
        ///     class.
        /// </summary>
        public ResourceInfo ( Uri source , object key , object value )
        {
            Source = source ;
            Key    = key ;
            Value  = value ;
        }

        /// <summary>
        /// </summary>
        /// <param name="resourcesSource"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="containingType"></param>
        /// <param name="parent"></param>
        public ResourceInfo (
            Uri                resourcesSource
          , object             key
          , object             value
          , Type               containingType
          , ResourceDictionary parent
        ) : this ( resourcesSource , key , value )
        {
            ContainingType = containingType ;
            Parent         = parent ;
        }

        /// <summary>
        /// </summary>
        public Type ContainingType { get ; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public ResourceDictionary Parent { get { return _parent ; } set { _parent = value ; } }

        /// <summary>Gets the source.</summary>
        /// <value>The source.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Source

        public Uri Source { get ; }

        /// <summary>Gets the key1.</summary>
        /// <value>The key1.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Key1

        public object Key { get ; }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Value

        public object Value { get ; }
    }
}