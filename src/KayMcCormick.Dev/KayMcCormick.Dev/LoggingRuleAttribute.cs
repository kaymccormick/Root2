﻿using JetBrains.Annotations;
using NLog;
using NLog.Config;
using NLog.Targets;
using System;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LoggingAttribute : Attribute
    {
        /// <summary>Applies the specified context.</summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Apply
        public abstract bool Apply(LoggingAttributeContext context);
    }

    /// <summary>
    /// 
    /// </summary>
    public class LoggingAttributeContext
    {
        /// <summary>
        /// 
        /// </summary>
        public Target Target { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public bool RuleMatch([NotNull] LoggingRule rule)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            return Target == null || rule.Targets.Contains(Target);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class LoggingRuleAttribute : LoggingAttribute
    {
        private string _loggerNamePattern;

        /// <summary>
        /// 
        /// </summary>
        public Type ClassLoggerType { get; }

        /// <summary>
        /// 
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classLoggerType"></param>
        /// <param name="logLevel"></param>
        [UsedImplicitly]
        public LoggingRuleAttribute([NotNull] Type classLoggerType, [NotNull] string logLevel)
        {
            if (logLevel == null)
            {
                throw new ArgumentNullException(nameof(logLevel));
            }

            ClassLoggerType = classLoggerType ?? throw new ArgumentNullException(nameof(classLoggerType));
            LoggerNamePattern = ClassLoggerType.ToString();
            Level = LogLevel.FromString(logLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="logLevel"></param>
        public LoggingRuleAttribute(string pattern, string logLevel)
        {
            ClassLoggerType = null;
            LoggerNamePattern = pattern;
            Level = LogLevel.FromString(logLevel);
        }

        /// <summary>
        /// 
        /// </summary>
        public string LoggerNamePattern
        {
            get => _loggerNamePattern;
            set => _loggerNamePattern = value;
        }

        #region Overrides of LoggingAttribute
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Apply(LoggingAttributeContext context) => false;
        #endregion
    }
}