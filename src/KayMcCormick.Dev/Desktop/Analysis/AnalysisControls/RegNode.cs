using System.Threading.Tasks;
using Autofac.Core;
using KayMcCormick.Dev.Interfaces;

namespace AnalysisControls
{
    public class RegNode : BaseNode
    {
        private IComponentRegistration _registration;
        private IObjectIdProvider _idProvider;

        public IComponentRegistration Registration
        {
            get { return _registration; }
            set
            {
                _registration = value;
                if (_registration != null && IdProvider != null)
                {
                    _items.Clear();
                    var i = IdProvider.GetComponentInfo(_registration.Id);
                    if (i != null)
                        foreach (var instanceInfo in i.Instances)
                        {
                            _items.Add(new InstanceNode()
                            {
                                LifetimeScope = LifetimeScope,
                                IdProvider = IdProvider,
                                Registration = Registration,
                                InstanceInfo = instanceInfo
                            });
                        }

                }
            }
        }


        public override object NodeItem => Registration;

        /// <inheritdoc />
        public override object Header => Registration.Activator.LimitType.FullName;

        /// <inheritdoc />
        public override IObjectIdProvider IdProvider
        {
            get { return _idProvider; }
            set
            {
                _idProvider = value;

                if (Registration != null && _idProvider != null)
                {
                    _items.Clear();
                    var i = _idProvider.GetComponentInfo(Registration.Id);
                    if (i != null)
                        foreach (var instanceInfo in i.Instances)
                        {
                            _items.Add(new InstanceNode()
                            {
                                LifetimeScope = LifetimeScope,
                                IdProvider = _idProvider,
                                Registration = Registration,
                                InstanceInfo = instanceInfo
                            });
                        }

                }

            }
        }

        /// <inheritdoc />
        public override Task ExpandAsync()
        {
            _items.Clear();
            var i = IdProvider.GetComponentInfo(Registration.Id);
            if (i != null)
                foreach (var instanceInfo in i.Instances)
                {
                    _items.Add(new InstanceNode()
                    {
                        LifetimeScope = LifetimeScope,
                        IdProvider = IdProvider,
                        Registration = Registration,
                        InstanceInfo = instanceInfo
                    });
                }

            _isExpanded = true;
            OnPropertyChanged(nameof(IsExpanded));
            return Task.CompletedTask;
        }
    }
}