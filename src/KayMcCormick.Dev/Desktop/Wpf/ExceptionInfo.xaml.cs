using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Collections.ObjectModel ;
using System.Diagnostics ;
using System.IO ;
using System.Linq ;
using System.Runtime.Serialization.Formatters.Soap ;
using System.Text ;
using System.Threading.Tasks ;
using System.Windows ;
using System.Windows.Controls ;
using System.Windows.Data ;
using System.Windows.Documents ;
using System.Windows.Input ;
using System.Windows.Media ;
using System.Windows.Media.Imaging ;
using System.Windows.Navigation ;
using System.Windows.Shapes ;

using KayMcCormick.Dev ;
using Newtonsoft.Json ;

namespace KayMcCormick.Lib.Wpf
{
    /// <summary>
    /// Interaction logic for ExceptionInfo.xaml
    /// </summary>
    public partial class ExceptionInfo : UserControl
    {
        private static string @xml =
            "<SOAP-ENV:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:SOAP-ENC=\"http://schemas.xmlsoap.org/soap/encoding/\" xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:clr=\"http://schemas.microsoft.com/soap/encoding/clr/1.0\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">\r\n<SOAP-ENV:Body>\r\n<a1:AggregateException id=\"ref-1\" xmlns:a1=\"http://schemas.microsoft.com/clr/ns/System\">\r\n<ClassName id=\"ref-3\">System.AggregateException</ClassName>\r\n<Message id=\"ref-4\">One or more errors occurred.</Message>\r\n<Data xsi:null=\"1\"/>\r\n<InnerException href=\"#ref-5\"/>\r\n<HelpURL xsi:null=\"1\"/>\r\n<StackTraceString xsi:null=\"1\"/>\r\n<RemoteStackTraceString xsi:null=\"1\"/>\r\n<RemoteStackIndex>0</RemoteStackIndex>\r\n<ExceptionMethod xsi:null=\"1\"/>\r\n<HResult>-2146233088</HResult>\r\n<Source xsi:null=\"1\"/>\r\n<WatsonBuckets xsi:null=\"1\"/>\r\n<InnerExceptions href=\"#ref-6\"/>\r\n</a1:AggregateException>\r\n<a3:DependencyResolutionException id=\"ref-5\" xmlns:a3=\"http://schemas.microsoft.com/clr/nsassem/Autofac.Core/Autofac%2C%20Version%3D5.1.2.0%2C%20Culture%3Dneutral%2C%20PublicKeyToken%3D17863af14b0044da\">\r\n<ClassName id=\"ref-7\">Autofac.Core.DependencyResolutionException</ClassName>\r\n<Message id=\"ref-8\">An exception was thrown while activating λ:ProjInterface.LogViewerControl -&#62; ProjInterface.LogViewerControl -&#62; λ:ProjInterface.LogViewerConfig -&#62; λ:ProjInterface.LogViewerConfig.</Message>\r\n<Data href=\"#ref-9\"/>\r\n<InnerException href=\"#ref-10\"/>\r\n<HelpURL xsi:null=\"1\"/>\r\n<StackTraceString id=\"ref-11\">   at Autofac.Core.Resolving.InstanceLookup.CreateInstance(IEnumerable`1 parameters) in C:\\projects\\autofac\\src\\Autofac\\Core\\Resolving\\InstanceLookup.cs:line 150\r\n   at Autofac.Core.Resolving.InstanceLookup.Execute() in C:\\projects\\autofac\\src\\Autofac\\Core\\Resolving\\InstanceLookup.cs:line 91\r\n   at Autofac.Core.Resolving.ResolveOperation.GetOrCreateInstance(ISharingLifetimeScope currentOperationScope, ResolveRequest request) in C:\\projects\\autofac\\src\\Autofac\\Core\\Resolving\\ResolveOperation.cs:line 125\r\n   at Autofac.Core.Resolving.ResolveOperation.Execute(ResolveRequest request) in C:\\projects\\autofac\\src\\Autofac\\Core\\Resolving\\ResolveOperation.cs:line 87\r\n   at Autofac.Core.Lifetime.LifetimeScope.ResolveComponent(ResolveRequest request) in C:\\projects\\autofac\\src\\Autofac\\Core\\Lifetime\\LifetimeScope.cs:line 279\r\n   at lambda_method(Closure , LayoutDocumentPane )\r\n   at ProjInterface.ProjInterfaceModule.CommandFunc(LambdaAppCommand command) in C:\\Users\\mccor.LAPTOP-T6T0BN1K\\source\\repos\\v3\\NewRoot\\src\\KayMcCormick.Dev\\Desktop\\Analysis\\ProjInterface\\ProjInterfaceModule.cs:line 266\r\n   at KayMcCormick.Lib.Wpf.LambdaAppCommand.&#60;ExecuteAsync&#62;d__9.MoveNext() in C:\\Users\\mccor.LAPTOP-T6T0BN1K\\source\\repos\\v3\\NewRoot\\src\\KayMcCormick.Dev\\Desktop\\Wpf\\AppCommand.cs:line 135</StackTraceString>\r\n<RemoteStackTraceString xsi:null=\"1\"/>\r\n<RemoteStackIndex>0</RemoteStackIndex>\r\n<ExceptionMethod xsi:null=\"1\"/>\r\n<HResult>-2146233088</HResult>\r\n<Source id=\"ref-12\">Autofac</Source>\r\n<WatsonBuckets xsi:null=\"1\"/>\r\n</a3:DependencyResolutionException>\r\n<SOAP-ENC:Array id=\"ref-6\" SOAP-ENC:arrayType=\"a1:Exception[1]\" xmlns:a1=\"http://schemas.microsoft.com/clr/ns/System\">\r\n<item href=\"#ref-5\"/>\r\n</SOAP-ENC:Array>\r\n<a2:ListDictionaryInternal id=\"ref-9\" xmlns:a2=\"http://schemas.microsoft.com/clr/ns/System.Collections\">\r\n<head href=\"#ref-13\"/>\r\n<version>1</version>\r\n<count>1</count>\r\n</a2:ListDictionaryInternal>\r\n<a1:InvalidOperationException id=\"ref-10\" xmlns:a1=\"http://schemas.microsoft.com/clr/ns/System\">\r\n<ClassName id=\"ref-14\">System.InvalidOperationException</ClassName>\r\n<Message id=\"ref-15\">Sequence contains no elements</Message>\r\n<Data href=\"#ref-16\"/>\r\n<InnerException xsi:null=\"1\"/>\r\n<HelpURL xsi:null=\"1\"/>\r\n<StackTraceString id=\"ref-17\">   at System.Linq.Enumerable.First[TSource](IEnumerable`1 source)\r\n   at Autofac.ParameterExtensions.ConstantValue[TParameter,TValue](IEnumerable`1 parameters, Func`2 predicate) in C:\\projects\\autofac\\src\\Autofac\\ParameterExtensions.cs:line 109\r\n   at Autofac.ParameterExtensions.TypedAs[T](IEnumerable`1 parameters) in C:\\projects\\autofac\\src\\Autofac\\ParameterExtensions.cs:line 100\r\n   at ProjInterface.ProjInterfaceModule.&#60;&#62;c.&#60;DoLoad&#62;b__2_3(IComponentContext c, IEnumerable`1 p) in C:\\Users\\mccor.LAPTOP-T6T0BN1K\\source\\repos\\v3\\NewRoot\\src\\KayMcCormick.Dev\\Desktop\\Analysis\\ProjInterface\\ProjInterfaceModule.cs:line 187\r\n   at Autofac.Builder.RegistrationBuilder.&#60;&#62;c__DisplayClass0_0`1.&#60;ForDelegate&#62;b__0(IComponentContext c, IEnumerable`1 p) in C:\\projects\\autofac\\src\\Autofac\\Builder\\RegistrationBuilder.cs:line 63\r\n   at Autofac.Core.Activators.Delegate.DelegateActivator.ActivateInstance(IComponentContext context, IEnumerable`1 parameters) in C:\\projects\\autofac\\src\\Autofac\\Core\\Activators\\Delegate\\DelegateActivator.cs:line 71\r\n   at Autofac.Core.Resolving.InstanceLookup.CreateInstance(IEnumerable`1 parameters) in C:\\projects\\autofac\\src\\Autofac\\Core\\Resolving\\InstanceLookup.cs:line 138</StackTraceString>\r\n<RemoteStackTraceString xsi:null=\"1\"/>\r\n<RemoteStackIndex>0</RemoteStackIndex>\r\n<ExceptionMethod xsi:null=\"1\"/>\r\n<HResult>-2146233079</HResult>\r\n<Source id=\"ref-18\">System.Core</Source>\r\n<WatsonBuckets xsi:null=\"1\"/>\r\n</a1:InvalidOperationException>\r\n<a2:ListDictionaryInternal_x002B_DictionaryNode id=\"ref-13\" xmlns:a2=\"http://schemas.microsoft.com/clr/ns/System.Collections\">\r\n<key id=\"ref-19\" xsi:type=\"SOAP-ENC:string\">ActivatorChain</key>\r\n<value id=\"ref-20\" xsi:type=\"SOAP-ENC:string\">λ:ProjInterface.LogViewerControl -&#62; ProjInterface.LogViewerControl -&#62; λ:ProjInterface.LogViewerConfig -&#62; λ:ProjInterface.LogViewerConfig</value>\r\n<next xsi:null=\"1\"/>\r\n</a2:ListDictionaryInternal_x002B_DictionaryNode>\r\n<a2:ListDictionaryInternal id=\"ref-16\" xmlns:a2=\"http://schemas.microsoft.com/clr/ns/System.Collections\">\r\n<head xsi:null=\"1\"/>\r\n<version>0</version>\r\n<count>0</count>\r\n</a2:ListDictionaryInternal>\r\n</SOAP-ENV:Body>\r\n</SOAP-ENV:Envelope>\r\n" ;

        private static SoapFormatter f = new SoapFormatter ( ) ;

        private ObservableCollection < StackTraceEntry > entries =
            new ObservableCollection < StackTraceEntry > ( ) ;

        private static Exception ex =
            ( Exception ) f.Deserialize ( new MemoryStream ( Encoding.UTF8.GetBytes ( xml ) ) ) ;

        public ExceptionInfo ( ) { InitializeComponent ( ) ; }

        #region Overrides of FrameworkElement
        public override void OnApplyTemplate ( )
        {
            base.OnApplyTemplate ( ) ;



            // foreach (var x in result)
            // {
            // Debug.WriteLine ( x ) ;
            // }
        }
        #endregion

        public static Exception Ex { get { return ex ; } set { ex = value ; } }

        public ObservableCollection < StackTraceEntry > Entries
        {
            get { return entries ; }
            set { entries = value ; }
        }

        private void ButtonBase_OnClick ( object sender , RoutedEventArgs e )
        {
            Window.GetWindow ( this )?.Close ( ) ;
        }

        private void ExceptionInfo_OnDataContextChanged (
            object                             sender
          , DependencyPropertyChangedEventArgs e
        )
        {
            Entries.Clear ( ) ;
            if ( e.NewValue is Exception x )
            {
                var stackTrace = x.StackTrace ;
                if ( String.IsNullOrEmpty ( stackTrace ) )
                {
                    if ( x.InnerException != null )
                    {
                        stackTrace = x.InnerException.StackTrace ;
                    }
                }

                foreach ( var stackTraceEntry in Utils.ParseStackTrace ( stackTrace ) )
                {
                    Entries.Add ( stackTraceEntry ) ;
                }
                
            }
        }
    }

    public class MyFrame
    {
    }
}