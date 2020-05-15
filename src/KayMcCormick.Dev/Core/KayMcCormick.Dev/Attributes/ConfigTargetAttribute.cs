﻿#region header
// Kay McCormick (mccor)
// 
// WpfApp
// WpfApp
// ConfigTargetAttribute.cs
// 
// 2020-02-05-4:04 PM
// 
// ---
#endregion
using System ;

namespace KayMcCormick.Dev.Attributes
{
    /// <summary></summary>
    /// <seealso cref="System.Attribute" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for ConfigTargetAttribute
    [ AttributeUsage ( AttributeTargets.Class ) ]
    public sealed class ConfigTargetAttribute : Attribute
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <see cref="ConfigTargetAttribute" /> class.
        /// </summary>
        /// <param name="targetType"></param>
        public ConfigTargetAttribute ( Type targetType ) { TargetType = targetType ; }

        /// <summary>Gets or sets the type of the target.</summary>
        /// <value>The type of the target.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TargetType
        public Type TargetType { get ; }
    }
}