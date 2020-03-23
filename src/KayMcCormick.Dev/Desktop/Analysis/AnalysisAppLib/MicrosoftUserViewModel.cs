#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// MicrosoftUserViewModel.cs
// 
// 2020-03-20-7:48 PM
// 
// ---
#endregion
using System ;
using System.ComponentModel ;
using System.Linq ;
using System.Runtime.CompilerServices ;
using System.Runtime.Serialization ;
using System.Threading.Tasks ;
using AnalysisAppLib.ViewModel ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;
using Microsoft.Graph ;
using Microsoft.Identity.Client ;

namespace AnalysisAppLib
{
    public class MicrosoftUserViewModel : IViewModel , INotifyPropertyChanged
    {
        public bool CanLogin { get { return Account == null ; } }

        private IAccount                             _account ;
        private GraphServiceClient                   _graphClient ;
        private Func < string , GraphServiceClient > _graphFunc ;
        private IPublicClientApplication             _publicClient ;

        public MicrosoftUserViewModel (
            [ NotNull ] Func < Guid , IPublicClientApplication > publicClientFunc
          , Func < string , GraphServiceClient >                 graphFunc
        )
        {
            if ( publicClientFunc == null )
            {
                throw new ArgumentNullException ( nameof ( publicClientFunc ) ) ;
            }

            _graphFunc = graphFunc ;
            _publicClient =
                publicClientFunc ( Guid.Parse ( "73d9e90c-5cd2-4fd7-9e36-4faab9404a7c" ) ) ;
        }

        public async Task Login ( )
        {
            var scopes = new[] { "user.read.all" , "group.read.all" , "contacts.read" } ;

            var app = _publicClient ;
            var accounts = await app.GetAccountsAsync ( ) ;
            AuthenticationResult result ;
            try
            {
                result = await app.AcquireTokenSilent ( scopes , accounts.FirstOrDefault ( ) )
                                  .ExecuteAsync ( ) ;
            }
            catch ( MsalUiRequiredException )
            {
                result = await app.AcquireTokenInteractive ( scopes ).ExecuteAsync ( ) ;
            }

            GraphClient = _graphFunc ( result.AccessToken ) ;
            Account     = result.Account ;
        }

        public async Task < bool > LoginSilentAsync ( )
        {
            var app = _publicClient ;
            var accounts = await app.GetAccountsAsync ( ) ;
            var account = accounts.FirstOrDefault ( ) ;

            var scopes = new[] { "user.read.all" , "group.read.all" } ;

            // if the app manages is at most one account  
            AuthenticationResult result ;
            try
            {
                result = await app.AcquireTokenSilent ( scopes , account ).ExecuteAsync ( ) ;
            }
            catch ( MsalUiRequiredException ex )
            {
                return false ;
            }

            GraphClient = _graphFunc ( result.AccessToken ) ;
            Account     = result.Account ;
            return true ;
        }

        public IAccount Account
        {
            get { return _account ; }
            private set
            {
                if ( Equals ( value , _account ) )
                {
                    return ;
                }

                _account = value ;

                OnPropertyChanged ( ) ;
            }
        }


        public GraphServiceClient GraphClient
        {
            get { return _graphClient ; }
            set
            {
                if ( Equals ( value , _graphClient ) )
                {
                    return ;
                }

                _graphClient = value ;
                OnPropertyChanged ( ) ;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged ;

        [ NotifyPropertyChangedInvocator ]
        protected void OnPropertyChanged ( [ CallerMemberName ] string propertyName = null )
        {
            PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( propertyName ) ) ;
        }

        #region Implementation of ISerializable
        public void GetObjectData ( SerializationInfo info , StreamingContext context )
        {
        }
        #endregion
    }
}