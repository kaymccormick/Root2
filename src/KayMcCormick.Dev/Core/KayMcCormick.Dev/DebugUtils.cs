﻿using Autofac.Core;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace KayMcCormick.Dev
{
    /// <summary>Extension methods for debug output.</summary>
    public static class DebugUtils
    {
        /// <summary>Debugs the format.</summary>
        /// <param name="reg">The reg.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DebugFormat
        public static string DebugFormat([NotNull] this IComponentRegistration reg)
        {
            if (reg == null)
            {
                throw new ArgumentNullException(nameof(reg));
            }

            return string.Join(
                                ", "
                              , reg.Services.Where((service, i) => service != null)
                                   .Select((service, i) => service.DebugFormat())
                               );
        }

        /// <summary>Debugs the format.</summary>
        /// <param name="service">The service.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for DebugFormat
        public static string DebugFormat([NotNull] this Service service)
        {
            if (service == null)
            {
                throw new ArgumentNullException(nameof(service));
            }

            return service.Description;
        }
    }
}