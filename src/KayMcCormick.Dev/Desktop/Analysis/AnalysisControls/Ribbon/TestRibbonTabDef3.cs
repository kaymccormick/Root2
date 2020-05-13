using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using AnalysisAppLib;
using AnalysisAppLib.Syntax;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore.Internal;

namespace AnalysisControls.RibbonM
{
    public class TestRibbonTabDef3 : RibbonModelTab
    {
        
        public TestRibbonTabDef3(IEnumerable<Meta<RibbonModelGroup>> groups)
        {
            Visibility = Visibility.Visible;
            ContextualTabGroupHeader = "Assemblies";
            Header = "Assemblies";
            foreach (var ribbonModelGroup in groups)
            {
                var props = MetaHelper.GetMetadataProps(ribbonModelGroup.Metadata);
                DebugUtils.WriteLine($"{props.TabHeader} {ContextualTabGroupHeader}");
                if (props.TabHeader != null && props.TabHeader.Equals((string)Header))
                {
                    Items.Add(ribbonModelGroup.Value);
                }
            }
        }
    }

    [RibbonTabHeader("Assemblies")]
    public class RibbonModelGroup1 : RibbonModelGroup
    {
        public RibbonModelGroup1(ITypesViewModel model)
        {
            var combo = CreateRibbonComboBox("Types");
            var combo1 = CreateRibbonComboBox("Types");

            var tgal = RibbonModel.CreateGallery() as RibbonGallery;
            tgal.SelectionChanged += (sender, args) =>
            {
                DebugUtils.WriteLine(args.NewValue.ToString());
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
            var cat2 = RibbonModel.CreateGalleryCategory(tgal, "Type");
            foreach (var appTypeInfo in model.GetAppTypeInfos())
            {
                var gal111 = TypeItems(appTypeInfo, model);
                RibbonModel.CreateGalleryItem(cat2,
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
                gal = RibbonModel.CreateGallery();

                if (gal is RibbonGallery rg)
                {
                }

                var kindcat = RibbonModel.CreateGalleryCategory(gal, "Kind");
                foreach (var kind in appTypeInfo.Kinds)
                {
                    RibbonModel.CreateGalleryItem(kindcat, new RibbonModelMenuItem() {Header = kind.ToString()});
                }
                foreach (SyntaxFieldInfo field in appTypeInfo.Fields)
                {

                    switch (field.TypeName)
                    {

                        case "SyntaxToken":
                            var cat = RibbonModel.CreateGalleryCategory(gal, field.Name);
                            foreach (var fieldKind in field.Kinds)
                            {
                                RibbonModel.CreateGalleryItem(cat,
                                    new TextBlock {Text = fieldKind.ToString(), Height = 25});
                            }

                            break;
                        case "IdentifierNameSyntax":
                            var cat1 = RibbonModel.CreateGalleryCategory(gal, field.Name);
                            RibbonModel.CreateGalleryItem(cat1,
                                new RibbonModelItemTextBox() {Label = "Identifier"});
                            break;
                        default:
                            var cat3 = RibbonModel.CreateGalleryCategory(gal, field.Name);
                            if (field.IsCollection)
                            {
                                var element = field.ElementTypeMetadataName;
                                var name = element.Substring(element.LastIndexOf('.') + 1);
                                var ati = model.GetAppTypeInfo(name);
                                DebugUtils.WriteLine($"{ati}");
                                RibbonModel.CreateGalleryItem(cat3, new RibbonModelMenuItem {Header = ati.Title});
                            } else
                            {
                                foreach (SyntaxKind fieldKind in field.Kinds)
                                {
                                    
                                    RibbonModel.CreateGalleryItem(cat3, new TextBlock { Text = fieldKind.ToString() });
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
                var gal = RibbonModel.CreateGallery();
                combo1.Items.Add(gal);
                if (gal is RibbonGallery rg)
                {
                }

                foreach (SyntaxFieldInfo field in appTypeInfo.Fields)
                {
                    switch (field.TypeName)
                    {
                        case "SyntaxToken":
                            var cat = RibbonModel.CreateGalleryCategory(gal, field.Name);
                            foreach (var fieldKind in field.Kinds)
                            {
                                RibbonModel.CreateGalleryItem(cat,
                                    new TextBlock {Text = fieldKind.ToString(), Height = 25});
                            }

                            break;
                        case "IdentifierNameSyntax":
                            var cat1 = RibbonModel.CreateGalleryCategory(gal, field.Name);
                            RibbonModel.CreateGalleryItem(cat1,
                                new RibbonModelItemTextBox() {Label = "Identifier"});
                            break;
                    }
                }
            }
        }
    }

    [MetadataAttribute]
    public class RibbonTabHeaderAttribute : Attribute
    {
        private readonly string _tabHeader;

        public RibbonTabHeaderAttribute(string tabHeader)
        {
            _tabHeader = tabHeader;
        }

        public string TabHeader
        {
            get { return _tabHeader; }
        }
    }

    public class ContextualTabGroup1 : RibbonModelContextualTabGroup
    {
        public ContextualTabGroup1()
        {
            Header = "Code Analysis";
        }
    }
}