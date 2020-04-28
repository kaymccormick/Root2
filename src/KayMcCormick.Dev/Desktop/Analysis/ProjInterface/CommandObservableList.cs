using System.Collections.Generic;
using AnalysisAppLib;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ProjInterface
{
    public class CommandObservableCollection : ObservableCollection<CommandInfo>, IList<CommandInfo>
    {
        protected override void ClearItems()
        {
            base.ClearItems();
        }

        protected override void InsertItem(int index, CommandInfo item)
        {
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, CommandInfo item)
        {
            base.SetItem(index, item);
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);
        }

        public override event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add => base.CollectionChanged += value;
            remove => base.CollectionChanged -= value;
        }

        protected override event PropertyChangedEventHandler PropertyChanged
        {
            add => base.PropertyChanged += value;
            remove => base.PropertyChanged -= value;
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }
    }
}