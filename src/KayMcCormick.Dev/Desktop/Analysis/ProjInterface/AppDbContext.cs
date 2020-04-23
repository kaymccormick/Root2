#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// AppDbContext.cs
// 
// 2020-04-17-1:30 PM
// 
// ---
#endregion
using AnalysisAppLib ;
using AnalysisAppLib.Project ;
using AnalysisAppLib.Syntax ;
using KayMcCormick.Dev ;
using Microsoft.EntityFrameworkCore ;

namespace ProjInterface
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppTypeInfo> AppTypeInfos { get; set; }
        public DbSet<AppClrType>  AppClrType   { get; set; }
        public DbSet <ProjectInfo> Project { get ; set ; }
        public DbSet < ProjectLanguage> ProjectLanguage { get ; set ; }
        public DbSet<LogInvocation2<string>> LogInvocation { get; set; }
        #region Overrides of DbContext
        // ReSharper disable once AnnotateNotNullParameter
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\sql2017;Database=projinterface;Integrated security=true");
        }

        // ReSharper disable once AnnotateNotNullParameter
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppTypeInfo>().HasMany(t => t.Fields);
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                DebugUtils.WriteLine($"Entity name: {mutableEntityType.Name}");
            }
        }
        #endregion
    }
}