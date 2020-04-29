using System.Threading;
using System.Threading.Tasks;
using AnalysisAppLib.Project;
using AnalysisAppLib.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AnalysisAppLib
{
    public interface IAppDbContext1
    {
        DbSet<AppTypeInfo> AppTypeInfos { get; set; }
        DbSet<AppClrType> AppClrType { get; set; }
        DbSet<ProjectInfo> Projects { get; set; }
        DbSet<SyntaxFieldInfo> SyntaxFieldInfo { get; set; }
        DbSet<LogInvocation2<string>> LogInvocation { get; set; }
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}