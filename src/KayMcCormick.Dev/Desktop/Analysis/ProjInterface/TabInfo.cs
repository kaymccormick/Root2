using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AnalysisAppLib;
using JetBrains.Annotations;

namespace ProjInterface
{
    public class TabInfo :INotifyPropertyChanged
    {
        private Category _category;
        private ObservableCollection<GroupInfo> _groups = new ObservableCollection<GroupInfo>();

        public TabInfo()
        {
        }

        public Category Category
        {
            get => _category;
            set
            {
                if (value == _category) return;
                _category = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<GroupInfo> Groups
        {
            get => _groups;
            internal set
            {
                if (Equals(value, _groups)) return;
                _groups = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}