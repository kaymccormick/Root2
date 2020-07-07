using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using AnalysisControls.ViewModel;
using JetBrains.Annotations;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;
using RibbonLib.Model;

namespace AnalysisControls
{
    [UsedImplicitly]
    internal class ViewTabProvider : RibbonModelTabProvider1
    {
        private readonly IDocumentHost _docHost;
        private readonly IContentSelector _contentSel;
        private RibbonModelGroup _viewsGroup;
        private DocModel _docModel;
        readonly List<ViewSpec> _defaultViews = new List<ViewSpec>();

        public ViewTabProvider(Func<RibbonModelTab> factory, IDocumentHost docHost, IContentSelector contentSel) :
            base(factory)
        {
            _docHost = docHost;
            _contentSel = contentSel;

            _defaultViews.Add(new ViewSpec { ViewName = "View1" });
            _defaultViews.Add(new ViewSpec { ViewName = "List" });
            _defaultViews.Add(new ViewSpec { ViewName = "Details" });

        }

        public override RibbonModelTab ProvideModelItem()
        {
            var tab = Factory();
            tab.Header = "View";
            _viewsGroup = new RibbonModelGroup();
            tab.ItemsCollection.Add(_viewsGroup);
            var view1 = new RibbonModelRadioButton
            {
                Label = "View1",
                GroupName = "CurrentView",
                Command = new LambdaAppCommand("Change view", CommandFuncAsync, "View1").Command
            };
            _contentSel.PropertyChanged += ContentSelOnPropertyChanged;
            _viewsGroup.Items.Add(view1);
            return tab;
        }

        private Task<IAppCommandResult> CommandFuncAsync(LambdaAppCommand arg)
        {
            if (_docModel != null)
            {
                foreach (var docModelView in _docModel.Views)
                {
                    if (docModelView.ViewName.ToLowerInvariant() == ((string) arg.Argument).ToLowerInvariant())
                    {
                        _docModel.CurrentView = docModelView;
                    }
                }

            }
            return Task.FromResult(AppCommandResult.Success);

        }

        private void ContentSelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "ActiveContent") return;
            var doc = _contentSel.ActiveContent;
            if (_docModel != null) _docModel.PropertyChanged -= DocModelOnPropertyChanged;
            if (!(doc is DocModel docModel)) return;
            docModel.PropertyChanged += DocModelOnPropertyChanged;
            _docModel = docModel;
            foreach (var ribbonModelItem in _viewsGroup.Items)
            {
                if (!(ribbonModelItem is RibbonModelRadioButton rb)) continue;
                var found = false;
                if (docModel.Views != null)
                    foreach (var view in docModel.Views)
                    {
                        if (rb.Label.ToLowerInvariant() != view.ViewName.ToLowerInvariant()) continue;
                        found = true;
                        rb.IsEnabled = true;
                    }

                if (!found)
                {
                    rb.IsEnabled = false;
                }
            }

            if (docModel.Views != null)
            {
                foreach (var view in docModel.Views)
                {
                    var found = false;
                    foreach (var item in _viewsGroup.Items)
                    {
                        if (item is RibbonModelRadioButton rb)
                        {
                            if (item.Label.ToLowerInvariant() == view.ViewName.ToLowerInvariant())
                            {
                                found = true;
                            }
                        }
                    }
                    // foreach (var defaultView in _defaultViews)
                    // {
                    //     if (defaultView.ViewName.ToLowerInvariant() == view.ViewName.ToLowerInvariant())
                    //     {
                    //         found = true;
                    //         break;
                    //     }
                    // }

                    if (found) continue;
                    var ribbonModelRadioButton = new RibbonModelRadioButton
                    {
                        GroupName = "Views",
                        Label = view.ViewName,
                        LargeImageSource = view.LargeImageSource,
                        Command = new LambdaAppCommand("Change view", CommandFuncAsync, view.ViewName).Command
                    };
                    _viewsGroup.Items.Add(ribbonModelRadioButton);
                }
            }
            List<RibbonModelItem> removeViews = new List<RibbonModelItem>();
            foreach (var ribbonModelItem in _viewsGroup.Items)
            {
                if (ribbonModelItem is RibbonModelRadioButton rb)
                {
                    var found = false;
                    if (docModel.Views != null)
                        foreach (ViewSpec docModelView in docModel.Views)
                        {
                            if (docModelView.ViewName.ToLowerInvariant() == rb.Label.ToLowerInvariant())
                            {
                                found = true;
                            }
                        }

                    if (!found)
                    {
                        
                        removeViews.Add(ribbonModelItem);
                    }
                }
            }
            foreach (var ribbonModelItem in removeViews)
            {
                _viewsGroup.Items.Remove(ribbonModelItem);
            }


            if (docModel.CurrentView != null)
            {
                foreach (var ribbonModelItem in _viewsGroup.Items)
                {
                    if (ribbonModelItem is RibbonModelRadioButton rmrb)
                    {
                        if (rmrb.Label.ToLowerInvariant() == docModel.CurrentView.ViewName.ToLowerInvariant())
                        {
                            rmrb.IsChecked = true;
                        }

                    }
                }
            }
        }

        private void DocModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "CurrentView") return;
            var v = _docModel.CurrentView;
            foreach (var ribbonModelItem in _viewsGroup.Items)
            {
                if (ribbonModelItem is RibbonModelRadioButton rb)
                {
                    rb.IsChecked = rb.Label.ToLowerInvariant() == v.ViewName.ToLowerInvariant();
                }
            }
        }
    }
}