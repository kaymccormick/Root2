﻿using System ;
using System.Collections.Generic ;
using System.Text.Json ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary>
    /// </summary>
    public sealed class LogEventInstance
    {
        private readonly IDictionary < string , object > _properties =
            new Dictionary < string , object > ( ) ;

        private string                          _callerClassName ;
        private string                          _callerFilePath ;
        private int                             _callerLineNumber ;
        private string                          _callerMemberName ;
        private int                             _currentTaskId ;
        private string                          _exceptionString ;
        private string                          _formattedMessage ;
        private IDictionary < string , object > _gdc = new Dictionary < string , object > ( ) ;
        private int                             _level ;
        private string                          _loggerName ;
        private int                             _managedThreadId ;
        private IDictionary < string , object > _mdlc = new Dictionary < string , object > ( ) ;
        private string                          _message ;
        private int                             _processId ;
        private long                            _sequenceId ;

        private object _serializedForm ;
        private string _threadName ;

        private DateTime _timeStamp ;

        /// <summary>
        /// </summary>
        public int Level { get { return _level ; } set { _level = value ; } }

        /// <summary>
        /// </summary>
        public string LoggerName { get { return _loggerName ; } set { _loggerName = value ; } }

        /// <summary>
        /// </summary>
        public string FormattedMessage
        {
            get { return _formattedMessage ; }
            set { _formattedMessage = value ; }
        }

        /// <summary>
        /// </summary>
        public IDictionary < string , object > Properties { get { return _properties ; } }

        /// <summary>
        /// </summary>
        public string CallerClassName
        {
            get { return _callerClassName ; }
            set { _callerClassName = value ; }
        }

        /// <summary>
        /// </summary>
        public IDictionary < string , object > UnknownFields { get ; } =
            new Dictionary < string , object > ( ) ;

        /// <summary>
        /// </summary>

        public DateTime TimeStamp { get { return _timeStamp ; } set { _timeStamp = value ; } }

        /// <summary>
        /// </summary>

        // ReSharper disable once InconsistentNaming
        public long SequenceID { get { return _sequenceId ; } set { _sequenceId = value ; } }

        /// <summary>
        /// </summary>

        public object SerializedForm
        {
            get { return _serializedForm ; }
            set { _serializedForm = value ; }
        }

        /// <summary>
        /// </summary>
        public int ProcessId { get { return _processId ; } set { _processId = value ; } }

        /// <summary>
        /// </summary>
        public int ManagedThreadId
        {
            get { return _managedThreadId ; }
            set { _managedThreadId = value ; }
        }

        /// <summary>
        /// </summary>
        public string ThreadName { get { return _threadName ; } set { _threadName = value ; } }

        /// <summary>
        /// </summary>
        public int CurrentTaskId
        {
            get { return _currentTaskId ; }
            set { _currentTaskId = value ; }
        }

        /// <summary>
        /// </summary>
        public string Message { get { return _message ; } set { _message = value ; } }

        /// <summary>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IDictionary < string , object > GDC { get { return _gdc ; } set { _gdc = value ; } }

        /// <summary>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public IDictionary < string , object > MDLC
        {
            get { return _mdlc ; }
            set { _mdlc = value ; }
        }

        /// <summary>
        /// </summary>
        public int CallerLineNumber
        {
            get { return _callerLineNumber ; }
            set { _callerLineNumber = value ; }
        }

        /// <summary>
        /// </summary>
        public string CallerFilePath
        {
            get { return _callerFilePath ; }
            set { _callerFilePath = value ; }
        }

        /// <summary>
        /// </summary>
        public string CallerMemberName
        {
            get { return _callerMemberName ; }
            set { _callerMemberName = value ; }
        }

        /// <summary>
        /// </summary>
        public string ExceptionString
        {
            get { return _exceptionString ; }
            set { _exceptionString = value ; }
        }

        /// <summary>
        /// </summary>
        /// <param name="field"></param>
        /// <param name="elem"></param>
        public void AddUnknown ( [ JetBrains.Annotations.NotNull ] string field , JsonElement elem )
        {
            UnknownFields[ field ] = elem ;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString ( )
        {
            return
                $"{GetType ( )}:\n\t{nameof ( Level )}:\t\t{Level}  ;\n\t{nameof ( LoggerName )}:\t{LoggerName}  ;\n\t{nameof ( FormattedMessage )}:\t{FormattedMessage}  ;\n\t{nameof ( Properties )}:\t\t{Properties}  ;\n\t{nameof ( CallerClassName )}:\t{CallerClassName}  ;\n\t{nameof ( UnknownFields )}:\t{UnknownFields}  ;\n\t{nameof ( TimeStamp )}:\t\t{TimeStamp}  ;\n\t{nameof ( SequenceID )}:\t{SequenceID}  ;\n\t{nameof ( SerializedForm )}:\t{SerializedForm}  ;\n\t{nameof ( ProcessId )}:\t{ProcessId}  ;\n\t{nameof ( ManagedThreadId )}:\t{ManagedThreadId}  ;\n\t{nameof ( ThreadName )}:\t{ThreadName}  ;\n\t{nameof ( CurrentTaskId )}:\t{CurrentTaskId}  ;\n\t{nameof ( Message )}:\t\t{Message}  ;\n\t{nameof ( GDC )}:\t\t{GDC}  ;\n\t{nameof ( MDLC )}:\t\t{MDLC}  ;\n\t{nameof ( CallerLineNumber )}:\t{CallerLineNumber}  ;\n\t{nameof ( CallerFilePath )}:\t{CallerFilePath}  ;\n\t{nameof ( CallerMemberName )}:\t{CallerMemberName}" ;
        }
    }
}