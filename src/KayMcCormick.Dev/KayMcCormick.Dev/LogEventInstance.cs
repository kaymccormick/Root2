using System ;
using System.Collections.Generic ;
using System.Linq ;
using System.Text.Json ;

namespace KayMcCormick.Dev
{


    /// <summary>
    /// 
    /// </summary>
    public class LogEventInstance
    {
        private int _level;
        private string _loggerName;
        private string _formattedMessage;
        private long _sequenceId;

        private readonly IDictionary<string, object>
            _properties = new Dictionary<string, object>();

        private string _callerClassName;

        private readonly IDictionary<string, object> _unknownFields =
            new Dictionary<string, object>();

        private DateTime _timeStamp;

        /// <summary>
        /// 
        /// </summary>
        public int Level
        {
            get { return _level ; }
            set { _level = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string LoggerName
        {
            get { return _loggerName ; }
            set { _loggerName = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FormattedMessage
        {
            get { return _formattedMessage ; }
            set { _formattedMessage = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> Properties
        {
            get { return _properties ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CallerClassName
        {
            get { return _callerClassName ; }
            set { _callerClassName = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> UnknownFields
        {
            get { return _unknownFields ; }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public DateTime TimeStamp { get { return _timeStamp; } set { _timeStamp = value; } }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public long SequenceID
        {
            // ReSharper disable once UnusedMember.Global
            get { return _sequenceId ; }
            set { _sequenceId = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public object SerializedForm
        {
            get { return _serializedForm ; }
            set { _serializedForm = value ; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ProcessId { get { return _processId ; } set { _processId = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public int ManagedThreadId { get { return _managedThreadId ; } set { _managedThreadId = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string ThreadName { get { return _threadName ; } set { _threadName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentTaskId { get { return _currentTaskId ; } set { _currentTaskId = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string Message { get { return _message ; } set { _message = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary <string, object> GDC { get { return _gdc ; } set { _gdc = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary <string, object> MDLC { get { return _mdlc ; } set { _mdlc = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public int CallerLineNumber { get { return _callerLineNumber ; } set { _callerLineNumber = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string CallerFilePath { get { return _callerFilePath ; } set { _callerFilePath = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string CallerMemberName { get { return _callerMemberName ; } set { _callerMemberName = value ; } }

        /// <summary>
        /// 
        /// </summary>
        public string ExceptionString { get { return _exceptionString ; } set { _exceptionString = value ; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="elem"></param>
        // ReSharper disable once UnusedMember.Global
        public void AddUnknown(string field, in JsonElement elem)
        {
            _unknownFields[field] = elem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return $"{GetType()}:\n\t{nameof ( Level )}:\t\t{Level}  ;\n\t{nameof ( LoggerName )}:\t{LoggerName}  ;\n\t{nameof ( FormattedMessage )}:\t{FormattedMessage}  ;\n\t{nameof ( Properties )}:\t\t{Properties}  ;\n\t{nameof ( CallerClassName )}:\t{CallerClassName}  ;\n\t{nameof ( UnknownFields )}:\t{UnknownFields}  ;\n\t{nameof ( TimeStamp )}:\t\t{TimeStamp}  ;\n\t{nameof ( SequenceID )}:\t{SequenceID}  ;\n\t{nameof ( SerializedForm )}:\t{SerializedForm}  ;\n\t{nameof ( ProcessId )}:\t{ProcessId}  ;\n\t{nameof ( ManagedThreadId )}:\t{ManagedThreadId}  ;\n\t{nameof ( ThreadName )}:\t{ThreadName}  ;\n\t{nameof ( CurrentTaskId )}:\t{CurrentTaskId}  ;\n\t{nameof ( Message )}:\t\t{Message}  ;\n\t{nameof ( GDC )}:\t\t{GDC}  ;\n\t{nameof ( MDLC )}:\t\t{MDLC}  ;\n\t{nameof ( CallerLineNumber )}:\t{CallerLineNumber}  ;\n\t{nameof ( CallerFilePath )}:\t{CallerFilePath}  ;\n\t{nameof ( CallerMemberName )}:\t{CallerMemberName}" ;
        }

        private object _serializedForm;
        private int _processId ;
        private int _managedThreadId ;
        private string _threadName ;
        private int _currentTaskId ;
        private string _message ;
        private IDictionary <string, object> _gdc = new Dictionary < string , object > ();
        private IDictionary <string, object> _mdlc = new Dictionary < string , object > ();
        private int _callerLineNumber ;
        private string _callerFilePath ;
        private string _callerMemberName ;
        private string _exceptionString ;
    }
}
