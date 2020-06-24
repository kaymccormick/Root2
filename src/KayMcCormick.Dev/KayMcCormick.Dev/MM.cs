using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Autofac;
using Autofac.Core;
using Autofac.Core.Activators.Delegate;
using Autofac.Core.Lifetime;
using Autofac.Core.Registration;
using Autofac.Features.Decorators;

namespace KmDevLib
{
    public class MM : IRegistrationSource
    {
        private readonly MySubjectReplaySubject _impl2;
        private IComponentLifetime _current;

        public MM(MySubjectReplaySubject impl2, IComponentLifetime current  = null)
        {
            _impl2 = impl2;
            _current = current ?? new CurrentScopeLifetime();
        }

        /// <inheritdoc />
        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service,
            Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            if (service is IServiceWithType ss)
            {
                if (service is DecoratorService)
                {
                    return Enumerable.Empty<IComponentRegistration>();
                }

                if (ss.ServiceType.IsGenericType &&
                    ss.ServiceType.GetGenericTypeDefinition() == typeof(MyReplaySubject<>))
                {


                    var xyz = registrationAccessor.Invoke(service);
                    var newGuid = Guid.NewGuid();
                    var delegateActivator = new DelegateActivator(ss.ServiceType,
                        (c, p) =>
                        {
                            // Debug.WriteLine($"{registrationAccessor}{xyz}");
                            

                            var instance = Activator.CreateInstance(ss.ServiceType);
                            // w.Subject((IMySubject)instance);
                            _impl2.Subject.OnNext((IMySubject) instance);
                            return instance;
                        });
                    // 
                    // return instance;
                    // });
                    var reg = new ComponentRegistration(newGuid, delegateActivator, 
                        _current, 
                        InstanceSharing.Shared, InstanceOwnership.OwnedByLifetimeScope,
                        new[] {service, new TypedService(typeof(IMySubject))},
                        new Dictionary<string, object>());
                    return new[]
                    {
                        reg
                    };
                }

                return Enumerable.Empty<IComponentRegistration>();
            }

            return Enumerable.Empty<IComponentRegistration>();
        }




        /// <inheritdoc />
        public bool IsAdapterForIndividualComponents { get; }
    }
}