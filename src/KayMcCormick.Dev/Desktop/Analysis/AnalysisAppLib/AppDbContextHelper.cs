using System;
using System.Linq;
using AnalysisAppLib.Syntax;
using JetBrains.Annotations;
using KayMcCormick.Dev;

namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public class AppDbContextHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [NotNull]
        public AppClrType FindOrAddClrType(IAppDbContext1 db, Type type)
        {
            DebugUtils.WriteLine($"Finding or adding clr type {type.AssemblyQualifiedName}");
            try
            {
                var clr = db.AppClrType.SingleOrDefault(
                              c => c.AssemblyQualifiedName
                                   == type.AssemblyQualifiedName
                          )
                          ?? AddClrType(db, type);
                return clr;
            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [NotNull]
        public  AppClrType AddClrType(IAppDbContext1 db, Type type)
        {
            DebugUtils.WriteLine($"Adding CLR type {type.FullName}");
            var appClrType = new AppClrType
            {
                AssemblyQualifiedName = type.AssemblyQualifiedName
                ,
                FullName = type.FullName
                ,
                IsAbstract = type.IsAbstract
                ,
                IsClass = type.IsClass
                ,
                IsConstructedGenericType = type.IsConstructedGenericType
                ,
                IsGenericType = type.IsGenericType
                ,
                IsGenericTypeDefinition = type.IsGenericTypeDefinition
            };
            if (type.BaseType != null)
            {
                appClrType.BaseType = FindOrAddClrType(db, type.BaseType);
            }

            db.AppClrType.Add(appClrType);
            db.SaveChanges();

            return appClrType;
        }
    }
}