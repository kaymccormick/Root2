using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AnalysisAppLib;
using Autofac.Features.Metadata;
using AvalonDock.Layout;
using KayMcCormick.Dev;
using KayMcCormick.Lib.Wpf.Command;

namespace AnalysisControls{

    /// <summary>
    /// 
    /// </summary>
    public class AllCommands 
    {
        private readonly IEnumerable<Meta<Lazy<IBaseLibCommand>>> _commands;
        private readonly IEnumerable<IDisplayableAppCommand> _displayCommands;
        private readonly IEnumerable<Func<LayoutDocumentPane, IDisplayableAppCommand>> _funcs = null;
        private List<Meta<Lazy<IBaseLibCommand>>> _cmds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commands"></param>
        /// <param name="displayCommands"></param>
        public AllCommands(IEnumerable<Meta<Lazy<IBaseLibCommand>>> commands, IEnumerable<IDisplayableAppCommand> displayCommands)
        {
            _commands = commands;
            _displayCommands = displayCommands;
            
            _cmds = _commands.ToList();
            
        }

        /// <summary>
        /// 
        /// </summary>
        public LayoutDocumentPane DocPane { get; set; }

        public IEnumerable<Meta<Lazy<IBaseLibCommand>>> Commands1
        {
            get { return _commands; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object GetComponent()
        {
            var myGroup = new RibbonGroup();
            var x = new RibbonTextBox();
            // x.PreviewKeyDown += (sender, args) =>
            // {
            //     if (args.Key == Key.Enter)
            //     {
            //         var type = Type.GetType(x.Text);
            //         if (type != null)
            //         {
            //             DebugUtils.WriteLine(type.ToString());
            //             DocContent doc = new DocContent() {Content = type};
            //             _docContent.OnNext(doc);
            //         }
            //     }
            // };
            myGroup.Items.Add(x);
            var gallery = new RibbonGallery();
            var galCat = new RibbonGalleryCategory { MaxColumnCount = 1, MinColumnCount = 1 };
            var galCat2 = new RibbonGalleryCategory { MaxColumnCount = 1, MinColumnCount = 1,DisplayMemberPath = "Title"};
            var items = new List<RibbonGalleryItem>();
            var items2 = new List<object>();
            foreach (var baseLibCommand in _commands)
            {
                var ribbonGalleryItem = new RibbonGalleryItem();
                var props = MetaHelper.GetMetadataProps(baseLibCommand.Metadata);
                var ribbonButtonContent = props.Title ?? "no title";
                ribbonGalleryItem.Content = ribbonButtonContent;
                ribbonGalleryItem.Tag = baseLibCommand.Value;
                items.Add(ribbonGalleryItem);
                var commandInfo = new CommandInfo() {Command = baseLibCommand};
                DebugUtils.WriteLine(commandInfo.TheCommand.GetType().FullName);
                DebugUtils.WriteLine(commandInfo.Title);
                items2.Add(commandInfo);
            }
            foreach (var cmd1 in _displayCommands)
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
            galCat2.ItemsSource = items2;
            var gallery2 = new RibbonGallery();
            gallery2.ItemsSource = new[] { galCat2 };
            var split = new RibbonSplitButton() { Label = "Command", LabelPosition = RibbonSplitButtonLabelPosition.DropDown };
            //split.Items.Add(gallery2);

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
            var status = new RibbonControl {Content = statusContent};

            myGroup.Items.Add(status);
            //myGroup.Items.Add(split);
            var combo2 = new RibbonComboBox();
            combo2.Label = "Command Info";
            combo2.Items.Add(gallery2);
            var group2 = new RibbonGroup();
            var g2 = new RibbonControlGroup();
            group2.Items.Add(g2);
            var comboBox = new RibbonComboBox();
            //comboBox.Items.Add(gallery2);
            comboBox.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("SelectedItem.Metadata")
            {
                Source = gallery2
            });
            g2.Items.Add(comboBox);

            myGroup.Items.Add(ribbonComboBox);
            myGroup.Items.Add(combo2);

            return new[] {myGroup, group2};
        }
    }
}