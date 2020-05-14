using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using AnalysisControls;
using JetBrains.Annotations;

namespace ProjTests
{
    public class AssemblyResourceModel : INotifyPropertyChanged
    {
        private INodeData _selectedNode;
        private Assembly _selectedAssembly;
        private ObservableCollection<Assembly> _assemblies = new ObservableCollection<Assembly>();

        public ObservableCollection<Assembly> Assemblies
        {
            get { return _assemblies; }
            set
            {
                if (Equals(value, _assemblies)) return;
                _assemblies = value;
                OnPropertyChanged();
            }
        }

        public Assembly SelectedAssembly
        {
            get { return _selectedAssembly; }
            set
            {
                if (Equals(value, _selectedAssembly)) return;
                _selectedAssembly = value;
                OnPropertyChanged();
            }
        }

        public INodeData SelectedNode
        {
            get { return _selectedNode; }
            set
            {
                if (Equals(value, _selectedNode)) return;
                _selectedNode = value;
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