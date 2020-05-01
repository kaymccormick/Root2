using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using AnalysisAppLib.Project;
using FindLogUsages;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Command;

namespace AnalysisAppLib
{
    /// <summary>
    /// Generic analyze command.
    /// </summary>
    [CategoryMetadata(Category.LogUsage)]
    [TitleMetadata("Primary analyze command")]
    [GroupMetadata("XX")]
    [CommandIdMetadata("{D4D9671C-656B-41DA-B4B1-50C550394B59}")]
    public sealed class AnalyzeCommandWrap : IBaseLibCommand
    {
        readonly AnalyzeCommand _cmd;
        private ITargetBlock<RejectedItem> _rejectTarget;
        private object _argument;
        private readonly IProjectBrowserNode projectNode;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="projectNode"></param>
        public AnalyzeCommandWrap(AnalyzeCommand cmd, IProjectBrowserNode projectNode = null)
        {
            _cmd = cmd;
            this.projectNode = projectNode;
        }

        /// <inheritdoc />
        public object Argument
        {
            get => _argument;
            set => _argument = value;
        }

        /// <inheritdoc />
        public Task<IAppCommandResult> ExecuteAsync()
        {
            return _cmd.AnalyzeCommandAsync(projectNode, _rejectTarget).ContinueWith(task => AppCommandResult.Success);
        }

        /// <inheritdoc />
        public void OnFault(AggregateException exception)
        {
            throw new NotImplementedException();
        }
    }
}