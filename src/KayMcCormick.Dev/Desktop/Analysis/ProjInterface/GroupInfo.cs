using System.ComponentModel;
using System.Runtime.CompilerServices;
using AnalysisAppLib;
using JetBrains.Annotations;

namespace ProjInterface
{
    public class GroupInfo : INotifyPropertyChanged
    {
        private string _group;
        private Category _category;
        private CommandObservableCollection _commands = new CommandObservableCollection();

        public string Group
        {
            get => _group;
            set
            {
                if (value == _group) return;
                _group = value;
                OnPropertyChanged();
            }
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

        public CommandObservableCollection Commands
        {
            get => _commands;
            internal set
            {
                if (Equals(value, _commands)) return;
                _commands = value;
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