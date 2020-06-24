using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Core;

namespace KayMcCormick.Dev
{
    public class RegInfo
    {
        private IComponentRegistration _registration;
        /// <summary>
        /// Gets a unique identifier for this component (shared in all sub-contexts.)
        /// This value also appears in Services.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the activator used to create instances.
        /// </summary>
        public IInstanceActivator Activator { get; }

        /// <summary>
        /// Gets the lifetime associated with the component.
        /// </summary>
        public IComponentLifetime Lifetime { get; }

        /// <summary>
        /// Gets a value indicating whether the component instances are shared or not.
        /// </summary>
        public InstanceSharing Sharing { get; }

        /// <summary>
        /// Gets a value indicating whether the instances of the component should be disposed by the container.
        /// </summary>
        public InstanceOwnership Ownership { get; }

        /// <summary>
        /// Gets the services provided by the component.
        /// </summary>
        public IEnumerable<Autofac.Core.Service> Services { get; }

        /// <summary>
        /// Gets additional data associated with the component.
        /// </summary>
        public IDictionary<string, object?> Metadata { get; }

        /// <summary>
        /// Gets the component registration upon which this registration is based.
        /// </summary>
        Autofac.Core.IComponentRegistration Target { get; }

        /// <summary>
        /// Gets a value indicating whether the registration is a 1:1 adapter on top
        /// of another component (e.g., Meta, Func, or Owned).
        /// </summary>
        public bool IsAdapterForIndividualComponent { get; }


        public RegInfo(IComponentRegistration registration)
        {
            
            Registration = registration;
            Id = registration.Id;
            Activator = registration.Activator;
            Lifetime = registration.Lifetime;
            Sharing = registration.Sharing;
            Ownership = registration.Ownership;
            Services = registration.Services.ToList();
            Metadata = registration.Metadata;
            IsAdapterForIndividualComponent = registration.IsAdapterForIndividualComponent;
            Target = registration.Target;


        }

        public IComponentRegistration Registration
        {
            get { return _registration; }
            set
            {
                _registration = value;
                
            }
        }
    }
}