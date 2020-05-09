using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AnalysisAppLib.Syntax;
using JetBrains.Annotations;
using KayMcCormick.Dev;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class ProjectModel : INotifyPropertyChanged
    {
        public ProjectModel()
        {
            Documents.CollectionChanged += DocumentsOnCollectionChanged;
            PathModel docs = new PathModel(PathModelKind.Virtual);
            docs.ElementName = "Documents";
            RootPathInfo.Add(docs);
            DocumentsPath = docs;
            PathModel diag = new PathModel(PathModelKind.Virtual) {ElementName = "Diagnostics"};
            RootPathInfo.Add(diag);
        }

        public PathModel DocumentsPath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ProjectId Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<DocumentModel> Documents { get; } = new ObservableCollection<DocumentModel>();
        /// <summary>
        /// 
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SolutionModel Solution { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        
        private void DocumentsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddItems(e.NewItems);

                    break;
                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(e.OldItems);

                    break;
                case NotifyCollectionChangedAction.Replace:
                    RemoveItems(e.OldItems);
                    AddItems(e.NewItems);
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    RootPathInfo = new PathModel(PathModelKind.Directory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void RemoveItems(IList eOldItems)
        {
            foreach (DocumentModel eNewItem in eOldItems)
            {
                var filePath = eNewItem.FilePath;
                if (eNewItem.Project.FilePath != null)
                    filePath = Ext.GetRelativePath(eNewItem.Project.FilePath, filePath);
                var pathInfo = GetPathInfo(filePath, DocumentsPath);
                pathInfo.Entries.Remove(System.IO.Path.GetFileName(filePath));
                while (!pathInfo.Entries.Any())
                {
                    var n = pathInfo.ElementName;

                    pathInfo.Parent.Entries.Remove(n);
                    pathInfo = pathInfo.Parent;
                }
            }
        }

        private void AddItems(IList eNewItems)
        {
            foreach (DocumentModel eNewItem in eNewItems)
            {
                var filePath = eNewItem.FilePath;
                if (eNewItem.Project != null && eNewItem.Project.FilePath != null) filePath = Ext.GetRelativePath(eNewItem.Project.FilePath, filePath);
                var pathInfo = GetPathInfo(filePath, DocumentsPath);
                pathInfo.Entries[filePath] = new PathModel(PathModelKind.File)
                {
                    ElementName = System.IO.Path.GetFileName(filePath), Item = eNewItem, Parent = pathInfo,
                    Path = filePath
                };
            }
        }

        private PathModel GetPathInfo(string filePath, PathModel root)
        {
            var dirs = new Queue<string>(filePath.Split(System.IO.Path.DirectorySeparatorChar));
            var cur = root;
            var curPath = "";
            int pathi = 0;
            while (dirs.Count > 1)
            {
                var s = dirs.Dequeue();
                if (pathi == 0)
                {
                    DebugUtils.WriteLine("First path element is " + s);
                }

                curPath += s + System.IO.Path.DirectorySeparatorChar;
                
                if (cur.Entries.TryGetValue(s, out var val)) continue;
                val = new PathModel(PathModelKind.Directory) {Path = curPath, Parent = cur, ElementName = s};
                cur.Entries[s] = val;
                cur = val;
                pathi++;
            }

            return cur;
        }

        /// <summary>
        /// 
        /// </summary>
        public PathModel RootPathInfo { get; set; } = new PathModel(PathModelKind.Directory);
        /// <summary>
        /// 
        /// </summary>
        public List<WorkspaceDiagnostic> Diagnostics { get; set; } = new List<WorkspaceDiagnostic>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}