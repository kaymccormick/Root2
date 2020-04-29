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
    [TitleMetadata("Do it")]
    public sealed class AnalyzeCommandWrap : IBaseLibCommand
    {
        AnalyzeCommand _cmd;
        private ITargetBlock<RejectedItem> rejectTarget;
        private object _argument;
        private IProjectBrowserNode projectNode;

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

        public object Argument
        {
            get => _argument;
            set => _argument = value;
        }

        public Task<IAppCommandResult> ExecuteAsync()
        {
            return _cmd.AnalyzeCommandAsync(projectNode, rejectTarget).ContinueWith(task => AppCommandResult.Success);
        }

        public void OnFault(AggregateException exception)
        {
            throw new NotImplementedException();
        }
    }
}