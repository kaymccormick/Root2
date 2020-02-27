#region header
// Kay McCormick (mccor)
// 
// Proj
// ProjLib
// MruItemProvider.cs
// 
// 2020-02-20-6:19 PM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using System.Diagnostics ;
using System.IO ;
using System.Runtime.Serialization ;
using JetBrains.Annotations ;
using Microsoft.VisualStudio.Settings ;
using NLog ;

namespace ProjLib
{
    public class MruItemProvider : IMruItemProvider
    {
        private Logger Logger = LogManager.GetCurrentClassLogger ( ) ;
        public List < IMruItem > GetMruItemListFor ( [ NotNull ] IVsInstance vsInstance )
        {
            if ( vsInstance == null )
            {
                throw new ArgumentNullException ( nameof ( vsInstance ) ) ;
            }

            if (vsInstance.InstallationPath == null)
            {
                throw new ArgumentException("installation path is null");
            }
            if (vsInstance.ProductPath == null)
            {
                throw new ArgumentException("product path is null");
            }

            var mruItemListFor = new List<IMruItem>();;
            var path = Path.Combine ( vsInstance.InstallationPath , vsInstance.ProductPath ) ;
            try
            {
                var ext = ExternalSettingsManager.CreateForApplication ( path ) ;
                var store = ext.GetReadOnlySettingsStore ( SettingsScope.UserSettings ) ;
                var mru = @"MRUItems\{a9c4a31f-f9cb-47a9-abc0-49ce82d0b3ac}\Items" ;
                foreach ( var name in store.GetPropertyNames ( mru ) )
                {
                    var value = store.GetString ( mru , name ) ;
                    Debug.WriteLine ( "Property name: {0}, value: {1}" , name , value ) ;
                    var strings = value.Split ( '|' ) ;
                    try
                    {
                        var mruitem = new MruItem( strings[ 0 ] , strings.Length >= 4 ? strings[ 3 ] : null) ;
                        Logger.Debug ( "Mru Item is {mruitem}" , mruitem ) ;
                        mruItemListFor.Add ( mruitem ) ;
                    }
                    catch ( Exception ex )
                    {
                        throw new PropagateException ("", ex ) ;
                    }
                }
            }
            catch ( PropagateException ex1 )
            {
                throw ex1.InnerException ?? ex1 ;
            }
            catch ( Exception ex )
            {
                Logger.Error ( ex , ex.ToString ( ) ) ;
            }

            
            return mruItemListFor ;
        }
    }

    public class PropagateException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class.</summary>
        public PropagateException ( ) {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message.</summary>
        /// <param name="message">The message that describes the error. </param>
        public PropagateException ( string message ) : base ( message )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with a specified error message and a reference to the inner exception that is the cause of this exception.</summary>
        /// <param name="message">The error message that explains the reason for the exception. </param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (<see langword="Nothing" /> in Visual Basic) if no inner exception is specified. </param>
        public PropagateException ( string message , Exception innerException ) : base ( message , innerException )
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Exception" /> class with serialized data.</summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown. </param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination. </param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info" /> parameter is <see langword="null" />. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is <see langword="null" /> or <see cref="P:System.Exception.HResult" /> is zero (0). </exception>
        protected PropagateException ( [ NotNull ] SerializationInfo info , StreamingContext context ) : base ( info , context )
        {
        }
    }
}