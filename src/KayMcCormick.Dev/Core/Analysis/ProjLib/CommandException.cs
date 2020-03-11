#region header
// Kay McCormick (mccor)
// 
// Deployment
// ProjLib
// CommandException.cs
// 
// 2020-03-08-8:19 PM
// 
// ---
#endregion
using System ;
using System.Runtime.Serialization ;

namespace ProjLib
{
    [Serializable]
    public class CommandException : Exception
    {
        public CommandException ( ) {
        }

        public CommandException ( string message ) : base ( message )
        {
        }

        public CommandException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        protected CommandException ( [ JetBrains.Annotations.NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}