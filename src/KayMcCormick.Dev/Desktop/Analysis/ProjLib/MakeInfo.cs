#region header
// Kay McCormick (mccor)
// 
// KayMcCormick.Dev
// ProjInterface
// MakeInfo.cs
// 
// 2020-02-27-3:27 AM
// 
// ---
#endregion
using System ;
using System.Windows.Controls ;
using System.Windows.Markup ;
using AnalysisFramework ;


namespace ProjLib
{
    public class MakeInfo
    {
        private CodeAnalyseContext.Factory1 _makeFunc ;
        private CodeAnalyseContext _context ;

        public MakeInfo ( ContentControl w , FormattedCode control , string sourceText = null, IAddChild addChild = null, CodeAnalyseContext.Factory1 makeFunc = null )
        {
            MakeFunc = makeFunc ;
            W          = w ;
            Control    = control ;
            SourceText = sourceText ;
            AddChild   = addChild ;
        }

        public MakeInfo ( FormattedCode fmt , string transformSourceCode )
        {
            SourceText = transformSourceCode ;
            Control = fmt ;
        }

        public ContentControl W { get ; private set ; }

        public ProjLib.FormattedCode Control { get ; private set ; }

        public string SourceText { get ; private set ; }

        public IAddChild AddChild { get ; private set ; }

        public CodeAnalyseContext.Factory1 MakeFunc
        {
            get => _makeFunc ;
            set => _makeFunc = value ;
        }

        public CodeAnalyseContext Context { get { return _context ; } set { _context = value ; } }
    }
}