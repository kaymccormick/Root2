using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Internal;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class PathModel
    {
        public PathModel(PathModelKind kind)
        {
            Kind = kind;
        }

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Entries)}: {Entries.Keys.Join(";")}, {nameof(Path)}: {Path}, {nameof(Item)}: {Item}, {nameof(Parent)}: {Parent}, {nameof(ElementName)}: {ElementName}";
        }
    }

    public enum PathModelKind  
    {
        None = 0,
        Directory ,
        File
    }
}