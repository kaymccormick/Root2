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

namespace KayMcCormick.Dev.Tracing
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;
    using System.Diagnostics.Eventing;
    using Microsoft.Win32;
    using System.Runtime.InteropServices;
    using System.Security.Principal;

    public static class PROVIDER_GUID
    {
        //
        // Provider "KayMcCormick-Development-CodeAnalysis" event count = 7
        //

        internal static EventProviderVersionTwo m_provider = new EventProviderVersionTwo(new Guid("5dd6e00a-e8fc-4e66-b8d7-dc8f0d62b0a1"));
        //
        // Task :  eventGUIDs
        //
        private static Guid InitializationId = new Guid("0f6f2e80-ee12-4cc7-a8d3-91cd66013055");
        private static Guid CodeAnalysisId = new Guid("f8eb7d1a-98fe-4c21-a7fe-d626dffdfacf");
        private static Guid ContainerSetupId = new Guid("13fd22f9-ec69-4ebc-8d86-02ff4cf2394c");
        private static Guid TestingId = new Guid("d0c260eb-437e-4de6-a558-a23f5d2f15ba");

        //
        // Event Descriptors
        //
        private static EventDescriptor SETUP_LOGGING_EVENT;
        private static EventDescriptor CODE_ANALYSIS_EVENT;
        private static EventDescriptor EVENT_COMPONENT_RESOLVED;
        private static EventDescriptor EVENT_COMPONENT_REGISTERED;
        private static EventDescriptor EVENT_TEST_OUTPUT;
        private static EventDescriptor EVENT_LEAFSERVICE_MESSAGE;
        private static EventDescriptor LOGTARGET_ATTACHED_EVENT;

        static PROVIDER_GUID()
        {
            unchecked
            {
                SETUP_LOGGING_EVENT = new EventDescriptor(0x1, 0x0, 0x10, 0x5, 0xa, 0x1, (long)0x8000000000000001);
                CODE_ANALYSIS_EVENT = new EventDescriptor(0x2, 0x0, 0x10, 0x4, 0xb, 0x2, (long)0x8000000000000000);
                EVENT_COMPONENT_RESOLVED = new EventDescriptor(0x3, 0x0, 0x11, 0x4, 0x0, 0x0, (long)0x4000000000000000);
                EVENT_COMPONENT_REGISTERED = new EventDescriptor(0x4, 0x0, 0x11, 0x4, 0x0, 0x0, (long)0x4000000000000000);
                EVENT_TEST_OUTPUT = new EventDescriptor(0x5, 0x0, 0x12, 0x5, 0x0, 0x4, (long)0x2000000000000000);
                EVENT_LEAFSERVICE_MESSAGE = new EventDescriptor(0x6, 0x0, 0x10, 0x4, 0x0, 0x0, (long)0x8000000000000000);
                LOGTARGET_ATTACHED_EVENT = new EventDescriptor(0x7, 0x0, 0x11, 0x5, 0xc, 0x0, (long)0x4000000000000001);
            }
        }

        //
        // Event method for SETUP_LOGGING_EVENT
        //
        public static bool EventWriteSETUP_LOGGING_EVENT(string MessageValue)
        {
            return m_provider.WriteEvent(ref SETUP_LOGGING_EVENT, MessageValue);
        }

        //
        // Event method for CODE_ANALYSIS_EVENT
        //
        public static bool EventWriteCODE_ANALYSIS_EVENT(string MessageValue)
        {
            return m_provider.WriteEvent(ref CODE_ANALYSIS_EVENT, MessageValue);
        }

        //
        // Event method for EVENT_COMPONENT_RESOLVED
        //
        public static bool EventWriteEVENT_COMPONENT_RESOLVED()
        {
            if (!m_provider.IsEnabled())
            {
                return true;
            }

            return m_provider.TemplateEventDescriptor(ref EVENT_COMPONENT_RESOLVED);
        }

        //
        // Event method for EVENT_COMPONENT_REGISTERED
        //
        public static bool EventWriteEVENT_COMPONENT_REGISTERED(string LimitType, Guid Id)
        {
            if (!m_provider.IsEnabled())
            {
                return true;
            }

            return m_provider.Templatet4(ref EVENT_COMPONENT_REGISTERED, LimitType, Id);
        }

        //
        // Event method for EVENT_TEST_OUTPUT
        //
        public static bool EventWriteEVENT_TEST_OUTPUT(string Message)
        {
            return m_provider.WriteEvent(ref EVENT_TEST_OUTPUT, Message);
        }

        //
        // Event method for EVENT_LEAFSERVICE_MESSAGE
        //
        public static bool EventWriteEVENT_LEAFSERVICE_MESSAGE(string Message)
        {
            return m_provider.WriteEvent(ref EVENT_LEAFSERVICE_MESSAGE, Message);
        }

        //
        // Event method for LOGTARGET_ATTACHED_EVENT
        //
        public static bool EventWriteLOGTARGET_ATTACHED_EVENT(string TargetName, string TargetType)
        {
            if (!m_provider.IsEnabled())
            {
                return true;
            }

            return m_provider.Templatet7(ref LOGTARGET_ATTACHED_EVENT, TargetName, TargetType);
        }
    }

    internal class EventProviderVersionTwo : EventProvider
    {
         internal EventProviderVersionTwo(Guid id)
                : base(id)
         {}

        [StructLayout(LayoutKind.Explicit, Size = 16)]
        private struct EventData
        {
            [FieldOffset(0)]
            internal UInt64 DataPointer;
            [FieldOffset(8)]
            internal uint Size;
            [FieldOffset(12)]
            internal int Reserved;
        }

        internal bool TemplateEventDescriptor(
            ref EventDescriptor eventDescriptor
            )
        {
            if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords)){
                return WriteEvent(ref eventDescriptor, 0, IntPtr.Zero);
            }

            return true;
        }

        internal unsafe bool Templatet4(
            ref EventDescriptor eventDescriptor,
            string LimitType,
            Guid Id
            )
        {
            int argumentCount = 2;
            bool status = true;

            if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
            {
                byte* userData = stackalloc byte[sizeof(EventData) * argumentCount];
                EventData* userDataPtr = (EventData*)userData;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[0].Size = (uint)(LimitType.Length + 1)*sizeof(char);

                userDataPtr[1].DataPointer = (UInt64)(&Id);
                userDataPtr[1].Size = (uint)(sizeof(Guid)  );

                fixed (char* a0 = LimitType)
                {
                    userDataPtr[0].DataPointer = (ulong)a0;
                    status = WriteEvent(ref eventDescriptor, argumentCount, (IntPtr)(userData));
                }
            }

            return status;
        }

        internal unsafe bool Templatet7(
            ref EventDescriptor eventDescriptor,
            string TargetName,
            string TargetType
            )
        {
            int argumentCount = 2;
            bool status = true;

            if (IsEnabled(eventDescriptor.Level, eventDescriptor.Keywords))
            {
                byte* userData = stackalloc byte[sizeof(EventData) * argumentCount];
                EventData* userDataPtr = (EventData*)userData;

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[0].Size = (uint)(TargetName.Length + 1)*sizeof(char);

                // Value is a nul-terminated string (assume no embedded nuls):
                userDataPtr[1].Size = (uint)(TargetType.Length + 1)*sizeof(char);

                fixed (char* a0 = TargetName, a1 = TargetType)
                {
                    userDataPtr[0].DataPointer = (ulong)a0;
                    userDataPtr[1].DataPointer = (ulong)a1;
                    status = WriteEvent(ref eventDescriptor, argumentCount, (IntPtr)(userData));
                }
            }

            return status;
        }
    }
}