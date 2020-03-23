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
using MessageTemplates ;

#if !NETSTANDARD2_0
#endif

namespace AnalysisAppLib
{
    internal class LogMessageRepr
    {
        public LogMessageRepr ( bool isMessageTemplate , object constantMessage )
        {
            IsMessageTemplate = isMessageTemplate ;
            ConstantMessage   = constantMessage ;
            if(IsMessageTemplate)
            {
                MessageTemplate = MessageTemplate.Parse ( constantMessage.ToString() ) ;
            }
        }

        public LogMessageRepr ( ) { }

        public MessageTemplate MessageTemplate { get ; set ; }

        public bool IsMessageTemplate { get ; set ; }

        public object MessageExprPojo { get ; set ; }

        public object PrimaryMessage => IsMessageTemplate ? MessageTemplate?.Text ?? "" : MessageExprPojo ;

        public object ConstantMessage { get ; set ; }
    }

}