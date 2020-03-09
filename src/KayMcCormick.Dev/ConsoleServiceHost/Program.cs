using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel ;
using System.Text;
using System.Threading.Tasks;
using AnalysisServiceLibrary ;
using KayMcCormick.Dev.Interfaces ;

namespace ConsoleServiceHost
{
    class Program
    {
        static void Main(string[] args)
        {
            CentralService service = new CentralService();
            ServiceHost host2 = new ServiceHost ( service, new Uri("http://localhost:8737/CentralSvc" )) ;
            host2.Open();

            using (ServiceHost serviceHost = new ServiceHost(typeof(AnalysisService1), new Uri("http://localhost:8738/AnalysisService1")))
            {
                try
                {
                    // Open the ServiceHost to start listening for messages.
                    serviceHost.Open();

                    // The service can now be accessed.
                    Console.WriteLine("The service is ready.");
                    Console.WriteLine("Press <ENTER> to terminate service.");
                    Console.ReadLine();

                    // Close the ServiceHost.
                    serviceHost.Close();
                }
                catch (TimeoutException timeProblem)
                {
                    Console.WriteLine(timeProblem.Message);
                    Console.ReadLine();
                }
                catch (CommunicationException commProblem)
                {
                    Console.WriteLine(commProblem.Message);
                    Console.ReadLine();
                }
            }
        }
    }
    
}
