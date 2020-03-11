#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjLib
// FailedBuildException.cs
// 
// 2020-03-04-10:52 AM
// 
// ---
#endregion
using System ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;

namespace ProjLib
{
    [Serializable]
    public class FailedBuildException : Exception
    {
        public FailedBuildException ( ) {
        }

        [ UsedImplicitly ]
        public FailedBuildException ( string message ) : base ( message )
        {
        }
        [UsedImplicitly]
        public FailedBuildException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }
        [UsedImplicitly]
        protected FailedBuildException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}