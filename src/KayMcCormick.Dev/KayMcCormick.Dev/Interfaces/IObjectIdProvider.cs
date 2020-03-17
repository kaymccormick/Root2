﻿#region header
// Kay McCormick (mccor)
// 
// FileFinder3
// WpfApp1
// IObjectIdProvider.cs
// 
// 2020-01-25-12:54 AM
// 
// ---
#endregion
using Autofac.Core;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KayMcCormick.Dev.Interfaces
{
    // [ InterfaceMetadata ( typeof ( ObjectIdControl ) ) ]
    /// <summary></summary>
    /// <autogeneratedoc />
    /// TODO Edit XML Comment Template for IObjectIdProvider
    public interface IObjectIdProvider
    {
        /// <summary>Gets the instance by component registration.</summary>
        /// <param name="reg">The reg.</param>
        /// <returns></returns>
        [UsedImplicitly]
        IList<InstanceInfo> GetInstanceByComponentRegistration(IComponentRegistration reg);

        /// <summary>Gets the instance count.</summary>
        /// <param name="reg">The reg.</param>
        /// <returns></returns>
        [UsedImplicitly]
        int GetInstanceCount(IComponentRegistration reg);

        /// <summary>Gets the object instances.</summary>
        /// <returns></returns>
        IEnumerable GetObjectInstances();

        /// <summary>Gets the object instance identifier.</summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetObjectInstanceIdentifier
        object GetObjectInstanceIdentifier(object instance);

        /// <summary>
        ///     <para>
        ///         Gets the object by identifier.
        ///     </para>
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        object GetObjectById(object id);

        /// <summary>Provides the object instance identifier.</summary>
        /// <param name="instance">The instance.</param>
        /// <param name="eComponent">The e component.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ProvideObjectInstanceIdentifier
        object ProvideObjectInstanceIdentifier(
            object instance
          , IComponentRegistration eComponent
          , IEnumerable<Parameter> parameters
        );

        /// <summary>Gets the root nodes.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetRootNodes
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        ICollection<Guid> GetRootNodes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="regId"></param>
        /// <returns></returns>
        ComponentInfo GetComponentInfo(Guid regId);
    }
}