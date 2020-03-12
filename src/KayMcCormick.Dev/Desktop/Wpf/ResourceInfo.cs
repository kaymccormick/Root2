﻿using System ;
using System.Windows ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ResourceInfo
    public class ResourceInfo
    {
        private ResourceDictionary _parent ;

        public Type ContainingType { get ; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="object" />
        ///     class.
        /// </summary>
        public ResourceInfo ( Uri source , object key , object value )
        {
            Source = source ;
            Key   = key ;
            Value  = value ;
        }

        public ResourceInfo (
            Uri                resourcesSource
          , object             key
          , object             value
          , Type               containingType
          , ResourceDictionary parent
        ) : this(resourcesSource, key, value)
        {
            ContainingType = containingType ;
            Parent = parent ;
        }

        public ResourceDictionary Parent { get { return _parent ; } set { _parent = value ; } }

        /// <summary>Gets the source.</summary>
        /// <value>The source.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Source
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Uri Source { get ; }

        /// <summary>Gets the key1.</summary>
        /// <value>The key1.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Key1
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public object Key { get ; }

        /// <summary>Gets the value.</summary>
        /// <value>The value.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Value
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public object Value { get ; }
    }
}