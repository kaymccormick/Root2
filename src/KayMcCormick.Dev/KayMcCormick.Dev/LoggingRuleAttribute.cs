﻿using System ;
using System.Linq ;
using NLog ;
using NLog.Config ;
using NLog.Targets ;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LoggingAttribute : Attribute
    {
        public abstract bool Apply ( LoggingAttributeContext context ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    public class ClearLoggingRulesAttribute : LoggingAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override bool Apply ( LoggingAttributeContext context )
        {
            var fupdated = false ;
            var configurationLoggingRules = LogManager.Configuration.LoggingRules.ToList ( ) ;
            foreach ( var loggingRule in configurationLoggingRules.Where (
                                                                          rule => context
                                                                             .RuleMatch ( rule )
                                                                         ) )
            {
                fupdated = true ;
                if ( context.Target != null )
                {
                    loggingRule.Targets.Remove ( context.Target ) ;
                }

                // LogManager.Configuration.LoggingRules.Remove ( loggingRule ) ;
            }

            return fupdated ;
        }
    }

    public class LoggingAttributeContext
    {
        public Target Target { get ; set ; }

        public bool RuleMatch ( LoggingRule rule )
        {
            return Target != null ? rule.Targets.Contains ( Target ) : true ;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [ AttributeUsage ( AttributeTargets.Class , AllowMultiple = true ) ]
    public class LoggingRuleAttribute : LoggingAttribute
    {
        private string _loggerNamePattern ;

        public Type ClassLoggerType { get ; }

        public LogLevel Level { get ; }

        public LoggingRuleAttribute(Type classLoggerType, string logLevel)
        {
            ClassLoggerType   = classLoggerType;
            LoggerNamePattern = ClassLoggerType.ToString();
            Level             = LogLevel.FromString(logLevel);
        }

        public LoggingRuleAttribute(string pattern, string logLevel)
        {
            ClassLoggerType = null ;
            LoggerNamePattern = pattern ;
            Level             = LogLevel.FromString(logLevel);
        }

        public string LoggerNamePattern
        {
            get => _loggerNamePattern ;
            set => _loggerNamePattern = value ;
        }

        #region Overrides of LoggingAttribute
        public override bool Apply ( LoggingAttributeContext context ) { return false ; }
        #endregion
    }
}