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
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.Settings;
using ProjLib.Properties ;

namespace ProjLib
{
    public class WorkspacesViewModel : IWorkspacesViewModel, ISupportInitialize
    {
        // private IList<IVsInstance> vsInstances;
        private readonly IVsInstanceCollector vsInstanceCollector;
        private readonly VisualStudioInstancesCollection _vsCollection = new VisualStudioInstancesCollection() ;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}