﻿using Autofac;
using JetBrains.Annotations;

namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public class InfraModule : Module
    {
        #region Overrides of Module
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<IdGeneratorModule>();
            base.Load(builder);
        }
        #endregion
    }
}