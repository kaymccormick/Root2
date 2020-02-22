using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;
using NLog;
using NLog.LogReceiverService;

namespace WcfService1
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class LogReceiverServer : ILogReceiverServer
    {
        public void ProcessLogMessages(NLogEvents nevents)
        {
            LogManager.GetCurrentClassLogger ( )
                      .Trace (
                              "{name}: {count}"
                            , nameof ( ProcessLogMessages )
                            , nevents.Events.Length
                             ) ;
            OperationContext context = OperationContext.Current;
            MessageProperties properties = context.IncomingMessageProperties;
            RemoteEndpointMessageProperty endpoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            string address = string.Empty;
            //http://www.simosh.com/article/ddbggghj-get-client-ip-address-using-wcf-4-5-remoteendpointmessageproperty-in-load-balanc.html
            if (properties.Keys.Contains(HttpRequestMessageProperty.Name))
            {
                HttpRequestMessageProperty endpointLoadBalancer = properties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
                if (endpointLoadBalancer != null && endpointLoadBalancer.Headers["X-Forwarded-For"] != null)
                    address = endpointLoadBalancer.Headers["X-Forwarded-For"];
            }
            if (string.IsNullOrEmpty(address))
            {
                address = endpoint.Address;
            }

            var events = nevents.ToEventInfo("Client." + address?.ToString() + ".");
            Debug.WriteLine("in: {0} {1}", nevents.Events.Length, events.Count);

            foreach (var ev in events)
            {

                var logger = LogManager.GetLogger(ev.LoggerName);
                logger.Log(ev);
            }
        }
    }
}

