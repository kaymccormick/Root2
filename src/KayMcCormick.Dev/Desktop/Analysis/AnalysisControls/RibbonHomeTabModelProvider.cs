using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AnalysisAppLib;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Metadata;
using JetBrains.Annotations;
using KayMcCormick.Dev.Command;
using KayMcCormick.Lib.Wpf.Command;
using KmDevLib;
using RibbonLib.Model;

namespace AnalysisControls
{
    [UsedImplicitly]
    internal class RibbonHomeTabModelProvider : RibbonModelTabProvider1, ISubjectWatcher, IFontSettingsSource
    {
        private readonly MyReplaySubject<IMySubject> _subj;
        private readonly List<Meta<Lazy<IAppCommand>>> _commands;
        private RibbonModelGroup _g2;
        private RibbonModelItemMenuButton _subjectCombo;
        private RibbonModelGroup _g3;

        public RibbonHomeTabModelProvider(Func<RibbonModelTab> factory,
            [MetadataFilter("TabHeader", "Home")] IEnumerable<Meta<Lazy<IAppCommand>>> commands,
            MySubjectReplaySubject subj) : base(factory)
        {
            _subj = subj;

            _commands = commands.ToList();
        }

        /// <inheritdoc />
        public override RibbonModelTab ProvideModelItem()
        {
            var r = Factory();
            r.Header = "Home";
            var g = new RibbonModelGroup();
            r.ItemsCollection.Add(g);
            g.Label = "Commands";
            foreach (var command in _commands)
            {
                var b = new RibbonModelButton();
                var p = MetaHelper.GetMetadataProps(command.Metadata);
                b.Label = p.Title ?? "None";
                b.Command = command.Value.Value.Command;
                g.Items.Add(b);
            }

            _g3 = new RibbonModelGroup();
            r.ItemsCollection.Add(_g3);
            _g3.Items.Add(new RibbonModelItemFontComboBox() {Label = "Font: "});
            var ribbonModelItemComboBox = new RibbonModelItemComboBox
                {Label = "Font Size: ", Items = new double[] {8, 10, 12, 14, 18, 20}};
            _g3.Items.Add(ribbonModelItemComboBox);
            _g2 = new RibbonModelGroup();
            r.ItemsCollection.Add(_g2);
            _subjectCombo = new RibbonModelItemMenuButton();
            _g2.Items.Add(_subjectCombo);
            _subj.Subject.Subscribe(x => Subject(x));
            return r;
        }


        /// <inheritdoc />
        public void Subject(IMySubject x)
        {
            var displayName = x.Title;
            var xx = new LambdaAppCommand(displayName, (command, o) => Task.FromResult(AppCommandResult.Success), null);
            var xxx = new RibbonModelMenuItem() {Header = displayName, Command = xx.Command};
            _subjectCombo.ItemsCollection.Add(xxx);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public interface IFontSettingsSource : INotifyPropertyChanged


    {
    }
}