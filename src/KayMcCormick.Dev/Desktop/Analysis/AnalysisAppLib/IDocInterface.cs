#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// AnalysisAppLib
// IDocInterface.cs
// 
// 2020-04-13-4:47 AM
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
    /// 
    /// </summary>
    public interface IDocInterface
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        TypeDocInfo GetTypeDocumentation ( Type type ) ;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodDocumentation"></param>
        void CollectDoc ( CodeElementDocumentation methodDocumentation ) ;
    }

    /// <summary>
    /// 
    /// </summary>
    public class DocInterface : IDocInterface
    {

        private readonly Dictionary<Type, TypeDocInfo>
            _docs = new Dictionary<Type, TypeDocInfo>();

        private DocumentCollection _documentCollection ;
        #region Implementation of IDocInterface
        /// <inheritdoc />
        public TypeDocInfo GetTypeDocumentation ( Type type )
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

        public DocumentCollection DocumentCollection { get { return _documentCollection ; } set { _documentCollection = value ; } }
        #endregion
    }
}