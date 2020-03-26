﻿using System ;
using Castle.DynamicProxy ;

namespace KayMcCormick.Dev
{
    /// <summary>DynamicProxy interceptor to inject a logger into classes. TODO refactor.</summary>
    /// <seealso cref="Castle.DynamicProxy.IInterceptor" />
    public class LoggingInterceptor : IInterceptor
    {
        /// <summary>Intercepts the specified invocation.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Intercept
        public void Intercept ( IInvocation invocation )
        {
            
            var customAttributes = Attribute.GetCustomAttributes (
                                                                  invocation
                                                                     .GetConcreteMethodInvocationTarget ( )
                                                                , typeof ( PushContextAttribute )
                                                                 ) ;


            if ( invocation.InvocationTarget is IHaveLogger haveLogger )
            {
                var logger = haveLogger.Logger ;
                logger?.Trace ( $"invocation of {invocation.Method.Name}" ) ;
            }

            invocation.Proceed ( ) ;
        }
    }
}