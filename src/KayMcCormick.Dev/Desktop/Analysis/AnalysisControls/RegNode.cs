using System.Threading.Tasks;
using Autofac.Core;

namespace AnalysisControls
{
    public class RegNode : BaseNode
    {
        public IComponentRegistration Registration { get; set; }

        public override object NodeItem => Registration;

        /// <inheritdoc />
        public override object Header => Registration.Activator.LimitType.FullName;

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