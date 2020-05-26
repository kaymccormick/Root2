using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using AnalysisControls.Converters;
using AnalysisControls.RibbonModel;
using AnalysisControls.ViewModel;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeContextualTab : RibbonModelTab
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
        public CodeContextualTab()
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

            var model = d.Compilation.GetSemanticModel(d.SyntaxTree);

            var nodePresenter = new RibbonModelTwoLineText();
            ribbonModelGroup.Items.Add(nodePresenter);

            var symbolPresenter = new RibbonModelPresenter();
            d.CodeControl.SetBinding(FormattedTextControl.HoverSymbolProperty,
                new Binding("Content") {Mode = BindingMode.OneWayToSource, Source = symbolPresenter});
            controlGroup.Items.Add(symbolPresenter);

            var errorMenu = new RibbonModelItemMenuButton() {Label = "Errors"};
            errorMenu.ItemContainerTemplateKey = "Diagnostic";

            controlGroup.Items.Add(errorMenu);
            
            errorMenu.Items = d.Compilation.GetDiagnostics().ToList();//.Where(d1 => d1.Severity >= DiagnosticSeverity.Error);
            // foreach (var diagnostic in d.Compilation.GetDiagnostics()
            // .Where(d1 => d1.Severity >= DiagnosticSeverity.Error))
            // errorMenu.ItemsCollection.Add(new RibbonModelAppMenuItem() {Header = diagnostic.GetMessage()});

            d.CodeControl.SetBinding(FormattedTextControl.HoverSyntaxNodeProperty,
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

            var methodsMenu = new RibbonModelItemMenuButton() {Label = "Methods"};
            methodsMenu.Items = info.Methods;
            info.MethodsMenu = methodsMenu;
            controlGroup.Items.Add(info.MethodsMenu);
            return info;
        }

        private CodeDocInfo CurrentInfo { get; set; }
    }
}