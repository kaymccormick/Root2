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
        [ NotNull ]
        public static IRegistrationBuilder < TLimit , TActivatorData , TRegistrationStyle >
            WithCallerMetadata < TLimit , TActivatorData , TRegistrationStyle > (
                [ NotNull ] this   IRegistrationBuilder < TLimit , TActivatorData , TRegistrationStyle > builder
              , [CallerFilePath]   string                                                                callerFilePath   = "",
                [CallerLineNumber] int                                                                   callerLineNumber = 0,
                [CallerMemberName] string                                                                callerMemberName = ""
            )
        {
            return builder.WithMetadata("CallerFilePath", callerFilePath).WithMetadata("CallerFilename",Path.GetFileName(callerFilePath)).WithMetadata("CallerLineNumber",  callerLineNumber)
                          .WithMetadata("CallerMemberName",  callerMemberName);
        }
    }
}