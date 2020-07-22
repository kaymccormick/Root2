using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using AnalysisControls.Converters;
using AnalysisControls.ViewModel;
using Microsoft.CodeAnalysis;
using RibbonLib;
using RibbonLib.Model;
using RoslynCodeControls;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeContextualTab2 : RibbonModelTab
    {
        private readonly RibbonModelControlGroup cgroup = new RibbonModelControlGroup();

        private readonly Dictionary<object, CodeDocInfo> DocDict = new Dictionary<object, CodeDocInfo>();
        private IRibbonModelGroup _group = new RibbonModelGroup();

        /// <summary>
        /// 
        /// </summary>
        private class CodeDocInfo
        {
            public ObservableCollection<object> Methods { get; set; }
            public RibbonModelItemMenuButton MethodsMenu { get; set; }
            public RibbonModelControlGroup ControlGroup { get; set; }
        }

        /// <inheritdoc />
        public override object Header
        {
            get { return "Code"; }
        }

        /// <inheritdoc />
        public CodeContextualTab2()
        {
            ItemsCollection.Add(_group);
        }

        /// <inheritdoc />
        public override void OnActiveContentChanged(object sender, ActiveContentChangedEventArgs e)
        {
            base.OnActiveContentChanged(sender, e);
            if (CurrentInfo?.ControlGroup != null) CurrentInfo.ControlGroup.Visibility = Visibility.Collapsed;

            if (e.ActiveContent is CodeDocument d)
            {
                if (!DocDict.TryGetValue(d, out var info))
                {
                    info = CodeDocInfo1(_group, d);
                    DocDict[d] = info;
                }

                CurrentInfo = info;
                info.ControlGroup.Visibility = Visibility.Visible;
            }
        }

        private static CodeDocInfo CodeDocInfo1(IRibbonModelGroup ribbonModelGroup, CodeDocument d)
        {
            var info = new CodeDocInfo();

            var controlGroup = new RibbonModelControlGroup();
            info.ControlGroup = controlGroup;
            ribbonModelGroup.Items.Add(controlGroup);

            if (d.Compilation != null)
            {
                var model = d.Compilation.GetSemanticModel(d.SyntaxTree);
                d.CodeControl.SetValue(RoslynCodeControls.RoslynProperties.SemanticModelProperty,model);
                var nodePresenter = new RibbonModelTwoLineText();
                ribbonModelGroup.Items.Add(nodePresenter);

                
                var symbolPresenter = new RibbonModelPresenter();
                d.CodeControl.SetBinding(RoslynCodeControl.HoverSymbolProperty,
                    new Binding("Content") {Mode = BindingMode.OneWayToSource, Source = symbolPresenter});
                controlGroup.Items.Add(symbolPresenter);

                var errorMenu = new RibbonModelItemMenuButton {Label = "Errors", ItemContainerTemplateKey = "Diagnostic"};

                controlGroup.Items.Add(errorMenu);
            
                errorMenu.Items = d.Compilation.GetDiagnostics().ToList();//.Where(d1 => d1.Severity >= DiagnosticSeverity.Error);
                // foreach (var diagnostic in d.Compilation.GetDiagnostics()
                // .Where(d1 => d1.Severity >= DiagnosticSeverity.Error))
                // errorMenu.ItemsCollection.Add(new RibbonModelAppMenuItem() {Header = diagnostic.GetMessage()});

                d.CodeControl.SetBinding(RoslynCodeControl.HoverSyntaxNodeProperty,
                    new Binding("Text")
                    {
                        Mode = BindingMode.OneWayToSource, Source = nodePresenter, ConverterParameter = SyntaxNodeInfo.Kind,
                        Converter = new SyntaxNodeConverter { }
                    });

                info.Methods = new ObservableCollection<object>();
                foreach (var methodSymbol in d.Compilation
                    .GetSymbolsWithName(s => true, SymbolFilter.Member)
                    .Where(z => z.Kind == SymbolKind.Method).OfType<IMethodSymbol>())
                {
                    var item = new RibbonModelMenuItem() {Header = methodSymbol};
                    info.Methods.Add(item);
                }

                var item2  = new RibbonModelItemMenuButton();
                item2.Label = "root";
                var xx = new MySymbolVisitor(item2);
                xx.DefaultVisit(model.Compilation.Assembly);
                controlGroup.Items.Add(item2);
                var menuItem = new RibbonModelItemMenuButton() {Label = "Namespace"};
                menuItem.Label =
                    model.Compilation.GlobalNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                try
                {
                    Add(menuItem, model.Compilation.GlobalNamespace, model.Compilation.Assembly.Identity);
                } catch{}

                var combobox = new RibbonModelItemComboBox();
                foreach (var value in Enum.GetValues(typeof(OutputKind)))
                {
                    combobox.ItemsCollection.Add(new RibbonModelMenuItem(){Header=value});
                }
                
            
    
                combobox.SelectionBoxItem = model.Compilation.Options.OutputKind;
                combobox.Label = "Output Kind";
            
                controlGroup.Items.Add(combobox);
                controlGroup.Items.Add(menuItem);
            }

            //            ((CSharpCompilation)model.Compilation).Options.OutputKind.

            var label1 = new RibbonModelTwoLineText();
            d.CodeControl.SetBinding(RoslynCodeControl.HoverSymbolProperty,
                new Binding("Text") {Source = label1, Mode = BindingMode.OneWayToSource});

            controlGroup.Items.Add(label1);
            var methodsMenu = new RibbonModelItemMenuButton {Label = "Methods", Items = info.Methods};
            info.MethodsMenu = methodsMenu;
            controlGroup.Items.Add(info.MethodsMenu);
            return info;
        }

        private static void Add(IHasMenuItems menuItem, ISymbol symbol, AssemblyIdentity a)
        {
            switch (symbol)
            {
                case INamespaceOrTypeSymbol t:
                {
                    //AssemblyIdentityComparer.
                    //if (t.ContainingAssembly.Identity.Equals(a))
                    {
                        foreach (var member in t.GetMembers())
                        {
                            if (!member.CanBeReferencedByName)
                                return;

                            var item = new RibbonModelMenuItem();
                            item.Header = member.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                            Add(item, member, a);
                            menuItem.AddItem(item);
                        }
                    }

                    break;
                }
            }
        }

        private CodeDocInfo CurrentInfo { get; set; }
    }
}