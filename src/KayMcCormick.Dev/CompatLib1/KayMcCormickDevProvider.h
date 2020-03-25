//**********************************************************************`
//* This is an include file generated by Message Compiler.             *`
//*                                                                    *`
//* Copyright (c) Microsoft Corporation. All Rights Reserved.          *`
//**********************************************************************`
#pragma once

//*****************************************************************************
//
// Notes on the ETW event code generated by MC:
//
// - Structures and arrays of structures are treated as an opaque binary blob.
//   The caller is responsible for packing the data for the structure into a
//   single region of memory, with no padding between values. The macro will
//   have an extra parameter for the length of the blob.
// - Arrays of nul-terminated strings must be packed by the caller into a
//   single binary blob containing the correct number of strings, with a nul
//   after each string. The size of the blob is specified in characters, and
//   includes the final nul.
// - If a SID is provided, its length will be determined by calling
//   GetLengthSid.
// - Arrays of SID are treated as a single binary blob. The caller is
//   responsible for packing the SID values into a single region of memory with
//   no padding.
// - The length attribute on the data element in the manifest is significant
//   for values with intype win:UnicodeString, win:AnsiString, or win:Binary.
//   The length attribute must be specified for win:Binary, and is optional for
//   win:UnicodeString and win:AnsiString (if no length is given, the strings
//   are assumed to be nul-terminated). For win:UnicodeString, the length is
//   measured in characters, not bytes.
// - For an array of win:UnicodeString, win:AnsiString, or win:Binary, the
//   length attribute applies to every value in the array, so every value in
//   the array must have the same length. The values in the array are provided
//   to the macro via a single pointer -- the caller is responsible for packing
//   all of the values into a single region of memory with no padding between
//   values.
// - Values of type win:CountedUnicodeString, win:CountedAnsiString, and
//   win:CountedBinary can be generated and collected on Vista or later.
//   However, they may not decode properly without the Windows 10 2018 Fall
//   Update.
// - Arrays of type win:CountedUnicodeString, win:CountedAnsiString, and
//   win:CountedBinary must be packed by the caller into a single region of
//   memory. The format for each item is a UINT16 byte-count followed by that
//   many bytes of data. When providing the array to the generated macro, you
//   must provide the total size of the packed array data, including the UINT16
//   sizes for each item. In the case of win:CountedUnicodeString, the data
//   size is specified in WCHAR (16-bit) units. In the case of
//   win:CountedAnsiString and win:CountedBinary, the data size is specified in
//   bytes.
//
//*****************************************************************************

#include <wmistr.h>
#include <evntrace.h>
#include <evntprov.h>

#if !defined(ETW_INLINE)
#define ETW_INLINE DECLSPEC_NOINLINE __inline
#endif

#if defined(__cplusplus)
extern "C" {
#endif

//
// MCGEN_DISABLE_PROVIDER_CODE_GENERATION macro:
// Define this macro to have the compiler skip the generated functions in this
// header.
//
#ifndef MCGEN_DISABLE_PROVIDER_CODE_GENERATION

//
// MCGEN_USE_KERNEL_MODE_APIS macro:
// Controls whether the generated code uses kernel-mode or user-mode APIs.
// - Set to 0 to use Windows user-mode APIs such as EventRegister.
// - Set to 1 to use Windows kernel-mode APIs such as EtwRegister.
// Default is based on whether the _ETW_KM_ macro is defined (i.e. by wdm.h).
// Note that the APIs can also be overridden directly, e.g. by setting the
// MCGEN_EVENTWRITETRANSFER or MCGEN_EVENTREGISTER macros.
//
#ifndef MCGEN_USE_KERNEL_MODE_APIS
  #ifdef _ETW_KM_
    #define MCGEN_USE_KERNEL_MODE_APIS 1
  #else
    #define MCGEN_USE_KERNEL_MODE_APIS 0
  #endif
#endif // MCGEN_USE_KERNEL_MODE_APIS

//
// MCGEN_HAVE_EVENTSETINFORMATION macro:
// Controls how McGenEventSetInformation uses the EventSetInformation API.
// - Set to 0 to disable the use of EventSetInformation
//   (McGenEventSetInformation will always return an error).
// - Set to 1 to directly invoke MCGEN_EVENTSETINFORMATION.
// - Set to 2 to to locate EventSetInformation at runtime via GetProcAddress
//   (user-mode) or MmGetSystemRoutineAddress (kernel-mode).
// Default is determined as follows:
// - If MCGEN_EVENTSETINFORMATION has been customized, set to 1
//   (i.e. use MCGEN_EVENTSETINFORMATION).
// - Else if the target OS version has EventSetInformation, set to 1
//   (i.e. use MCGEN_EVENTSETINFORMATION).
// - Else set to 2 (i.e. try to dynamically locate EventSetInformation).
// Note that an McGenEventSetInformation function will only be generated if one
// or more provider in a manifest has provider traits.
//
#ifndef MCGEN_HAVE_EVENTSETINFORMATION
  #ifdef MCGEN_EVENTSETINFORMATION             // if MCGEN_EVENTSETINFORMATION has been customized,
    #define MCGEN_HAVE_EVENTSETINFORMATION   1 //   directly invoke MCGEN_EVENTSETINFORMATION(...).
  #elif MCGEN_USE_KERNEL_MODE_APIS             // else if using kernel-mode APIs,
    #if NTDDI_VERSION >= 0x06040000            //   if target OS is Windows 10 or later,
      #define MCGEN_HAVE_EVENTSETINFORMATION 1 //     directly invoke MCGEN_EVENTSETINFORMATION(...).
    #else                                      //   else
      #define MCGEN_HAVE_EVENTSETINFORMATION 2 //     find "EtwSetInformation" via MmGetSystemRoutineAddress.
    #endif                                     // else (using user-mode APIs)
  #else                                        //   if target OS and SDK is Windows 8 or later,
    #if WINVER >= 0x0602 && defined(EVENT_FILTER_TYPE_SCHEMATIZED)
      #define MCGEN_HAVE_EVENTSETINFORMATION 1 //     directly invoke MCGEN_EVENTSETINFORMATION(...).
    #else                                      //   else
      #define MCGEN_HAVE_EVENTSETINFORMATION 2 //     find "EventSetInformation" via GetModuleHandleExW/GetProcAddress.
    #endif
  #endif
#endif // MCGEN_HAVE_EVENTSETINFORMATION

//
// MCGEN_EVENTWRITETRANSFER macro:
// Override to use a custom API.
//
#ifndef MCGEN_EVENTWRITETRANSFER
  #if MCGEN_USE_KERNEL_MODE_APIS
    #define MCGEN_EVENTWRITETRANSFER   EtwWriteTransfer
  #else
    #define MCGEN_EVENTWRITETRANSFER   EventWriteTransfer
  #endif
#endif // MCGEN_EVENTWRITETRANSFER

//
// MCGEN_EVENTREGISTER macro:
// Override to use a custom API.
//
#ifndef MCGEN_EVENTREGISTER
  #if MCGEN_USE_KERNEL_MODE_APIS
    #define MCGEN_EVENTREGISTER        EtwRegister
  #else
    #define MCGEN_EVENTREGISTER        EventRegister
  #endif
#endif // MCGEN_EVENTREGISTER

//
// MCGEN_EVENTSETINFORMATION macro:
// Override to use a custom API.
// (McGenEventSetInformation also affected by MCGEN_HAVE_EVENTSETINFORMATION.)
//
#ifndef MCGEN_EVENTSETINFORMATION
  #if MCGEN_USE_KERNEL_MODE_APIS
    #define MCGEN_EVENTSETINFORMATION  EtwSetInformation
  #else
    #define MCGEN_EVENTSETINFORMATION  EventSetInformation
  #endif
#endif // MCGEN_EVENTSETINFORMATION

//
// MCGEN_EVENTUNREGISTER macro:
// Override to use a custom API.
//
#ifndef MCGEN_EVENTUNREGISTER
  #if MCGEN_USE_KERNEL_MODE_APIS
    #define MCGEN_EVENTUNREGISTER      EtwUnregister
  #else
    #define MCGEN_EVENTUNREGISTER      EventUnregister
  #endif
#endif // MCGEN_EVENTUNREGISTER

//
// MCGEN_PENABLECALLBACK macro:
// Override to use a custom function pointer type.
// (Should match the type used by MCGEN_EVENTREGISTER.)
//
#ifndef MCGEN_PENABLECALLBACK
  #if MCGEN_USE_KERNEL_MODE_APIS
    #define MCGEN_PENABLECALLBACK      PETWENABLECALLBACK
  #else
    #define MCGEN_PENABLECALLBACK      PENABLECALLBACK
  #endif
#endif // MCGEN_PENABLECALLBACK

//
// MCGEN_GETLENGTHSID macro:
// Override to use a custom API.
//
#ifndef MCGEN_GETLENGTHSID
  #if MCGEN_USE_KERNEL_MODE_APIS
    #define MCGEN_GETLENGTHSID(p)      RtlLengthSid((PSID)(p))
  #else
    #define MCGEN_GETLENGTHSID(p)      GetLengthSid((PSID)(p))
  #endif
#endif // MCGEN_GETLENGTHSID

//
// MCGEN_EVENT_ENABLED macro:
// Controls how the EventWrite[EventName] macros determine whether an event is
// enabled. The default behavior is for EventWrite[EventName] to use the
// EventEnabled[EventName] macros.
//
#ifndef MCGEN_EVENT_ENABLED
#define MCGEN_EVENT_ENABLED(EventName) EventEnabled##EventName()
#endif

//
// MCGEN_EVENT_BIT_SET macro:
// Implements testing a bit in an array of ULONG, optimized for CPU type.
//
#ifndef MCGEN_EVENT_BIT_SET
#  if defined(_M_IX86) || defined(_M_X64)
#    define MCGEN_EVENT_BIT_SET(EnableBits, BitPosition) ((((const unsigned char*)EnableBits)[BitPosition >> 3] & (1u << (BitPosition & 7))) != 0)
#  else
#    define MCGEN_EVENT_BIT_SET(EnableBits, BitPosition) ((EnableBits[BitPosition >> 5] & (1u << (BitPosition & 31))) != 0)
#  endif
#endif // MCGEN_EVENT_BIT_SET

//
// MCGEN_ENABLE_CHECK macro:
// Determines whether the specified event would be considered as enabled
// based on the state of the specified context. Slightly faster than calling
// McGenEventEnabled directly.
//
#ifndef MCGEN_ENABLE_CHECK
#define MCGEN_ENABLE_CHECK(Context, Descriptor) (Context.IsEnabled && McGenEventEnabled(&Context, &Descriptor))
#endif

#if !defined(MCGEN_TRACE_CONTEXT_DEF)
#define MCGEN_TRACE_CONTEXT_DEF
typedef struct _MCGEN_TRACE_CONTEXT
{
    TRACEHANDLE            RegistrationHandle;
    TRACEHANDLE            Logger;      // Used as pointer to provider traits.
    ULONGLONG              MatchAnyKeyword;
    ULONGLONG              MatchAllKeyword;
    ULONG                  Flags;
    ULONG                  IsEnabled;
    UCHAR                  Level;
    UCHAR                  Reserve;
    USHORT                 EnableBitsCount;
    PULONG                 EnableBitMask;
    const ULONGLONG*       EnableKeyWords;
    const UCHAR*           EnableLevel;
} MCGEN_TRACE_CONTEXT, *PMCGEN_TRACE_CONTEXT;
#endif // MCGEN_TRACE_CONTEXT_DEF

#if !defined(MCGEN_LEVEL_KEYWORD_ENABLED_DEF)
#define MCGEN_LEVEL_KEYWORD_ENABLED_DEF
//
// Determines whether an event with a given Level and Keyword would be
// considered as enabled based on the state of the specified context.
// Note that you may want to use MCGEN_ENABLE_CHECK instead of calling this
// function directly.
//
FORCEINLINE
BOOLEAN
McGenLevelKeywordEnabled(
    _In_ PMCGEN_TRACE_CONTEXT EnableInfo,
    _In_ UCHAR Level,
    _In_ ULONGLONG Keyword
    )
{
    //
    // Check if the event Level is lower than the level at which
    // the channel is enabled.
    // If the event Level is 0 or the channel is enabled at level 0,
    // all levels are enabled.
    //

    if ((Level <= EnableInfo->Level) || // This also covers the case of Level == 0.
        (EnableInfo->Level == 0)) {

        //
        // Check if Keyword is enabled
        //

        if ((Keyword == (ULONGLONG)0) ||
            ((Keyword & EnableInfo->MatchAnyKeyword) &&
             ((Keyword & EnableInfo->MatchAllKeyword) == EnableInfo->MatchAllKeyword))) {
            return TRUE;
        }
    }

    return FALSE;
}
#endif // MCGEN_LEVEL_KEYWORD_ENABLED_DEF

#if !defined(MCGEN_EVENT_ENABLED_DEF)
#define MCGEN_EVENT_ENABLED_DEF
//
// Determines whether the specified event would be considered as enabled based
// on the state of the specified context. Note that you may want to use
// MCGEN_ENABLE_CHECK instead of calling this function directly.
//
FORCEINLINE
BOOLEAN
McGenEventEnabled(
    _In_ PMCGEN_TRACE_CONTEXT EnableInfo,
    _In_ PCEVENT_DESCRIPTOR EventDescriptor
    )
{
    return McGenLevelKeywordEnabled(EnableInfo, EventDescriptor->Level, EventDescriptor->Keyword);
}
#endif // MCGEN_EVENT_ENABLED_DEF

#if !defined(MCGEN_CONTROL_CALLBACK)
#define MCGEN_CONTROL_CALLBACK

DECLSPEC_NOINLINE __inline
VOID
__stdcall
McGenControlCallbackV2(
    _In_ LPCGUID SourceId,
    _In_ ULONG ControlCode,
    _In_ UCHAR Level,
    _In_ ULONGLONG MatchAnyKeyword,
    _In_ ULONGLONG MatchAllKeyword,
    _In_opt_ PEVENT_FILTER_DESCRIPTOR FilterData,
    _Inout_opt_ PVOID CallbackContext
    )
/*++

Routine Description:

    This is the notification callback for Windows Vista and later.

Arguments:

    SourceId - The GUID that identifies the session that enabled the provider.

    ControlCode - The parameter indicates whether the provider
                  is being enabled or disabled.

    Level - The level at which the event is enabled.

    MatchAnyKeyword - The bitmask of keywords that the provider uses to
                      determine the category of events that it writes.

    MatchAllKeyword - This bitmask additionally restricts the category
                      of events that the provider writes.

    FilterData - The provider-defined data.

    CallbackContext - The context of the callback that is defined when the provider
                      called EtwRegister to register itself.

Remarks:

    ETW calls this function to notify provider of enable/disable

--*/
{
    PMCGEN_TRACE_CONTEXT Ctx = (PMCGEN_TRACE_CONTEXT)CallbackContext;
    ULONG Ix;
#ifndef MCGEN_PRIVATE_ENABLE_CALLBACK_V2
    UNREFERENCED_PARAMETER(SourceId);
    UNREFERENCED_PARAMETER(FilterData);
#endif

    if (Ctx == NULL) {
        return;
    }

    switch (ControlCode) {

        case EVENT_CONTROL_CODE_ENABLE_PROVIDER:
            Ctx->Level = Level;
            Ctx->MatchAnyKeyword = MatchAnyKeyword;
            Ctx->MatchAllKeyword = MatchAllKeyword;
            Ctx->IsEnabled = EVENT_CONTROL_CODE_ENABLE_PROVIDER;

            for (Ix = 0; Ix < Ctx->EnableBitsCount; Ix += 1) {
                if (McGenLevelKeywordEnabled(Ctx, Ctx->EnableLevel[Ix], Ctx->EnableKeyWords[Ix]) != FALSE) {
                    Ctx->EnableBitMask[Ix >> 5] |= (1 << (Ix % 32));
                } else {
                    Ctx->EnableBitMask[Ix >> 5] &= ~(1 << (Ix % 32));
                }
            }
            break;

        case EVENT_CONTROL_CODE_DISABLE_PROVIDER:
            Ctx->IsEnabled = EVENT_CONTROL_CODE_DISABLE_PROVIDER;
            Ctx->Level = 0;
            Ctx->MatchAnyKeyword = 0;
            Ctx->MatchAllKeyword = 0;
            if (Ctx->EnableBitsCount > 0) {
                RtlZeroMemory(Ctx->EnableBitMask, (((Ctx->EnableBitsCount - 1) / 32) + 1) * sizeof(ULONG));
            }
            break;

        default:
            break;
    }

#ifdef MCGEN_PRIVATE_ENABLE_CALLBACK_V2
    //
    // Call user defined callback
    //
    MCGEN_PRIVATE_ENABLE_CALLBACK_V2(
        SourceId,
        ControlCode,
        Level,
        MatchAnyKeyword,
        MatchAllKeyword,
        FilterData,
        CallbackContext
        );
#endif // MCGEN_PRIVATE_ENABLE_CALLBACK_V2

    return;
}

#endif // MCGEN_CONTROL_CALLBACK

#ifndef McGenEventWrite_def
#define McGenEventWrite_def
DECLSPEC_NOINLINE __inline
ULONG __stdcall
McGenEventWrite(
    _In_ PMCGEN_TRACE_CONTEXT Context,
    _In_ PCEVENT_DESCRIPTOR Descriptor,
    _In_opt_ LPCGUID ActivityId,
    _In_range_(1, 128) ULONG EventDataCount,
    _Inout_updates_(EventDataCount) EVENT_DATA_DESCRIPTOR* EventData
    )
{
    const USHORT UNALIGNED* Traits;

    // Some customized MCGEN_EVENTWRITETRANSFER macros might ignore ActivityId.
    UNREFERENCED_PARAMETER(ActivityId);

    Traits = (const USHORT UNALIGNED*)(UINT_PTR)Context->Logger;

    if (Traits == NULL) {
        EventData[0].Ptr = 0;
        EventData[0].Size = 0;
        EventData[0].Reserved = 0;
    } else {
        EventData[0].Ptr = (ULONG_PTR)Traits;
        EventData[0].Size = *Traits;
        EventData[0].Reserved = 2; // EVENT_DATA_DESCRIPTOR_TYPE_PROVIDER_METADATA
    }

    return MCGEN_EVENTWRITETRANSFER(
        Context->RegistrationHandle,
        Descriptor,
        ActivityId,
        NULL,
        EventDataCount,
        EventData);
}
#endif // McGenEventWrite_def

#if !defined(McGenEventRegisterUnregister)
#define McGenEventRegisterUnregister

#pragma warning(push)
#pragma warning(disable:6103)
DECLSPEC_NOINLINE __inline
ULONG __stdcall
McGenEventRegister(
    _In_ LPCGUID ProviderId,
    _In_opt_ MCGEN_PENABLECALLBACK EnableCallback,
    _In_opt_ PVOID CallbackContext,
    _Inout_ PREGHANDLE RegHandle
    )
/*++

Routine Description:

    This function registers the provider with ETW.

Arguments:

    ProviderId - Provider ID to register with ETW.

    EnableCallback - Callback to be used.

    CallbackContext - Context for the callback.

    RegHandle - Pointer to registration handle.

Remarks:

    Should not be called if the provider is already registered (i.e. should not
    be called if *RegHandle != 0). Repeatedly registering a provider is a bug
    and may indicate a race condition. However, for compatibility with previous
    behavior, this function will return SUCCESS in this case.

--*/
{
    ULONG Error;

    if (*RegHandle != 0)
    {
        Error = 0; // ERROR_SUCCESS
    }
    else
    {
        Error = MCGEN_EVENTREGISTER(ProviderId, EnableCallback, CallbackContext, RegHandle);
    }

    return Error;
}
#pragma warning(pop)

DECLSPEC_NOINLINE __inline
ULONG __stdcall
McGenEventUnregister(_Inout_ PREGHANDLE RegHandle)
/*++

Routine Description:

    Unregister from ETW and set *RegHandle = 0.

Arguments:

    RegHandle - the pointer to the provider registration handle

Remarks:

    If provider has not been registered (i.e. if *RegHandle == 0),
    return SUCCESS. It is safe to call McGenEventUnregister even if the
    call to McGenEventRegister returned an error.

--*/
{
    ULONG Error;

    if(*RegHandle == 0)
    {
        Error = 0; // ERROR_SUCCESS
    }
    else
    {
        Error = MCGEN_EVENTUNREGISTER(*RegHandle);
        *RegHandle = (REGHANDLE)0;
    }

    return Error;
}

#endif // McGenEventRegisterUnregister

#endif // MCGEN_DISABLE_PROVIDER_CODE_GENERATION

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// Provider "Kay McCormick" event count 8
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

// Provider GUID = 91068038-d3ce-44bc-a0f4-966ca64e2994
EXTERN_C __declspec(selectany) const GUID PROVIDER_GUID = {0x91068038, 0xd3ce, 0x44bc, {0xa0, 0xf4, 0x96, 0x6c, 0xa6, 0x4e, 0x29, 0x94}};

#ifndef PROVIDER_GUID_Traits
#define PROVIDER_GUID_Traits NULL
#endif // PROVIDER_GUID_Traits

//
// Channel
//
#define PROVIDER_GUID_CHANNEL_c2 0x10
#define PROVIDER_GUID_CHANNEL_c3 0x11
#define PROVIDER_GUID_CHANNEL_c4 0x12

//
// Opcodes
//
#define OPCODE_SETUP_LOGGING 0xa
#define OPCODE_CODE_ANALYSIS 0xb
#define OPCODE_LOGTARGET_ATTACH 0xc

//
// Tasks
//
#define TASK_INITIALIZATION 0x1
EXTERN_C __declspec(selectany) const GUID InitializationId = {0x0fb6694d, 0xc817, 0x4f5d, {0x85, 0x35, 0xf7, 0xa7, 0x1f, 0xf8, 0x15, 0x43}};
#define TASK_CODE_ANALYSIS 0x2
EXTERN_C __declspec(selectany) const GUID CodeAnalysisId = {0x0f2f2da4, 0x5706, 0x4e2a, {0xb9, 0xa2, 0xfd, 0xd9, 0xc0, 0xd1, 0x48, 0xc4}};
#define TASK_CONTAINER_SETUP 0x3
EXTERN_C __declspec(selectany) const GUID ContainerSetupId = {0x899ccee3, 0xd93b, 0x4989, {0x84, 0x84, 0x4b, 0x6e, 0xc4, 0xc9, 0x09, 0x36}};
#define TASK_TESTING 0x4
EXTERN_C __declspec(selectany) const GUID TestingId = {0xef3c49e7, 0x0273, 0x4311, {0xbe, 0x6b, 0xef, 0x9d, 0x2b, 0xa1, 0x8b, 0x5d}};

//
// Keyword
//
#define LOGGING_KEYWORD 0x1
#define USER_INTERFACE 0x2
#define WPF_TEMPLATES 0x4
#define Config 0x8

//
// Event Descriptors
//
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR SETUP_LOGGING_EVENT = {0x1, 0x0, 0x10, 0x5, 0xa, 0x1, 0x8000000000000001};
#define SETUP_LOGGING_EVENT_value 0x1
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR CODE_ANALYSIS_EVENT = {0x2, 0x0, 0x10, 0x4, 0xb, 0x2, 0x8000000000000000};
#define CODE_ANALYSIS_EVENT_value 0x2
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR EVENT_COMPONENT_RESOLVED = {0x3, 0x0, 0x11, 0x4, 0x0, 0x0, 0x4000000000000000};
#define EVENT_COMPONENT_RESOLVED_value 0x3
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR EVENT_COMPONENT_REGISTERED = {0x4, 0x0, 0x11, 0x4, 0x0, 0x0, 0x4000000000000000};
#define EVENT_COMPONENT_REGISTERED_value 0x4
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR EVENT_TEST_OUTPUT = {0x5, 0x0, 0x12, 0x5, 0x0, 0x4, 0x2000000000000000};
#define EVENT_TEST_OUTPUT_value 0x5
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR EVENT_LEAFSERVICE_MESSAGE = {0x6, 0x0, 0x10, 0x4, 0x0, 0x0, 0x8000000000000000};
#define EVENT_LEAFSERVICE_MESSAGE_value 0x6
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR LOGTARGET_ATTACHED_EVENT = {0x7, 0x0, 0x11, 0x5, 0xc, 0x0, 0x4000000000000001};
#define LOGTARGET_ATTACHED_EVENT_value 0x7
EXTERN_C __declspec(selectany) const EVENT_DESCRIPTOR EXCEPTION_RAISED_EVENT = {0x8, 0x0, 0x10, 0x2, 0x0, 0x0, 0x8000000000000000};
#define EXCEPTION_RAISED_EVENT_value 0x8

//
// MCGEN_DISABLE_PROVIDER_CODE_GENERATION macro:
// Define this macro to have the compiler skip the generated functions in this
// header.
//
#ifndef MCGEN_DISABLE_PROVIDER_CODE_GENERATION

//
// Event Enablement Bits
//
EXTERN_C __declspec(selectany) DECLSPEC_CACHEALIGN ULONG Kay_McCormickEnableBits[1];
EXTERN_C __declspec(selectany) const ULONGLONG Kay_McCormickKeywords[6] = {0x8000000000000001, 0x8000000000000000, 0x4000000000000000, 0x2000000000000000, 0x4000000000000001, 0x8000000000000000};
EXTERN_C __declspec(selectany) const unsigned char Kay_McCormickLevels[6] = {5, 4, 4, 5, 5, 2};

//
// Provider context
//
EXTERN_C __declspec(selectany) MCGEN_TRACE_CONTEXT PROVIDER_GUID_Context = {0, (ULONG_PTR)PROVIDER_GUID_Traits, 0, 0, 0, 0, 0, 0, 6, Kay_McCormickEnableBits, Kay_McCormickKeywords, Kay_McCormickLevels};

//
// Provider REGHANDLE
//
#define Kay_McCormickHandle (PROVIDER_GUID_Context.RegistrationHandle)

//
// This macro is set to 0, indicating that the EventWrite[Name] macros do not
// have an Activity parameter. This is controlled by the -km and -um options.
//
#define PROVIDER_GUID_EventWriteActivity 0

//
// Register with ETW using the control GUID specified in the manifest.
// Invoke this macro during module initialization (i.e. program startup,
// DLL process attach, or driver load) to initialize the provider.
// Note that if this function returns an error, the error means that
// will not work, but no action needs to be taken -- even if EventRegister
// returns an error, it is generally safe to use EventWrite and
// EventUnregister macros (they will be no-ops if EventRegister failed).
//
#ifndef EventRegisterKay_McCormick
#define EventRegisterKay_McCormick() McGenEventRegister(&PROVIDER_GUID, McGenControlCallbackV2, &PROVIDER_GUID_Context, &Kay_McCormickHandle)
#endif

//
// Register with ETW using a specific control GUID (i.e. a GUID other than what
// is specified in the manifest). Advanced scenarios only.
//
#ifndef EventRegisterByGuidKay_McCormick
#define EventRegisterByGuidKay_McCormick(Guid) McGenEventRegister(&(Guid), McGenControlCallbackV2, &PROVIDER_GUID_Context, &Kay_McCormickHandle)
#endif

//
// Unregister with ETW and close the provider.
// Invoke this macro during module shutdown (i.e. program exit, DLL process
// detach, or driver unload) to unregister the provider.
// Note that you MUST call EventUnregister before DLL or driver unload
// (not optional): failure to unregister a provider before DLL or driver unload
// will result in crashes.
//
#ifndef EventUnregisterKay_McCormick
#define EventUnregisterKay_McCormick() McGenEventUnregister(&Kay_McCormickHandle)
#endif

//
// Enablement check macro for SETUP_LOGGING_EVENT
//
#define EventEnabledSETUP_LOGGING_EVENT() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 0)

//
// Event write macros for SETUP_LOGGING_EVENT
//
#define EventWriteSETUP_LOGGING_EVENT(MessageValue) \
        MCGEN_EVENT_ENABLED(SETUP_LOGGING_EVENT) \
        ? McTemplateU0z(&PROVIDER_GUID_Context, &SETUP_LOGGING_EVENT, MessageValue) : 0
#define EventWriteSETUP_LOGGING_EVENT_AssumeEnabled(MessageValue) \
        McTemplateU0z(&PROVIDER_GUID_Context, &SETUP_LOGGING_EVENT, MessageValue)

//
// Enablement check macro for CODE_ANALYSIS_EVENT
//
#define EventEnabledCODE_ANALYSIS_EVENT() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 1)

//
// Event write macros for CODE_ANALYSIS_EVENT
//
#define EventWriteCODE_ANALYSIS_EVENT(MessageValue) \
        MCGEN_EVENT_ENABLED(CODE_ANALYSIS_EVENT) \
        ? McTemplateU0z(&PROVIDER_GUID_Context, &CODE_ANALYSIS_EVENT, MessageValue) : 0
#define EventWriteCODE_ANALYSIS_EVENT_AssumeEnabled(MessageValue) \
        McTemplateU0z(&PROVIDER_GUID_Context, &CODE_ANALYSIS_EVENT, MessageValue)

//
// Enablement check macro for EVENT_COMPONENT_RESOLVED
//
#define EventEnabledEVENT_COMPONENT_RESOLVED() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 2)

//
// Event write macros for EVENT_COMPONENT_RESOLVED
//
#define EventWriteEVENT_COMPONENT_RESOLVED() \
        MCGEN_EVENT_ENABLED(EVENT_COMPONENT_RESOLVED) \
        ? McTemplateU0(&PROVIDER_GUID_Context, &EVENT_COMPONENT_RESOLVED) : 0
#define EventWriteEVENT_COMPONENT_RESOLVED_AssumeEnabled() \
        McTemplateU0(&PROVIDER_GUID_Context, &EVENT_COMPONENT_RESOLVED)

//
// Enablement check macro for EVENT_COMPONENT_REGISTERED
//
#define EventEnabledEVENT_COMPONENT_REGISTERED() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 2)

//
// Event write macros for EVENT_COMPONENT_REGISTERED
//
#define EventWriteEVENT_COMPONENT_REGISTERED(LimitType, Id) \
        MCGEN_EVENT_ENABLED(EVENT_COMPONENT_REGISTERED) \
        ? McTemplateU0zj(&PROVIDER_GUID_Context, &EVENT_COMPONENT_REGISTERED, LimitType, Id) : 0
#define EventWriteEVENT_COMPONENT_REGISTERED_AssumeEnabled(LimitType, Id) \
        McTemplateU0zj(&PROVIDER_GUID_Context, &EVENT_COMPONENT_REGISTERED, LimitType, Id)

//
// Enablement check macro for EVENT_TEST_OUTPUT
//
#define EventEnabledEVENT_TEST_OUTPUT() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 3)

//
// Event write macros for EVENT_TEST_OUTPUT
//
#define EventWriteEVENT_TEST_OUTPUT(Message) \
        MCGEN_EVENT_ENABLED(EVENT_TEST_OUTPUT) \
        ? McTemplateU0z(&PROVIDER_GUID_Context, &EVENT_TEST_OUTPUT, Message) : 0
#define EventWriteEVENT_TEST_OUTPUT_AssumeEnabled(Message) \
        McTemplateU0z(&PROVIDER_GUID_Context, &EVENT_TEST_OUTPUT, Message)

//
// Enablement check macro for EVENT_LEAFSERVICE_MESSAGE
//
#define EventEnabledEVENT_LEAFSERVICE_MESSAGE() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 1)

//
// Event write macros for EVENT_LEAFSERVICE_MESSAGE
//
#define EventWriteEVENT_LEAFSERVICE_MESSAGE(Message) \
        MCGEN_EVENT_ENABLED(EVENT_LEAFSERVICE_MESSAGE) \
        ? McTemplateU0z(&PROVIDER_GUID_Context, &EVENT_LEAFSERVICE_MESSAGE, Message) : 0
#define EventWriteEVENT_LEAFSERVICE_MESSAGE_AssumeEnabled(Message) \
        McTemplateU0z(&PROVIDER_GUID_Context, &EVENT_LEAFSERVICE_MESSAGE, Message)

//
// Enablement check macro for LOGTARGET_ATTACHED_EVENT
//
#define EventEnabledLOGTARGET_ATTACHED_EVENT() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 4)

//
// Event write macros for LOGTARGET_ATTACHED_EVENT
//
#define EventWriteLOGTARGET_ATTACHED_EVENT(TargetName, TargetType) \
        MCGEN_EVENT_ENABLED(LOGTARGET_ATTACHED_EVENT) \
        ? McTemplateU0zz(&PROVIDER_GUID_Context, &LOGTARGET_ATTACHED_EVENT, TargetName, TargetType) : 0
#define EventWriteLOGTARGET_ATTACHED_EVENT_AssumeEnabled(TargetName, TargetType) \
        McTemplateU0zz(&PROVIDER_GUID_Context, &LOGTARGET_ATTACHED_EVENT, TargetName, TargetType)

//
// Enablement check macro for EXCEPTION_RAISED_EVENT
//
#define EventEnabledEXCEPTION_RAISED_EVENT() MCGEN_EVENT_BIT_SET(Kay_McCormickEnableBits, 5)

//
// Event write macros for EXCEPTION_RAISED_EVENT
//
#define EventWriteEXCEPTION_RAISED_EVENT(ExceptionType, StackTrace, Message, __binlength, SerializedForm, ParsedStackFrames) \
        MCGEN_EVENT_ENABLED(EXCEPTION_RAISED_EVENT) \
        ? McTemplateU0zzzqbr3z(&PROVIDER_GUID_Context, &EXCEPTION_RAISED_EVENT, ExceptionType, StackTrace, Message, __binlength, SerializedForm, ParsedStackFrames) : 0
#define EventWriteEXCEPTION_RAISED_EVENT_AssumeEnabled(ExceptionType, StackTrace, Message, __binlength, SerializedForm, ParsedStackFrames) \
        McTemplateU0zzzqbr3z(&PROVIDER_GUID_Context, &EXCEPTION_RAISED_EVENT, ExceptionType, StackTrace, Message, __binlength, SerializedForm, ParsedStackFrames)

#endif // MCGEN_DISABLE_PROVIDER_CODE_GENERATION

//
// MCGEN_DISABLE_PROVIDER_CODE_GENERATION macro:
// Define this macro to have the compiler skip the generated functions in this
// header.
//
#ifndef MCGEN_DISABLE_PROVIDER_CODE_GENERATION

//
// Template Functions
//
//
//Template from manifest : (null)
//
#ifndef McTemplateU0_def
#define McTemplateU0_def
ETW_INLINE
ULONG
McTemplateU0(
    _In_ PMCGEN_TRACE_CONTEXT Context,
    _In_ PCEVENT_DESCRIPTOR Descriptor
    )
{
#define McTemplateU0_ARGCOUNT 0

    EVENT_DATA_DESCRIPTOR EventData[McTemplateU0_ARGCOUNT + 1];

    return McGenEventWrite(Context, Descriptor, NULL, McTemplateU0_ARGCOUNT + 1, EventData);
}
#endif // McTemplateU0_def

//
//Template from manifest : t2
//
#ifndef McTemplateU0z_def
#define McTemplateU0z_def
ETW_INLINE
ULONG
McTemplateU0z(
    _In_ PMCGEN_TRACE_CONTEXT Context,
    _In_ PCEVENT_DESCRIPTOR Descriptor,
    _In_opt_ PCWSTR  _Arg0
    )
{
#define McTemplateU0z_ARGCOUNT 1

    EVENT_DATA_DESCRIPTOR EventData[McTemplateU0z_ARGCOUNT + 1];

    EventDataDescCreate(&EventData[1],
                        (_Arg0 != NULL) ? _Arg0 : L"NULL",
                        (_Arg0 != NULL) ? (ULONG)((wcslen(_Arg0) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    return McGenEventWrite(Context, Descriptor, NULL, McTemplateU0z_ARGCOUNT + 1, EventData);
}
#endif // McTemplateU0z_def

//
//Template from manifest : t4
//
#ifndef McTemplateU0zj_def
#define McTemplateU0zj_def
ETW_INLINE
ULONG
McTemplateU0zj(
    _In_ PMCGEN_TRACE_CONTEXT Context,
    _In_ PCEVENT_DESCRIPTOR Descriptor,
    _In_opt_ PCWSTR  _Arg0,
    _In_ const GUID*  _Arg1
    )
{
#define McTemplateU0zj_ARGCOUNT 2

    EVENT_DATA_DESCRIPTOR EventData[McTemplateU0zj_ARGCOUNT + 1];

    EventDataDescCreate(&EventData[1],
                        (_Arg0 != NULL) ? _Arg0 : L"NULL",
                        (_Arg0 != NULL) ? (ULONG)((wcslen(_Arg0) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    EventDataDescCreate(&EventData[2],_Arg1, sizeof(GUID)  );

    return McGenEventWrite(Context, Descriptor, NULL, McTemplateU0zj_ARGCOUNT + 1, EventData);
}
#endif // McTemplateU0zj_def

//
//Template from manifest : t7
//
#ifndef McTemplateU0zz_def
#define McTemplateU0zz_def
ETW_INLINE
ULONG
McTemplateU0zz(
    _In_ PMCGEN_TRACE_CONTEXT Context,
    _In_ PCEVENT_DESCRIPTOR Descriptor,
    _In_opt_ PCWSTR  _Arg0,
    _In_opt_ PCWSTR  _Arg1
    )
{
#define McTemplateU0zz_ARGCOUNT 2

    EVENT_DATA_DESCRIPTOR EventData[McTemplateU0zz_ARGCOUNT + 1];

    EventDataDescCreate(&EventData[1],
                        (_Arg0 != NULL) ? _Arg0 : L"NULL",
                        (_Arg0 != NULL) ? (ULONG)((wcslen(_Arg0) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    EventDataDescCreate(&EventData[2],
                        (_Arg1 != NULL) ? _Arg1 : L"NULL",
                        (_Arg1 != NULL) ? (ULONG)((wcslen(_Arg1) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    return McGenEventWrite(Context, Descriptor, NULL, McTemplateU0zz_ARGCOUNT + 1, EventData);
}
#endif // McTemplateU0zz_def

//
//Template from manifest : t8
//
#ifndef McTemplateU0zzzqbr3z_def
#define McTemplateU0zzzqbr3z_def
ETW_INLINE
ULONG
McTemplateU0zzzqbr3z(
    _In_ PMCGEN_TRACE_CONTEXT Context,
    _In_ PCEVENT_DESCRIPTOR Descriptor,
    _In_opt_ PCWSTR  _Arg0,
    _In_opt_ PCWSTR  _Arg1,
    _In_opt_ PCWSTR  _Arg2,
    _In_ const unsigned int  _Arg3,
    _In_reads_(_Arg3) const unsigned char*  _Arg4,
    _In_opt_ PCWSTR  _Arg5
    )
{
#define McTemplateU0zzzqbr3z_ARGCOUNT 6

    EVENT_DATA_DESCRIPTOR EventData[McTemplateU0zzzqbr3z_ARGCOUNT + 1];

    EventDataDescCreate(&EventData[1],
                        (_Arg0 != NULL) ? _Arg0 : L"NULL",
                        (_Arg0 != NULL) ? (ULONG)((wcslen(_Arg0) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    EventDataDescCreate(&EventData[2],
                        (_Arg1 != NULL) ? _Arg1 : L"NULL",
                        (_Arg1 != NULL) ? (ULONG)((wcslen(_Arg1) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    EventDataDescCreate(&EventData[3],
                        (_Arg2 != NULL) ? _Arg2 : L"NULL",
                        (_Arg2 != NULL) ? (ULONG)((wcslen(_Arg2) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    EventDataDescCreate(&EventData[4],&_Arg3, sizeof(const unsigned int)  );

    EventDataDescCreate(&EventData[5],_Arg4, (ULONG)sizeof(char)*_Arg3);

    EventDataDescCreate(&EventData[6],
                        (_Arg5 != NULL) ? _Arg5 : L"NULL",
                        (_Arg5 != NULL) ? (ULONG)((wcslen(_Arg5) + 1) * sizeof(WCHAR)) : (ULONG)sizeof(L"NULL"));

    return McGenEventWrite(Context, Descriptor, NULL, McTemplateU0zzzqbr3z_ARGCOUNT + 1, EventData);
}
#endif // McTemplateU0zzzqbr3z_def

#endif // MCGEN_DISABLE_PROVIDER_CODE_GENERATION

#if defined(__cplusplus)
};
#endif

#define MSG_Task_Initialization_SetupLogging 0x3000000AL
#define MSG_Task_CodeAnalysis_CodeAnalysis   0x3000000BL
#define MSG_Task_Logging_LogTargetAttach     0x3000000CL
#define MSG_level_Informational              0x50000004L
#define MSG_Task_Initialization              0x70000001L
#define MSG_Task_CodeAnalysis                0x70000002L
#define MSG_task_TASK_CONTAINER_SETUP_message 0x70000003L
#define MSG_task_TASK_TESTING_message        0x70000004L
#define MSG_Provider_Name                    0x90000001L
#define MSG_Event_SetupLogging               0xB0000001L
#define MSG_Event_CodeAnalysis_CodeAnalysis  0xB0000002L
#define MSG_Event_3_message                  0xB0000003L
#define MSG_Event_4_message                  0xB0000004L
#define MSG_Event_5_message                  0xB0000005L
#define MSG_Event_LogTargetAttached          0xB0000007L
#define MSG_Event_ExceptionRaised            0xB0000008L
