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
        private readonly Workspace _workspace;

        public DocumentModel(ProjectModel project, Workspace workspace)
        {
            _workspace = workspace;
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
        public Document Document
        {
            get
            {
                if (_workspace?.CurrentSolution != null)
                {
                    return _workspace.CurrentSolution.GetDocument(DocumentId);
                } else
                {

                }

                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DocumentId DocumentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<WorkspaceDiagnostic> Diagnostics { get; set; }  = new List<WorkspaceDiagnostic>();
    }
}