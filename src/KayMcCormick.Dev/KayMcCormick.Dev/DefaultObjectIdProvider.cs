﻿using System ;
using System.Collections ;
using System.Collections.Concurrent ;
using System.Collections.Generic ;
using System.Runtime.Serialization ;
using Autofac.Core ;
using JetBrains.Annotations ;
using NLog ;
using NLog.Fluent ;

namespace KayMcCormick.Dev
{
    /// <summary>Default implementation for object ID provider service.</summary>
    /// <seealso cref="IObjectIdProvider" />
    public class DefaultObjectIdProvider : IObjectIdProvider
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger ( ) ;

        private readonly long _id ;

        private readonly ConcurrentDictionary < object , long > _registry ;

        // ReSharper disable once CollectionNeverUpdated.Local
        private readonly ConcurrentDictionary < long , object > _registryById ;

        /// <summary>
        ///     Initializes a new instance of the <see cref="object" />
        ///     class.
        /// </summary>
        public DefaultObjectIdProvider ( [ NotNull ] ObjectIDGenerator generator )
        {
            Generator     = generator ?? throw new ArgumentNullException ( nameof ( generator ) ) ;
            _registryById = new ConcurrentDictionary < long , object > ( ) ;
            _registry     = new ConcurrentDictionary < object , long > ( ) ;
            _id           = generator.GetId ( this , out _ ) ;
        }

        private ObjectIDGenerator Generator { get ; }

        private ConcurrentDictionary < Guid , ComponentInfo > ByComponent { get ; } =
            new ConcurrentDictionary < Guid , ComponentInfo > ( ) ;

        /// <summary>Gets the instance by component registration.</summary>
        /// <param name="reg">The reg.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetInstanceByComponentRegistration
        public IList < InstanceInfo > GetInstanceByComponentRegistration (
            [ NotNull ] IComponentRegistration reg
        )
        {
            if ( reg == null )
            {
                throw new ArgumentNullException ( nameof ( reg ) ) ;
            }

            Logger.Debug (
                          $"[{_id}] {nameof ( GetInstanceByComponentRegistration )} [ {reg.DebugFormat ( )} ]:"
                         ) ;

            if ( ByComponent.TryGetValue ( reg.Id , out var compInfo ) )
            {
                return new List < InstanceInfo > ( compInfo.Instances ) ;
            }

            return Array.Empty < InstanceInfo > ( ) ;
        }

        /// <summary>Gets the instance count.</summary>
        /// <param name="reg">The reg.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetInstanceCount
        public int GetInstanceCount ( IComponentRegistration reg )
        {
            return GetInstanceByComponentRegistration ( reg ).Count ;
        }

        /// <summary>Gets the object instances.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetObjectInstances
        public IEnumerable GetObjectInstances ( ) { return _registry.Keys ; }

        /// <summary>Gets the object instance identifier.</summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetObjectInstanceIdentifier
        public object GetObjectInstanceIdentifier ( object instance )
        {
            var id = Generator.HasId ( instance , out _ ) ;
            return id ;
        }

        /// <summary>
        ///     <para>
        ///         Gets the object by identifier.
        ///     </para>
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public object GetObjectById ( object id )
        {
            return _registryById.TryGetValue ( ( long ) id , out var instance ) ? instance : null ;
        }

        /// <summary>
        ///     <para>
        ///         Provides the object instance identifier.
        ///     </para>
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="eComponent">The e component.</param>
        /// <param name="eParameters">The e parameters.</param>
        /// <returns></returns>
        /// <exception cref="UnableToRegisterObjectIdException"></exception>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for ProvideObjectInstanceIdentifier
        public object ProvideObjectInstanceIdentifier (
            [ NotNull ] object        instance
          , [ NotNull ] IComponentRegistration    eComponent
          , [ NotNull ] IEnumerable < Parameter > eParameters
        )
        {
            if ( instance == null )
            {
                throw new ArgumentNullException ( nameof ( instance ) ) ;
            }

            if ( eComponent == null )
            {
                throw new ArgumentNullException ( nameof ( eComponent ) ) ;
            }

            if ( eParameters == null )
            {
                throw new ArgumentNullException ( nameof ( eParameters ) ) ;
            }

            var id = Generator.GetId ( instance , out var newFlag ) ;
            if ( newFlag )
            {
                new LogBuilder(Logger).Level(LogLevel.Debug).Message(
                              $"Provisioned ID {id} for instance of type {instance.GetType ( )}."
                             ).Property("objectId", id).Property("instanceType", instance.GetType().FullName).Write();
                RegisterObject ( instance , eComponent , eParameters , id ) ;
            }

            return id ;
        }

        private void RegisterObject (
            object                    instance
          , IComponentRegistration    eComponent
          , IEnumerable < Parameter > eParameters
          , long                      id
        )
        {
            Logger.Trace (
                          $"RegisterObject of type {instance.GetType ( )} for {eComponent.DebugFormat ( )} id is {id}"
                         ) ;
            if ( ! ByComponent.TryGetValue ( eComponent.Id , out var compReg ) )
            {
                compReg = new ComponentInfo ( ) ;
                if ( ! ByComponent.TryAdd ( eComponent.Id , compReg ) )
                {
                }
            }

            Logger.Trace ( $"Adding {instance} to reg for {eComponent.Id}" ) ;
            compReg.Instances.Add (
                                   new InstanceInfo
                                   {
                                       Instance = instance , Parameters = eParameters
                                   }
                                  ) ;
            if ( ! _registry.TryAdd ( instance , id ) )
            {
                throw new UnableToRegisterObjectIdException ( ) ;
            }
        }

        // ReSharper disable once UnusedMember.Global
        /// <summary>Gets the root nodes.</summary>
        /// <returns></returns>
        /// <autogeneratedoc />
        /// TODO Edit XML Comment Template for GetRootNodes
        public ICollection < Guid > GetRootNodes ( ) { return ByComponent.Keys ; }
    }
}