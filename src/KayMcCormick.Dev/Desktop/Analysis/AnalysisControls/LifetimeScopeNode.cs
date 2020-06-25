using System.Threading.Tasks;

namespace AnalysisControls
{
    public class LifetimeScopeNode : BaseNode
    {
        public override object NodeItem
        {
            get { return LifetimeScope; }
        }

        /// <inheritdoc />
        public override object Header => $"LifetimeScope {LifetimeScope.Tag}";

        /// <inheritdoc />
        public override Task ExpandAsync()
        {
            _items.Clear();
            foreach (var reg in LifetimeScope.ComponentRegistry.Registrations)
            {
                var node = new RegNode() { Registration = reg, LifetimeScope = LifetimeScope, IdProvider = IdProvider };
                _items.Add(node);

            }

            _isExpanded = true;
            OnPropertyChanged(nameof(IsExpanded));
            return Task.CompletedTask;
        }
    }
}