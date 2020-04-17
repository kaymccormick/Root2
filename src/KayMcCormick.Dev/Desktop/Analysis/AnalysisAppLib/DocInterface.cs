#region header
// Kay McCormick (mccor)
// 
// Analysis
// AnalysisAppLib
// DocInterface.cs
// 
// 2020-04-15-1:39 AM
// 
// ---
#endregion
using System ;
using System.Collections.Generic ;
using AnalysisAppLib.XmlDoc ;
using JetBrains.Annotations ;
using KayMcCormick.Dev ;

namespace AnalysisAppLib
{
    /// <summary>
    /// Implementation of <see cref="IDocInterface"/>
    /// </summary>
    public sealed class DocInterface : IDocInterface
    {

        private readonly Dictionary<Type, TypeDocInfo>
            // ReSharper disable once CollectionNeverUpdated.Local
            _docs = new Dictionary<Type, TypeDocInfo>();

        private DocumentCollection _documentCollection ;
        #region Implementation of IDocInterface
        /// <inheritdoc />
        public TypeDocInfo GetTypeDocumentation ( [ NotNull ] Type type )
        {
            return _docs[ type ] ;
        }

        /// <inheritdoc />
        public void CollectDoc([CanBeNull] CodeElementDocumentation docNode)
        {
            if (docNode == null)
            {
                return;
            }

            DebugUtils.WriteLine($"{docNode}");
            DocumentCollection.Add(docNode);
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public DocumentCollection DocumentCollection { get { return _documentCollection ; } set { _documentCollection = value ; } }
        #endregion
    }
}