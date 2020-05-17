using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore.Internal;

namespace AnalysisControls.RibbonModel.Definition
{
    /// <summary>
    /// 
    /// </summary>
    [RibbonTabHeader("Assemblies")]
    public class AssembliesTypesGroup : RibbonModelGroup
    {
        public AssembliesTypesGroup(ITypesViewModel model)
        {
            var combo = CreateRibbonComboBox("Types");
            var combo1 = CreateRibbonComboBox("Types");

            var tgal = PrimaryRibbonModel.CreateGallery() as RibbonGallery;
            tgal.SelectionChanged += (sender, args) =>
            {
                //DebugUtils.WriteLine(args.NewValue.ToString());
                if (args.NewValue is RibbonGalleryItem item)
                {
                    if (item.Content is RibbonModelMenuItem item2)
                    {
                        combo1.Items.Clear();
                        var appTypeInfo= (AppTypeInfo) item2.CommandParameter;
                        var gal1 = TypeItems(appTypeInfo, model);
                        if (gal1 != null)
                        {
                            combo1.Items.Add(gal1);
                        }
                    }
                }

            };
            combo.Items.Add(tgal);
            var cat2 = PrimaryRibbonModel.CreateGalleryCategory(tgal, "Type");
            foreach (var appTypeInfo in model.GetAppTypeInfos())
            {
                var gal111 = TypeItems(appTypeInfo, model);
                PrimaryRibbonModel.CreateGalleryItem(cat2,
                    new RibbonModelMenuItem() {Header = appTypeInfo.Title, CommandParameter = appTypeInfo, IsEnabled = gal111 != null});
            }
            combo1.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "SelectionBoxItem")
                {
                    var appTypeInfo = combo1.SelectionBoxItem as AppTypeInfo;
                    object gal = null;
                    gal = TypeItems(appTypeInfo, model);
                    if (gal != null) combo.Items.Add(gal);
                }
            };

        }

        private static object TypeItems(AppTypeInfo appTypeInfo, ITypesViewModel model)
        {
            object gal = null;
            if (appTypeInfo.Fields.Any())
            {
                gal = PrimaryRibbonModel.CreateGallery();

                if (gal is RibbonGallery rg)
                {
                }

                var kindcat = PrimaryRibbonModel.CreateGalleryCategory(gal, "Kind");
                foreach (var kind in appTypeInfo.Kinds)
                {
                    PrimaryRibbonModel.CreateGalleryItem(kindcat, new RibbonModelMenuItem() {Header = kind.ToString()});
                }
                foreach (SyntaxFieldInfo field in appTypeInfo.Fields)
                {

                    switch (field.TypeName)
                    {

                        case "SyntaxToken":
                            var cat = PrimaryRibbonModel.CreateGalleryCategory(gal, field.Name);
                            foreach (var fieldKind in field.Kinds)
                            {
                                PrimaryRibbonModel.CreateGalleryItem(cat,
                                    new TextBlock {Text = fieldKind.ToString(), Height = 25});
                            }

                            break;
                        case "IdentifierNameSyntax":
                            var cat1 = PrimaryRibbonModel.CreateGalleryCategory(gal, field.Name);
                            PrimaryRibbonModel.CreateGalleryItem(cat1,
                                new RibbonModelItemTextBox() {Label = "Identifier"});
                            break;
                        default:
                            var cat3 = PrimaryRibbonModel.CreateGalleryCategory(gal, field.Name);
                            if (field.IsCollection)
                            {
                                var element = field.ElementTypeMetadataName;
                                var name = element.Substring(element.LastIndexOf('.') + 1);
                                var ati = model.GetAppTypeInfo(name);
                                // DebugUtils.WriteLine($"{ati}");
                                PrimaryRibbonModel.CreateGalleryItem(cat3, new RibbonModelMenuItem {Header = ati.Title});
                            } else
                            {
                                foreach (SyntaxKind fieldKind in field.Kinds)
                                {
                                    
                                    PrimaryRibbonModel.CreateGalleryItem(cat3, new TextBlock { Text = fieldKind.ToString() });
                                }
                            }
                            break;
                    }
                }
            }

            return gal;
        }

        private static void TypeItems(AppTypeInfo appTypeInfo, RibbonModelItemComboBox combo1)
        {
            if (appTypeInfo.Fields.Any())
            {
                var gal = PrimaryRibbonModel.CreateGallery();
                combo1.Items.Add(gal);
                if (gal is RibbonGallery rg)
                {
                }

                foreach (SyntaxFieldInfo field in appTypeInfo.Fields)
                {
                    switch (field.TypeName)
                    {
                        case "SyntaxToken":
                            var cat = PrimaryRibbonModel.CreateGalleryCategory(gal, field.Name);
                            foreach (var fieldKind in field.Kinds)
                            {
                                PrimaryRibbonModel.CreateGalleryItem(cat,
                                    new TextBlock {Text = fieldKind.ToString(), Height = 25});
                            }

                            break;
                        case "IdentifierNameSyntax":
                            var cat1 = PrimaryRibbonModel.CreateGalleryCategory(gal, field.Name);
                            PrimaryRibbonModel.CreateGalleryItem(cat1,
                                new RibbonModelItemTextBox() {Label = "Identifier"});
                            break;
                    }
                }
            }
        }
    }
}