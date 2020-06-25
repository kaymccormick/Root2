using System.Threading.Tasks;
using Autofac.Core;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    public class InstanceNode : BaseNode
    {
        public override object NodeItem => InstanceInfo;

        /// <inheritdoc />
        public override object Header => InstanceInfo.Instance.GetType().FullName;

        public IComponentRegistration Registration { get; set; }
        public InstanceInfo InstanceInfo { get; set; }

        /// <inheritdoc />
        public override Task ExpandAsync()
        {
            return Task.CompletedTask;
        }
    }
}