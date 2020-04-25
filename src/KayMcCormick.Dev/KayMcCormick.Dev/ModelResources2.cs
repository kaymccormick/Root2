#region header
// Kay McCormick (mccor)
// 
// Analysis
// KayMcCormick.Dev
// ModelResources2.cs
// 
// 2020-04-25-2:04 PM
// 
// ---
#endregion
using System.Runtime.Serialization ;

namespace KayMcCormick.Dev
{
    [ Serialization ]
    internal sealed class ModelResources2 : IViewModel
    {
        public ModelResources2 ( ) { }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context ) { }
        #endregion
    }
}