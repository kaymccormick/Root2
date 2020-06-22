using System ;
using System.IO ;
using System.Runtime.CompilerServices ;
using Autofac.Builder ;
using JetBrains.Annotations ;

namespace KayMcCormick.Dev.Container
{
    /// <summary>
    /// 
    /// </summary>
    public  static class IocExt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="callerMemberName"></param>
        /// <typeparam name="TLimit"></typeparam>
        /// <typeparam name="TActivatorData"></typeparam>
        /// <typeparam name="TRegistrationStyle"></typeparam>
        /// <returns></returns>
        [ JetBrains.Annotations.NotNull ]
        public static IRegistrationBuilder < TLimit , TActivatorData , TRegistrationStyle >
            WithCallerMetadata < TLimit , TActivatorData , TRegistrationStyle > (
                [ JetBrains.Annotations.NotNull ] this   IRegistrationBuilder < TLimit , TActivatorData , TRegistrationStyle > builder
              , [CallerFilePath]   string                                                                callerFilePath   = "",
                [CallerLineNumber] int                                                                   callerLineNumber = 0,
                [CallerMemberName] string                                                                callerMemberName = ""
            )
        {
            Type lt =  null;
            switch (builder.ActivatorData)
            {
                case IConcreteActivatorData c1:
                    lt = c1.Activator.LimitType;
                    break;
            }
            var b =builder.WithMetadata ( "CallerFilePath" , callerFilePath )
                          .WithMetadata ( "CallerFilename" ,   Path.GetFileName ( callerFilePath ) )
                          .WithMetadata ( "CallerLineNumber" , callerLineNumber )
                          .WithMetadata ( "CallerMemberName" , callerMemberName )
                          .WithMetadata ( "RandomGuid" ,       Guid.NewGuid ( ) )
                          .WithMetadata ( "GuidFrom" ,         typeof ( IocExt ) );
                return lt == null ? b : b
                          .WithMetadata("TypeHint", lt);
        }
    }
}