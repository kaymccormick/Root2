// CompatLib1.cpp : Defines the functions for the static library.
//

#include "pch.h"
#include "framework.h"
#include "CompatLib.h"
#include "KayMcCormickDevProvider.h"

// TODO: This is an example of a library function
extern "C" {

ULONG fnEventWriteSETUP_LOGGING_EVENT_AssumeEnabled(LPCWSTR message)
{
	return EventWriteSETUP_LOGGING_EVENT_AssumeEnabled(message);
}

}


ULONG fnEventWriteEVENT_COMPONENT_REGISTERED(LPCWSTR limitType, const GUID* id)
{
	return EventWriteEVENT_COMPONENT_REGISTERED(limitType, id);
}

