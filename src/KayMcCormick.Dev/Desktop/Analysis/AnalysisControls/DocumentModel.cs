using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class DocumentModel
    {
        public DocumentModel(ProjectModel project)
        {
            Project = project;
        }

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
        public ProjectModel Project { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Document Document { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<WorkspaceDiagnostic> Diagnostics { get; set; }  = new List<WorkspaceDiagnostic>();
    }
}