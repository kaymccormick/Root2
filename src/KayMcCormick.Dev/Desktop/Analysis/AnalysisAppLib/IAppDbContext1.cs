using System.Threading;
using System.Threading.Tasks;
using AnalysisAppLib.Project;
using AnalysisAppLib.Syntax;
using Microsoft.EntityFrameworkCore;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAppDbContext1
    {
        /// <summary>
        /// 
        /// </summary>
        DbSet<AppTypeInfo> AppTypeInfos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DbSet<AppClrType> AppClrType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DbSet<ProjectInfo> Projects { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DbSet<SyntaxFieldInfo> SyntaxFieldInfo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DbSet<LogInvocation2<string>> LogInvocation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int SaveChanges();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}