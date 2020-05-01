using System;
using System.Threading.Tasks;
using AnalysisAppLib;
using AnalysisAppLib.Project;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls.Commands
{
    /// <summary>
    /// DatabasePopulateCommand
    /// </summary>
    [TitleMetadata("Populate Database")]
    [CategoryMetadata(Category.Infrastructure)]
    [GroupMetadata("Tasks")]
    [CommandIdMetadata("{D4D9671C-656B-41DA-B4B1-50C550394B59}")]
    [UsedImplicitly]
    public class DatabasePopulateCommand : AppCommand
    {
        private readonly Lazy<IAppDbContext1> _dbConLazy;

#pragma warning disable 1591
        public DatabasePopulateCommand(Lazy<IAppDbContext1> dbConLazy) : base("Populate Database")
#pragma warning restore 1591
        {
            _dbConLazy = dbConLazy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task PopulateDbAsync(
            IBaseLibCommand command, IAppDbContext1 db
        )
        {
            var p = new ProjectBrowserViewModel();
            await db.Projects.AddRangeAsync(p.Projects);
            await db.SaveChangesAsync();
        }

        /// <inheritdoc />
        public override object Argument { get; set; }

        /// <inheritdoc />
        public override async Task<IAppCommandResult> ExecuteAsync()
        {
            await PopulateDbAsync(this, _dbConLazy.Value);
            return AppCommandResult.Success;
        }

        /// <inheritdoc />
        public override void OnFault(AggregateException exception)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object LargeImageSourceKey { get; set; }

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            return true;
        }
    }
}