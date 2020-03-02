using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Client;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http ;
using System.Net.Http.Headers ;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory ;
using Microsoft.TeamFoundation.Core.WebApi ;

namespace ConsoleApp1
{
    class Program
    {                                                                                     //============= Config [Edit these with your settings] =====================
        internal const string vstsCollectionUrl = "https://dev.azure.com/SatoriDev";
        internal const string clientId          = "2efc2c2e-9e9c-42b7-8b47-6423d1071c43"; //update this with your Application ID from step 2.6 (do not change this if you have an MSA backed account)
        //==========================================================================

        internal const string VSTSResourceId = "499b84ac-1321-427f-aa17-267ca6975798"; //Static value to target VSTS. Do not change


        static async Task  Main(string[] args)
        {

            AuthenticationContext ctx = GetAuthenticationContext(null);
            AuthenticationResult result = null;
            try
            {
                DeviceCodeResult codeResult = ctx.AcquireDeviceCodeAsync(VSTSResourceId, clientId).Result;
                Console.WriteLine("You need to sign in.");
                Console.WriteLine("Message: " + codeResult.Message + "\n");
                result = ctx.AcquireTokenByDeviceCodeAsync(codeResult).Result;

                var bearerAuthHeader = new AuthenticationHeaderValue("Bearer", result.AccessToken);
                ListProjects(bearerAuthHeader);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Something went wrong.");
                Console.WriteLine("Message: " + ex.Message + "\n");
            }

            return ;
            const String c_collectionUri = "https://dev.azure.com/kaymccormick";
            const String c_projectName = "KayMcCormick.Dev";
            const String c_repoName = c_projectName;
            //s7tmwakurhvs64yntp5gvotskfbifif72l3teov4ne34bd7op5qq
            // Interactively ask the user for credentials, caching them so the user isn't constantly prompted
            VssCredentials creds = new VssClientCredentials();
            creds.Storage = new VssClientCredentialStorage();

            // Connect to Azure DevOps Services
            VssConnection connection = new VssConnection(new Uri(c_collectionUri), creds);

            // Get a GitHttpClient to talk to the Git endpoints
            GitHttpClient gitClient = connection.GetClient<GitHttpClient>();


            var cl = connection.GetClient < ProjectCollectionHttpClient > ( ) ;
            var projectCollections = await cl.GetProjectCollections ( ) ;
            foreach ( var proj in projectCollections )
            {
                Console.WriteLine(proj.Name);
            }
            // Get data about a specific repository
            var repo = gitClient.GetRepositoryAsync(c_projectName, c_repoName).Result;
        }

        private static AuthenticationContext GetAuthenticationContext(string tenant)
        {
            AuthenticationContext ctx = null;
            if (tenant != null)
                ctx = new AuthenticationContext("https://login.microsoftonline.com/" + tenant);
            else
            {
                ctx = new AuthenticationContext("https://login.windows.net/common");
                if (ctx.TokenCache.Count > 0)
                {
                    string homeTenant = ctx.TokenCache.ReadItems().First().TenantId;
                    ctx = new AuthenticationContext("https://login.microsoftonline.com/" + homeTenant);
                }
            }

            return ctx;
        }

        private static void ListProjects(AuthenticationHeaderValue authHeader)
        {
            // use the httpclient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(vstsCollectionUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("User-Agent", "VstsRestApiSamples");
                client.DefaultRequestHeaders.Add("X-TFS-FedAuthRedirect", "Suppress");
                client.DefaultRequestHeaders.Authorization = authHeader;

                // connect to the REST endpoint            
                HttpResponseMessage response = client.GetAsync("_apis/projects?stateFilter=All&api-version=2.2").Result;

                // check to see if we have a succesfull respond
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("\tSuccesful REST call");
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    Console.WriteLine("{0}:{1}", response.StatusCode, response.ReasonPhrase);
                }
            }
        }
    }
}
