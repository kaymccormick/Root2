﻿using JetBrains.Annotations;
using KayMcCormick.Dev.Logging;
using NLog;
using NLog.Fluent;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public static class Utils
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="level"></param>
        /// <param name="logger"></param>
        [UsedImplicitly]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        private static void HandleInnerExceptions(
            Exception e
          , LogLevel level = null
          , Logger logger = null
        )
        {
            using (var stringWriter = new StringWriter())
            {
                TextWriter s = stringWriter;
                try
                {
                    void doLog(Exception exception)
                    {
                        new LogBuilder(logger ?? Logger)
                           .Level(level ?? LogLevel.Debug)
                           .Exception(exception)
                           .Message(exception.Message)
                           .Write();
                    }

                    s.WriteLine(e.Message);
                    var inner = e.InnerException;
                    var seen = new HashSet<object>();
                    while (inner != null
                            && !seen.Contains(inner))
                    {
                        doLog(inner);
                        seen.Add(inner);
                        inner = inner.InnerException;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception: " + ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="out"></param>
        public static void
            PerformLogConfigDump(TextWriter @out)
        {
            var doDumpConfig = AppLoggingConfigHelper.DoDumpConfig(s => { });
            using (IndentedTextWriter writer = new IndentedTextWriter(@out))
            {
                DoDump(writer, doDumpConfig);
            }
        }

        private static void DoDump(
            IndentedTextWriter dumpConfig
          , IDictionary doDumpConfig
          , [UsedImplicitly] int depth = 0
        )
        {
            foreach (var key in doDumpConfig.Keys)
            {
                string @out = key.ToString();
                var value = doDumpConfig[key];
                if (!(value is IDictionary || value is string)
                     && value is IEnumerable e)
                {
                    Dictionary<int, object> d = new Dictionary<int, object>();
                    int i = 0;
                    foreach (var yy in e)
                    {
                        d[i] = yy;
                        i++;
                    }

                    value = d;
                }

                if (value is IDictionary dict)
                {
                    dumpConfig.WriteLine($"{key}:");
                    dumpConfig.Indent += 1;
                    DoDump(dumpConfig, dict, depth + 1);
                    dumpConfig.Indent -= 1;
                }
                else
                {
                    @out = @out + " = " + value;
                    dumpConfig.WriteLine(@out);
                }
            }
        }
    }
}