using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Linq;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using AnalysisControls.ViewModel;
using Autofac;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Attributes;
using KayMcCormick.Dev.Command;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class MiscCommands
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="unknown"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [NotNull]
        public List<AppTypeInfo> TypesViewModel_Stage1(
            
             out ITypesViewModel unknown
            , IAppDbContext1 db, AppDbContextHelper helper, Func<ITypesViewModel> factory, JsonSerializerOptions jsonSerializerOptions)
        {
            
            var typesViewModel = factory();
            //new TypesViewModel ( context.Scope.Resolve < JsonSerializerOptions > ( ) ) ;
            DebugUtils.WriteLine(
                $"InitializationDateTime : {typesViewModel.InitializationDateTime}"
            );
            typesViewModel.LoadTypeInfo();

            if (typesViewModel == null)
            {
                throw new ArgumentNullException(nameof(typesViewModel));
            }

            List<AppTypeInfo> r;

            {
                var appTypeInfos = typesViewModel.GetAppTypeInfos();
                var typeInfos = appTypeInfos as AppTypeInfo[] ?? appTypeInfos.ToArray();
                foreach (var appTypeInfo in typeInfos)
                {
                    DebugUtils.WriteLine(
                        $"Synchronizing {appTypeInfo.Title} ({appTypeInfo.Fields.Count})"
                    );

                    if (appTypeInfo.AppClrType != null)
                    {
                        continue;
                    }

                    
                    var clr = helper.FindOrAddClrType(db, appTypeInfo.Type);
                    appTypeInfo.AppClrType = clr;
                    if (appTypeInfo.Id != 0)
                    {
                        db.AppTypeInfos.Update(appTypeInfo);
                    }
                    else
                    {
                        db.AppTypeInfos.Add(appTypeInfo);
                    }
                }

                db.SaveChanges();
                r = db.AppTypeInfos.ToList();
            }

            WriteThisTypesViewModel(
                typesViewModel
                , model => Path.Combine(DataOutputPath, "model-v1.xaml")
            );
            DumpModelToJson(typesViewModel, jsonSerializerOptions, Path.Combine(DataOutputPath, "types-v1.json")
            );
            unknown = typesViewModel;
            return r;
        }

        private  void DumpModelToJson([NotNull] ITypesViewModel typesViewModel, JsonSerializerOptions jsonSerializerOptions, string jsonFilename = null
        )
        {
            using (var utf8Json = File.Open(jsonFilename, FileMode.Create))
            {
                var infos = typesViewModel.Map.Values.Cast<AppTypeInfo>().ToList();
                var writer = new Utf8JsonWriter(
                    utf8Json
                    , new JsonWriterOptions { Indented = true }
                );
                jsonSerializerOptions.WriteIndented = true;
                if (!jsonSerializerOptions
                    .Converters.Select(conv => conv.CanConvert(typeof(Type)))
                    .Any())

                {
                    throw new AppInvalidOperationException("no type converter");
                }

                foreach (var jsonConverter in jsonSerializerOptions.Converters)
                {
                    Console.WriteLine(jsonConverter);
                }

                try
                {
                    JsonSerializer.Serialize(writer, infos, jsonSerializerOptions);
                }
                catch (Exception)
                {
                    // ignored
                }

                writer.Flush();
            }
        }
        [NotNull]
        private static string ModelXamlFilename
        {
            get { return Path.Combine(DataOutputPath, ModelXamlFilenamePart); }
        }

        private void WriteThisTypesViewModel(
            [NotNull]   ITypesViewModel model
            , [CanBeNull] Func<ITypesViewModel, string> filenameFunc = null
        )
        {
            var xamlFilename = filenameFunc == null ? ModelXamlFilename : filenameFunc(model);

            DebugUtils.WriteLine($"Writing {xamlFilename}");
            var writer = XmlWriter.Create(
                xamlFilename
                , new XmlWriterSettings { Indent = true, Async = true }
            );
            foreach (var keyValuePair in model.Map.Dict)
            {
                model.Map2.Dict[keyValuePair.Key.StringValue] = keyValuePair.Value;
            }

            XamlWriter.Save(model, writer);
            writer.Close();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="context"></param>
        /// <param name="helper"></param>
        /// <param name="contextScope"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        [TitleMetadata("Build Types View")]
        [UsedImplicitly]
#pragma warning disable 1998
        public  async Task BuildTypeViewAsync(
#pragma warning restore 1998
            [NotNull] IBaseLibCommand command, IAppDbContext1 db, ILifetimeScope scope
            ,
            AppDbContextHelper helper, JsonSerializerOptions options)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

           

            DebugUtils.WriteLine("Begin initialize TypeViewModel");

            //var db = new AppDbContext ( ) ;
            {
                db.AppClrType.RemoveRange(db.AppClrType);
                db.AppTypeInfos.RemoveRange(db.AppTypeInfos);
                db.SyntaxFieldInfo.RemoveRange(db.SyntaxFieldInfo);
                await db.SaveChangesAsync();
            }

            {
                if (db.AppTypeInfos.Any()
                    || db.AppClrType.Any())
                {
                    throw new AppInvalidOperationException();
                }
            }

            var factory = scope.Resolve<Func<ITypesViewModel>>();
            var r = TypesViewModel_Stage1(out var typesViewModel, db, helper, factory, scope.Resolve<JsonSerializerOptions>());
            var t2 = new TypesViewModel(r);
            t2.BeginInit();
            t2.EndInit();
            var sts = scope.Resolve<ISyntaxTypesService>();
            var collectionMap = sts.CollectionMap();

            SyntaxTypesService.LoadSyntax(typesViewModel, collectionMap);
            // foreach ( AppTypeInfo ati in typesViewModel.Map.Values )
            // {
            // typesViewModel.PopulateFieldTypes ( ati ) ;
            // }

            typesViewModel.DetailFields();

            WriteModelToDatabase(typesViewModel, db, helper);
            WriteThisTypesViewModel(typesViewModel);
            DumpModelToJson(typesViewModel, options, Path.Combine(DataOutputPath, TypesJsonFilename)
            );
        }
        private const string ModelXamlFilenamePart = "model.xaml";
        private const string DataOutputPath = @"C:\data\logs";
        private const string TypesJsonFilename = "types.json";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typesViewModel"></param>
        /// <param name="db"></param>
        /// <param name="helper"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void WriteModelToDatabase(
            [NotNull] ITypesViewModel typesViewModel
            , IAppDbContext1 db, AppDbContextHelper helper
        )
        {
            
            if (typesViewModel == null)
            {
                throw new ArgumentNullException(nameof(typesViewModel));
            }

            {
                var appTypeInfos = typesViewModel.GetAppTypeInfos();
                var typeInfos = appTypeInfos as AppTypeInfo[] ?? appTypeInfos.ToArray();
                foreach (var appTypeInfo in typeInfos)
                {
                    DebugUtils.WriteLine(
                        $"Synchronizing {appTypeInfo.Title} ({appTypeInfo.Fields.Count})"
                    );
                    appTypeInfo.Version++;
                    var syntaxFieldCollection = appTypeInfo.Fields;
                    foreach (SyntaxFieldInfo o in syntaxFieldCollection)
                    {
                        db.SyntaxFieldInfo.Add(o);
                    }
                    // if ( appTypeInfo.AppClrType != null )
                    // {
                    // continue ;
                    // }


                    if (appTypeInfo.AppClrType == null)
                    {
                        
                        var clr = helper.FindOrAddClrType(db, appTypeInfo.Type);
                        appTypeInfo.AppClrType = clr;
                    }

                    continue;
                    if (appTypeInfo.Id != 0)
                    {
                        db.AppTypeInfos.Update(appTypeInfo);
                    }
                    else
                    {
                        db.AppTypeInfos.Add(appTypeInfo);
                    }
                }

                db.SaveChanges();
            }
        }

        public void PopulateSet (
            [ NotNull ] AppTypeInfoCollection subTypeInfos
            , ISet<Type> set
        )
        {
            foreach ( var rootSubTypeInfo in subTypeInfos )
            {
                if ( rootSubTypeInfo.Type.IsAbstract == false )
                {
                    set.Add ( rootSubTypeInfo.Type ) ;
                }

                PopulateSet ( rootSubTypeInfo.SubTypeInfos , set ) ;
            }
        }
    }
}