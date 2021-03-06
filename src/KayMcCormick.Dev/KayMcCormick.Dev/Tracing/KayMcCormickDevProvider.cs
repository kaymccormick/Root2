//---------------------------------------------------------------------
// <autogenerated>
//
//     Generated by Message Compiler (mc.exe)
//
//     Copyright (c) Microsoft Corporation. All Rights Reserved.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </autogenerated>
//---------------------------------------------------------------------

#if NETFRAMEWORK
using System ;
using System.Diagnostics.Eventing ;
using System.Runtime.InteropServices ;

namespace KayMcCormick.Dev.Tracing
{
    public static class PROVIDER_GUID
    {
        //
        // Provider "Kay McCormick" event count = 8
        //

        internal static EventProviderVersionTwo m_provider =
            new EventProviderVersionTwo ( new Guid ( "91068038-d3ce-44bc-a0f4-966ca64e2994" ) ) ;

        //
        // Task :  eventGUIDs
        //
        private static Guid InitializationId = new Guid ( "0fb6694d-c817-4f5d-8535-f7a71ff81543" ) ;
        private static Guid CodeAnalysisId   = new Guid ( "0f2f2da4-5706-4e2a-b9a2-fdd9c0d148c4" ) ;
        private static Guid ContainerSetupId = new Guid ( "899ccee3-d93b-4989-8484-4b6ec4c90936" ) ;
        private static Guid TestingId        = new Guid ( "ef3c49e7-0273-4311-be6b-ef9d2ba18b5d" ) ;

        //
        // Event Descriptors
        //
        private static EventDescriptor SETUP_LOGGING_EVENT ;
        private static EventDescriptor CODE_ANALYSIS_EVENT ;
        private static EventDescriptor EVENT_COMPONENT_RESOLVED ;
        private static EventDescriptor EVENT_COMPONENT_REGISTERED ;
        private static EventDescriptor EVENT_TEST_OUTPUT ;
        private static EventDescriptor EVENT_LEAFSERVICE_MESSAGE ;
        private static EventDescriptor LOGTARGET_ATTACHED_EVENT ;
        private static EventDescriptor EXCEPTION_RAISED_EVENT ;

        static PROVIDER_GUID ( )
        {
            unchecked
            {
                SETUP_LOGGING_EVENT = new EventDescriptor (
                                                           0x1
                                                         , 0x0
                                                         , 0x10
                                                         , 0x5
                                                         , 0xa
                                                         , 0x1
                                                         , ( long ) 0x8000000000000001
                                                          ) ;
                CODE_ANALYSIS_EVENT = new EventDescriptor (
                                                           0x2
                                                         , 0x0
                                                         , 0x10
                                                         , 0x4
                                                         , 0xb
                                                         , 0x2
                                                         , ( long ) 0x8000000000000000
                                                          ) ;
                EVENT_COMPONENT_RESOLVED = new EventDescriptor (
                                                                0x3
                                                              , 0x0
                                                              , 0x11
                                                              , 0x4
                                                              , 0x0
                                                              , 0x0
                                                              , 0x4000000000000000
                                                               ) ;
                EVENT_COMPONENT_REGISTERED = new EventDescriptor (
                                                                  0x4
                                                                , 0x0
                                                                , 0x11
                                                                , 0x4
                                                                , 0x0
                                                                , 0x0
                                                                , 0x4000000000000000
                                                                 ) ;
                EVENT_TEST_OUTPUT = new EventDescriptor (
                                                         0x5
                                                       , 0x0
                                                       , 0x12
                                                       , 0x5
                                                       , 0x0
                                                       , 0x4
                                                       , 0x2000000000000000
                                                        ) ;
                EVENT_LEAFSERVICE_MESSAGE = new EventDescriptor (
                                                                 0x6
                                                               , 0x0
                                                               , 0x10
                                                               , 0x4
                                                               , 0x0
                                                               , 0x0
                                                               , ( long ) 0x8000000000000000
                                                                ) ;
                LOGTARGET_ATTACHED_EVENT = new EventDescriptor (
                                                                0x7
                                                              , 0x0
                                                              , 0x11
                                                              , 0x5
                                                              , 0xc
                                                              , 0x0
                                                              , 0x4000000000000001
                                                               ) ;
                EXCEPTION_RAISED_EVENT = new EventDescriptor (
                                                              0x8
                                                            , 0x0
                                                            , 0x10
                                                            , 0x2
                                                            , 0x0
                                                            , 0x0
                                                            , ( long ) 0x8000000000000000
                                                             ) ;
            }
        }

        //
        // Event method for SETUP_LOGGING_EVENT
        //
        public static bool EventWriteSETUP_LOGGING_EVENT ( string MessageValue )
        {
            return m_provider.WriteEvent ( ref SETUP_LOGGING_EVENT , MessageValue ) ;
        }

        //
        // Event method for CODE_ANALYSIS_EVENT
        //
        public static bool EventWriteCODE_ANALYSIS_EVENT ( string MessageValue )
        {
            return m_provider.WriteEvent ( ref CODE_ANALYSIS_EVENT , MessageValue ) ;
        }

        //
        // Event method for EVENT_COMPONENT_RESOLVED
        //
        public static bool EventWriteEVENT_COMPONENT_RESOLVED ( )
        {
            if ( ! m_provider.IsEnabled ( ) )
            {
                return true ;
            }

            return m_provider.TemplateEventDescriptor ( ref EVENT_COMPONENT_RESOLVED ) ;
        }

        //
        // Event method for EVENT_COMPONENT_REGISTERED
        //
        public static bool EventWriteEVENT_COMPONENT_REGISTERED ( string LimitType , Guid Id )
        {
            if ( ! m_provider.IsEnabled ( ) )
            {
                return true ;
            }

            return m_provider.Templatet4 ( ref EVENT_COMPONENT_REGISTERED , LimitType , Id ) ;
        }

        //
        // Event method for EVENT_TEST_OUTPUT
        //
        public static bool EventWriteEVENT_TEST_OUTPUT ( string Message )
        {
            return m_provider.WriteEvent ( ref EVENT_TEST_OUTPUT , Message ) ;
        }

        //
        // Event method for EVENT_LEAFSERVICE_MESSAGE
        //
        public static bool EventWriteEVENT_LEAFSERVICE_MESSAGE ( string Message )
        {
            return m_provider.WriteEvent ( ref EVENT_LEAFSERVICE_MESSAGE , Message ) ;
        }

        //
        // Event method for LOGTARGET_ATTACHED_EVENT
        //
        public static bool EventWriteLOGTARGET_ATTACHED_EVENT (
            string TargetName
          , string TargetType
        )
        {
            if ( ! m_provider.IsEnabled ( ) )
            {
                return true ;
            }

            return m_provider.Templatet7 (
                                          ref LOGTARGET_ATTACHED_EVENT
                                        , TargetName
                                        , TargetType
                                         ) ;
        }

        //
        // Event method for EXCEPTION_RAISED_EVENT
        //
        public static bool EventWriteEXCEPTION_RAISED_EVENT (
            string ExceptionType
          , string StackTrace
          , string Message
          , uint   __binlength
          , byte[] SerializedForm
          , string ParsedStackFrames
        )
        {
            if ( ! m_provider.IsEnabled ( ) )
            {
                return true ;
            }

            return m_provider.Templatet8 (
                                          ref EXCEPTION_RAISED_EVENT
                                        , ExceptionType
                                        , StackTrace
                                        , Message
                                        , __binlength
                                        , SerializedForm
                                        , ParsedStackFrames
                                         ) ;
        }
    }

    internal class EventProviderVersionTwo : EventProvider
    {
        internal EventProviderVersionTwo ( Guid id ) : base ( id ) { }

        internal bool TemplateEventDescriptor ( ref EventDescriptor eventDescriptor )
        {
            if ( IsEnabled ( eventDescriptor.Level , eventDescriptor.Keywords ) )
            {
                return WriteEvent ( ref eventDescriptor , 0 , IntPtr.Zero ) ;
            }

            return true ;
        }

        internal unsafe bool Templatet4 (
            ref EventDescriptor eventDescriptor
          , string              LimitType
          , Guid                Id
        )
        {
            var argumentCount = 2 ;
            var status = true ;

            if ( IsEnabled ( eventDescriptor.Level , eventDescriptor.Keywords ) )
            {
                var userData = stackalloc byte[ sizeof ( EventData ) * argumentCount ] ;
                var userDataPtr = ( EventData * ) userData ;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[ 0 ].Size = ( uint ) ( LimitType.Length + 1 ) * sizeof ( char ) ;

                userDataPtr[ 1 ].DataPointer = ( ulong ) & Id ;
                userDataPtr[ 1 ].Size        = ( uint ) sizeof ( Guid ) ;

                fixed ( char * a0 = LimitType )
                {
                    userDataPtr[ 0 ].DataPointer = ( ulong ) a0 ;
                    status = WriteEvent (
                                         ref eventDescriptor
                                       , argumentCount
                                       , ( IntPtr ) userData
                                        ) ;
                }
            }

            return status ;
        }

        internal unsafe bool Templatet7 (
            ref EventDescriptor eventDescriptor
          , string              TargetName
          , string              TargetType
        )
        {
            var argumentCount = 2 ;
            var status = true ;

            if ( IsEnabled ( eventDescriptor.Level , eventDescriptor.Keywords ) )
            {
                var userData = stackalloc byte[ sizeof ( EventData ) * argumentCount ] ;
                var userDataPtr = ( EventData * ) userData ;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[ 0 ].Size = ( uint ) ( TargetName.Length + 1 ) * sizeof ( char ) ;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[ 1 ].Size = ( uint ) ( TargetType.Length + 1 ) * sizeof ( char ) ;

                fixed ( char * a0 = TargetName , a1 = TargetType )
                {
                    userDataPtr[ 0 ].DataPointer = ( ulong ) a0 ;
                    userDataPtr[ 1 ].DataPointer = ( ulong ) a1 ;
                    status = WriteEvent (
                                         ref eventDescriptor
                                       , argumentCount
                                       , ( IntPtr ) userData
                                        ) ;
                }
            }

            return status ;
        }

        internal unsafe bool Templatet8 (
            ref EventDescriptor eventDescriptor
          , string              ExceptionType
          , string              StackTrace
          , string              Message
          , uint                __binlength
          , byte[]              SerializedForm
          , string              ParsedStackFrames
        )
        {
            var argumentCount = 6 ;
            var status = true ;

            if ( IsEnabled ( eventDescriptor.Level , eventDescriptor.Keywords ) )
            {
                var userData = stackalloc byte[ sizeof ( EventData ) * argumentCount ] ;
                var userDataPtr = ( EventData * ) userData ;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[ 0 ].Size = ( uint ) ( ExceptionType.Length + 1 ) * sizeof ( char ) ;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[ 1 ].Size = ( uint ) ( StackTrace.Length + 1 ) * sizeof ( char ) ;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[ 2 ].Size = ( uint ) ( Message.Length + 1 ) * sizeof ( char ) ;

                userDataPtr[ 3 ].DataPointer = ( ulong ) & __binlength ;
                userDataPtr[ 3 ].Size        = sizeof ( int ) ;

                // Value has length = __binlength:
                if ( SerializedForm.Length < sizeof ( byte ) * __binlength )
                {
                    return false ;
                }

                userDataPtr[ 4 ].Size = sizeof ( byte ) * __binlength ;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[ 5 ].Size =
                    ( uint ) ( ParsedStackFrames.Length + 1 ) * sizeof ( char ) ;

                fixed ( char * a0 = ExceptionType , a1 = StackTrace , a2 = Message , a3 =
                    ParsedStackFrames )
                {
                    userDataPtr[ 0 ].DataPointer = ( ulong ) a0 ;
                    userDataPtr[ 1 ].DataPointer = ( ulong ) a1 ;
                    userDataPtr[ 2 ].DataPointer = ( ulong ) a2 ;
                    userDataPtr[ 5 ].DataPointer = ( ulong ) a3 ;
                    fixed ( byte * b0 = SerializedForm )
                    {
                        userDataPtr[ 4 ].DataPointer = ( ulong ) b0 ;
                        status = WriteEvent (
                                             ref eventDescriptor
                                           , argumentCount
                                           , ( IntPtr ) userData
                                            ) ;
                    }
                }
            }

            return status ;
        }

        [ StructLayout ( LayoutKind.Explicit , Size = 16 ) ]
        private struct EventData
        {
            [ FieldOffset ( 0 ) ]
            internal ulong DataPointer ;

            [ FieldOffset ( 8 ) ]
            internal uint Size ;

            [ FieldOffset ( 12 ) ]
            internal readonly int Reserved ;
        }
    }
}
#endif
