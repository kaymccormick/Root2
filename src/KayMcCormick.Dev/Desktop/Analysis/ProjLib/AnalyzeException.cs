#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// AnalyzeException.cs
// 
// 2020-03-08-8:18 PM
// 
// ---
#endregion
using System ;
using System.Runtime.Serialization ;

namespace ProjLib
{
    public class AnalyzeException : CommandException
    {
        public AnalyzeException ( ) {
        }

        public AnalyzeException ( string message ) : base ( message )
        {
        }

        public AnalyzeException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected AnalyzeException ( [ JetBrains.Annotations.NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}