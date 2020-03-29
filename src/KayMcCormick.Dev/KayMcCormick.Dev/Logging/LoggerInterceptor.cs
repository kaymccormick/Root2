﻿#if NETFRAMEWORK
using System ;
using System.Linq ;
using Castle.DynamicProxy ;
using NLog ;

namespace KayMcCormick.Dev.Logging
{
    /// <summary></summary>
    /// <seealso cref="Castle.DynamicProxy.IInterceptor" />
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for LoggerInterceptor
    public class LoggerInterceptor : IInterceptor
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LoggerInterceptor" />
        ///     class.
        /// </summary>
        /// <param name="useLogMethod">The use log method.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for #ctor
        public LoggerInterceptor ( LogDelegates.LogMethod useLogMethod )
        {
            UseLogMethod = useLogMethod ;
        }

        /// <summary>Gets the use log method.</summary>
        /// <value>The use log method.</value>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for UseLogMethod
        public LogDelegates.LogMethod UseLogMethod { get ; }

        /// <summary>Intercepts the specified invocation.</summary>
        /// <param name="invocation">The invocation.</param>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for Intercept
        public void Intercept ( IInvocation invocation )
        {
            UseLogMethod ( $"{invocation.Method.Name} on {invocation.InvocationTarget}" ) ;
            var enumerable =
                LogLevel.AllLevels.Select ( level => level.Name == invocation.Method.Name ) ;
            var bools = enumerable as bool[] ?? enumerable.ToArray ( ) ;
            if ( bools.Any ( ) )
            {
                var level = bools.First ( ) ;
                var @params = new Type[ invocation.Arguments.Length + 1 ] ;
                var args = new object[ invocation.Arguments.Length  + 1 ] ;
                var i = 1 ;
                foreach ( var arg in invocation.Arguments )
                {
                    @params[ i ] = arg.GetType ( ) ;
                    args[ i ]    = arg ;
                    var select = invocation.Method.GetParameters ( )
                                           .Select ( ( info , i1 ) => info.Name == "message" ) ;
                    if ( select.Any ( ) )
                    {
                        args[ i ] = "hello " + arg ;
                    }

                    i += 1 ;
                }

                @params[ 0 ] = typeof ( LogLevel ) ;
                args[ 0 ]    = level ;

                //LogBuilder b = new LogBuilder(invocation.InvocationTarget as ILogger);
                var method = invocation.TargetType.GetMethod ( "Log" , @params ) ;
                if ( method != null )
                {
                    method.Invoke ( invocation.InvocationTarget , args ) ;
                }
            }
        }
    }
}
#endif