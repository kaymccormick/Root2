#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// CodeAnalysisApp1
// LogMessageRepr.cs
// 
// 2020-02-26-10:39 PM
// 
// ---
#endregion
using System.ComponentModel ;
#if !NETSTANDARD2_0
using System.Text.Json.Serialization ;
#endif
using MessageTemplates ;

namespace AnalysisFramework
{
    internal class LogMessageRepr
    {
        private readonly string _message ;

        public LogMessageRepr ( bool isMessageTemplate , object constantMessage )
        {
            IsMessageTemplate = isMessageTemplate ;
            ConstantMessage   = constantMessage ;
            if(IsMessageTemplate)
            {
                MessageTemplate = MessageTemplate.Parse ( constantMessage.ToString() ) ;
            }
        }

        public LogMessageRepr ( string message ) { _message = message ; }
        public LogMessageRepr ( ) { }

#if !NETSTANDARD2_0
        [JsonIgnore ]
#endif
        [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden)]
        public MessageTemplate MessageTemplate { get ; set ; }

        public bool IsMessageTemplate { get ; set ; }

        public object MessageExprPojo { get ; set ; }

        public object PrimaryMessage => IsMessageTemplate ? MessageTemplate?.Text ?? "" : MessageExprPojo ;

        public object ConstantMessage { get ; set ; }
    }

}