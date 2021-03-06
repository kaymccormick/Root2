﻿#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// TokenCacheHelper.cs
// 
// 2020-03-20-1:34 PM
// 
// ---
#endregion
using System.IO ;
using System.Reflection ;
using System.Security.Cryptography ;
using JetBrains.Annotations ;
using Microsoft.Identity.Client ;

namespace ProjInterface
{
    internal static class TokenCacheHelper
    {
        /// <summary>
        ///     Path to the token cache
        /// </summary>
        public static readonly string CacheFilePath =
            Assembly.GetExecutingAssembly ( ).Location + ".msalcache.bin3" ;

        private static readonly object FileLock = new object ( ) ;

        public static void EnableSerialization ( [ NotNull ] ITokenCache tokenCache )
        {
            tokenCache.SetBeforeAccess ( BeforeAccessNotification ) ;
            tokenCache.SetAfterAccess ( AfterAccessNotification ) ;
        }


        private static void BeforeAccessNotification ( [ NotNull ] TokenCacheNotificationArgs args )
        {
            lock ( FileLock )
            {
                args.TokenCache.DeserializeMsalV3 (
                                                   File.Exists ( CacheFilePath )
                                                       ? ProtectedData.Unprotect (
                                                                                  File
                                                                                     .ReadAllBytes (
                                                                                                    CacheFilePath
                                                                                                   )
                                                                                , null
                                                                                , DataProtectionScope
                                                                                     .CurrentUser
                                                                                 )
                                                       : null
                                                  ) ;
            }
        }

        private static void AfterAccessNotification ( [ NotNull ] TokenCacheNotificationArgs args )
        {
            // if the access operation resulted in a cache update
            if ( ! args.HasStateChanged )
            {
                return ;
            }

            lock ( FileLock )
            {
                // reflect changes in the persistent store
                File.WriteAllBytes (
                                    CacheFilePath
                                  , ProtectedData.Protect (
                                                           args.TokenCache.SerializeMsalV3 ( )
                                                         , null
                                                         , DataProtectionScope.CurrentUser
                                                          )
                                   ) ;
            }
        }
    }
}