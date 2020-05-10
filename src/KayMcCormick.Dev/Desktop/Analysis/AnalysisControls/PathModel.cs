using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class PathModel : INotifyPropertyChanged
    {
        private ObservableCollection<PathModel> _children = new ObservableCollection<PathModel>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="kind"></param>
        public PathModel(PathModelKind kind)
        {
            Kind = kind;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsVirtualNode { get; set; }

        /// <summary>
        ///
        /// 
        /// </summary>
        public IDictionary<string, PathModel> Entries { get; } = new Dictionary<string, PathModel>();
        /// <summary>
        /// 
        /// </summary>
        public string Path { get; set; }
        public PathModelKind Kind { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object Item { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public PathModel Parent { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ElementName { get; set; }

        public virtual IEnumerable Children => _children;
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Entries)}: {Entries.Keys.Join(";")}, {nameof(Path)}: {Path}, {nameof(Item)}: {Item}, {nameof(Parent)}: {Parent}, {nameof(ElementName)}: {ElementName}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docs"></param>
        public virtual void Add(PathModel docs)
        {
            Entries[docs.ElementName] = docs;
            _children.Add(docs);
        }

        public virtual bool Remove(PathModel p)
        {
            Entries.Remove(p.ElementName);
            _children.Remove(p);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum PathModelKind  
    {
        None = 0,
        Directory ,
        File,
        Virtual,
        Diagnostic
    }
}