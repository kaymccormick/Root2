﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Input;
using System.Windows.Media;
using AnalysisAppLib;
using Autofac.Features.Metadata;
using AvalonDock.Layout;
using KayMcCormick.Dev;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls
{
    public class AllCommands : IRibbonComponent
    {
        private IEnumerable<Meta<IAppCommand>> _commands;
        private readonly IEnumerable<IDisplayableAppCommand> _dispCmds;
        private readonly ReplaySubject<DocContent> _docContent;
        private readonly IEnumerable<Func<LayoutDocumentPane, IDisplayableAppCommand>> _funcs;
        private List<Meta<IAppCommand>> _cmds;

        public AllCommands(IEnumerable<Meta<IAppCommand>> commands, IEnumerable<IDisplayableAppCommand> dispCmds,
            ReplaySubject<DocContent> docContent)
        {
            _commands = commands;
            _dispCmds = dispCmds;
            _docContent = docContent;
            _cmds = _commands.ToList();
            
        
        }

        public LayoutDocumentPane DocPane { get; set; }

        public object GetComponent()
        {
            RibbonGroup myGroup = new RibbonGroup();
            var x = new RibbonTextBox();
            x.PreviewKeyDown += (sender, args) =>
            {
                if (args.Key == Key.Enter)
                {
                    var type = Type.GetType(x.Text);
                    if (type != null)
                    {
                        DebugUtils.WriteLine(type.ToString());
                        DocContent doc = new DocContent() {Content = type};
                        _docContent.OnNext(doc);
                    }
                }
            };
            myGroup.Items.Add(x);
            RibbonGallery gallery = new RibbonGallery();
            var galCat = new RibbonGalleryCategory();
            galCat.MaxColumnCount = 1;
            galCat.MinColumnCount = 1;
            List<RibbonGalleryItem> items = new List<RibbonGalleryItem>();
            foreach (var baseLibCommand in _commands)
            {
                var ribbonGalleryItem = new RibbonGalleryItem();
                var ribbonButton = new RibbonButton();
                var props = MetaHelper.GetMetadataProps(baseLibCommand.Metadata);

                var ribbonButtonContent = props.Title ?? "no title";
                TextBlock bl = new TextBlock() {Text = ribbonButtonContent};
                ribbonButton.Content = bl;
                ribbonButton.Command = baseLibCommand.Value.Command;
                ribbonGalleryItem.Content = ribbonButtonContent;
                ribbonGalleryItem.Tag = baseLibCommand.Value;
                items.Add(ribbonGalleryItem);
            }
            foreach (var cmd1 in _dispCmds)
            {
                var ribbonGalleryItem = new RibbonGalleryItem();
                var ribbonButtonContent = cmd1.DisplayName;
                ribbonGalleryItem.Content = ribbonButtonContent;
                ribbonGalleryItem.Tag = cmd1;
                items.Add(ribbonGalleryItem);
            }   

            if (_funcs != null)
                foreach (var func in _funcs)
                {
                    var ribbonGalleryItem = new RibbonGalleryItem();
                    var cmd = func(DocPane);
                    ribbonGalleryItem.Content = cmd.DisplayName;
                    ribbonGalleryItem.Tag = cmd;
                    items.Add(ribbonGalleryItem);
                }

            galCat.ItemsSource = items;
            gallery.ItemsSource = new[]{galCat};

            var ribbonComboBox = new RibbonComboBox();
            ribbonComboBox.Items.Add(gallery);
            var statusContent = new Border() { Width = 15, Height = 15, Background = Brushes.Gray };
            gallery.Command = new LambdaAppCommand("", async command =>
            {
                RibbonGalleryItem g = (RibbonGalleryItem) gallery.SelectedItem;
                //var item = (RibbonGalleryItem)ribbonComboBox.SelectionBoxItem;
                IAppCommand cmd = (IAppCommand) g.Tag;
                statusContent.Background = Brushes.Green;
                var result = await cmd.ExecuteAsync();
                if (result.IsSuccess)
                    {
                        statusContent.Background = Brushes.Blue;
                    }
                    else
                    {
                        statusContent.Background = Brushes.Red;
                    }

                    return result;

            }, null);
            RibbonControl status = new RibbonControl();
            
            status.Content = statusContent;
            myGroup.Items.Add(status);
            myGroup.Items.Add(ribbonComboBox);
            return myGroup;
        }
    }
}