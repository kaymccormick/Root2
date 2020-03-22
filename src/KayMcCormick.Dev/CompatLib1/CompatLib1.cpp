// CompatLib1.cpp : Defines the functions for the static library.
//

#include "pch.h"
#include "framework.h"
#include "CodeAnalysisApp1.h"
#include "CompatLib.h"

// TODO: This is an example of a library function
extern "C" {

DLL1_API ULONG fnEventWriteSETUP_LOGGING_EVENT_AssumeEnabled(LPCWSTR message)
{
	return EventWriteSETUP_LOGGING_EVENT_AssumeEnabled(message);
}

DLL1_API int fnCompatLib1(void)
{
	return 5;
}
}
