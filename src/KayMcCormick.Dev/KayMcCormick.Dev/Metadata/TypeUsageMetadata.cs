#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Dev
// TypeUsageMetadata.cs
// 
// 2020-04-25-12:36 PM
// 
// ---
#endregion
namespace KayMcCormick.Dev.Metadata
{
    public class TypeUsageMetadata
    {
        /// <summary>Gets or sets a value indicating whether [UI conversion].</summary>
        /// <value>
        ///   <c>true</c> if [UI conversion]; otherwise, <c>false</c>.</value>
        public bool UiConversion { get ; set ; }

        /// <summary>Gets or sets a value indicating whether this instance has standard values.</summary>
        /// <value>
        ///   <c>true</c> if this instance has standard values; otherwise, <c>false</c>.</value>
        public bool hasStandardValues { get ; set ; }
    }
}
