#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// WpfApp2
// LogProperty.cs
// 
// 2020-02-10-6:55 PM
// 
// ---
#endregion
using System ;

namespace WpfApp2
{
    public class LogProperty
    {
        public string Name { get ; set ; }

        public string Header { get ; set ; }

        public string Description { get ; set ; }

        public string ExpectedType { get ; set ; }

        public string CellTemplateKey { get ; set ; }

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        public LogProperty ( string name , string header, string description, Type expectedType , string cellTemplateKey = null)
        {
            Name = name ;
            Header = header ;
            Description = description ;
            ExpectedType = expectedType.FullName ;
            CellTemplateKey = cellTemplateKey ;
        }
    }
}