#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ConsoleApp1
// AppDbContext.cs
// 
// 2020-04-17-10:43 AM
// 
// ---
#endregion
using AnalysisAppLib.Syntax ;
using KayMcCormick.Dev ;
using Microsoft.EntityFrameworkCore ;

namespace ConsoleApp1
{
    public class AppDbContext : DbContext
    {
        public DbSet<AppTypeInfo> AppTypeInfos { get; set; }
        public DbSet <AppClrType> AppClrType { get ; set ; }
        #region Overrides of DbContext
        // ReSharper disable once AnnotateNotNullParameter
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\sql2017;Database=projinterface;Integrated security=true");
        }

        // ReSharper disable once AnnotateNotNullParameter
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var mutableEntityType in modelBuilder.Model.GetEntityTypes())
            {
                DebugUtils.WriteLine(mutableEntityType.Name);
            }
        }
        #endregion
    }
}