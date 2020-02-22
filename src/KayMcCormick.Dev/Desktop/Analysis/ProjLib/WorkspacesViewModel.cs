#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// ProjLib
// WorkspacesViewModel.cs
// 
// 2020-02-19-7:26 AM
// 
// ---
#endregion
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq ;
using System.Runtime.CompilerServices;
using System.Threading.Tasks ;
using Microsoft.Build.Locator ;
using Microsoft.CodeAnalysis.MSBuild ;
using Microsoft.VisualStudio.Settings;
using ProjLib.Properties ;

namespace ProjLib
{
    public class WorkspacesViewModel : IWorkspacesViewModel, ISupportInitialize, INotifyPropertyChanged
    {
        // private IList<IVsInstance> vsInstances;
        private readonly IVsInstanceCollector vsInstanceCollector;
        private readonly VisualStudioInstancesCollection _vsCollection = new VisualStudioInstancesCollection() ;
        private MyProjectLoadProgress _currentProgress ;

        public MyProjectLoadProgress CurrentProgress
        {
            get => _currentProgress;
            set
            {
                _currentProgress = value;
                OnPropertyChanged ( nameof ( CurrentProgress ) ) ;
            }
        }

        public WorkspacesViewModel(IVsInstanceCollector collector)
        {
            vsInstanceCollector = collector;
            BeginInit();
        }

        /// <summary>Signals the object that initialization is starting.</summary>
        public void BeginInit()
        {
            var vsInstances = vsInstanceCollector.CollectVsInstances();
            foreach (var vsInstance in vsInstances)
            {
                VsCollection.Add(vsInstance);
            }
            OnPropertyChanged(nameof(VsCollection));

        }

        /// <summary>Signals the object that initialization is complete.</summary>
        public void EndInit ( )
        {

        }

        public VisualStudioInstancesCollection VsCollection
        {
            get
            {
                return _vsCollection;
            }
        }

        public async Task LoadSolutionAsync ( VsInstance vsSelectedItem , IMruItem sender2SelectedItem )
        {
            var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances();
            var i = visualStudioInstances
               .Single(
                       instance => instance.VisualStudioRootPath
                                   == vsSelectedItem.InstallationPath
                      ) ;
            ProjectHandler handler = new ProjectHandlerImpl(sender2SelectedItem.FilePath, i);
            handler.progressReporter = new MyProgress ( this ) ;
            await handler.LoadAsync ( ) ;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MyProjectLoadProgress
    {
        /// <summary>
        /// The project for which progress is being reported.
        /// </summary>
        public string FilePath { get; }

        public string FileName => Path.GetFileNameWithoutExtension ( FilePath ) ;

        /// <summary>
        /// The operation that has just completed.
        /// </summary>
        public string Operation { get; }

        /// <summary>
        /// The target framework of the project being built or resolved. This property is only valid for SDK-style projects
        /// during the <see cref="ProjectLoadOperation.Resolve"/> operation.
        /// </summary>
        public string TargetFramework { get; }

        /// <summary>
        /// The amount of time elapsed for this operation.
        /// </summary>
        public TimeSpan ElapsedTime { get; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public MyProjectLoadProgress ( string filePath , string operation , string targetFramework , TimeSpan elapsedTime )
        {
            FilePath = filePath ;
            Operation = operation ;
            TargetFramework = targetFramework ;
            ElapsedTime = elapsedTime ;
        }
    }

    public class MyProgress : IProgress < ProjectLoadProgress >
    {
        private readonly WorkspacesViewModel _workspacesViewModel ;

        public MyProgress ( WorkspacesViewModel workspacesViewModel )
        {
            _workspacesViewModel = workspacesViewModel ;
        }

        /// <summary>Reports a progress update.</summary>
        /// <param name="value">The value of the updated progress.</param>
        public void Report ( ProjectLoadProgress value )
        {
            _workspacesViewModel.CurrentProgress = new MyProjectLoadProgress(value.FilePath, value.Operation.ToString(), value.TargetFramework, value.ElapsedTime);
        }
    }
}