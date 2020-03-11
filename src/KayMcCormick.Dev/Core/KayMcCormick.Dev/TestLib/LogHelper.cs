﻿using KayMcCormick.Dev.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace KayMcCormick.Dev.TestLib
{
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LogHelper
    public static class LogHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param1"></param>
        public static void EnsureLoggingConfigured(LogDelegates.LogMethod param1 = null)
        {
            Environment.SetEnvironmentVariable("DISABLE_LOG_TARGETS", (Environment.GetEnvironmentVariable("DISABLE_LOG_TARGETS") ?? "") + ";log");

            AppLoggingConfigHelper.EnsureLoggingConfigured(param1);
        }
        /// <summary>Supplied structured logging properties for a particular test method indicated by method.. Supplied properties "TestMethodName", "TestClass",</summary>
        /// <param name="method">The method.</param>
        /// <param name="stage">The stage.</param>
        /// <returns></returns>
        /// Return a dictionary with keys
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for TestMethodProperties
        public static IDictionary TestMethodProperties(
            MethodInfo method
          , TestMethodLifecycle stage
        )
        {
            IDictionary r = new Dictionary<string, object>
            {
                ["TestMethodName"] = method.Name
            };
            if (method.DeclaringType != null)
            {
                r["TestClass"] = method.DeclaringType.ToString();
            }

            r["TestMethodLifecycle"] = stage;
            return r;
        }
    }
}