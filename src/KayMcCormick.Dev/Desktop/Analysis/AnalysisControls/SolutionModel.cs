using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class SolutionModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workspace"></param>
        public SolutionModel(Workspace workspace)
        {
            Workspace = workspace;
        }

        public Workspace Workspace { get; set; }
        public SolutionId Id { get; set; }
        public string FilePath { get; set; }
        public ObservableCollection<ProjectModel> Projects { get; set; } = new ObservableCollection<ProjectModel>();
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}