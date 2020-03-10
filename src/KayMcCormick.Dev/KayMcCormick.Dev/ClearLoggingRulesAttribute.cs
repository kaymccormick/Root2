#region header
// Kay McCormick (mccor)
// 
// ConfigTest
// KayMcCormick.Dev
// ClearLoggingRulesAttribute.cs
// 
// 2020-03-09-8:30 PM
// 
// ---
#endregion
using JetBrains.Annotations;
using NLog;
using System;
using System.Linq;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once UnusedType.Global
    public class ClearLoggingRulesAttribute : LoggingAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Apply([NotNull] LoggingAttributeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var fUpdated = false;
            var configurationLoggingRules = LogManager.Configuration.LoggingRules.ToList();
            foreach (var loggingRule in configurationLoggingRules.Where(
                                                                          context
                                                                             .RuleMatch
                                                                         ))
            {
                fUpdated = true;
                if (context.Target != null)
                {
                    loggingRule.Targets.Remove(context.Target);
                }

                // LogManager.Configuration.LoggingRules.Remove ( loggingRule ) ;
            }

            return fUpdated;
        }
    }
}