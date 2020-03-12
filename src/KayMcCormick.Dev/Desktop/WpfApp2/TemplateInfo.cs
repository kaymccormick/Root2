#region header
// Kay McCormick (mccor)
// 
// WpfApp2
// WpfApp2
// TemplateInfo.cs
// 
// 2020-02-10-8:42 PM
// 
// ---
#endregion
using System.Windows ;
using System.Windows.Markup ;
using System.Xml ;
using KayMcCormick.Lib.Wpf ;

namespace WpfApp2
{
    public class TemplateInfo
    {
        private XmlViewer _xamlViewer ;
        private DataTemplate _template ;
        private string _templateXaml ;

        public object       Key          { get ; set ; }
        public DataTemplate Template
        {
            get => _template ;
            set
            {
                _template = value ;
                var xml = XamlWriter.Save(_template);
                var x = new XmlDocument();
                x.LoadXml(xml);
                TemplateXamlXml = x;
                _templateXaml = xml ;
            }
        }

        public string TemplateXaml
        {
            get => _templateXaml ;
            set
            {
                _templateXaml = value ;
                var template = XamlReader.Parse ( _templateXaml ) ;
                var doc = new XmlDocument();
                doc.LoadXml( _templateXaml ) ;
                TemplateXamlXml = doc ;
                _template = ( DataTemplate ) template ;
            }
        }

        public XmlDocument TemplateXamlXml { get ; set ; }

        
        public TemplateInfo ( ) {
        }

        public XmlViewer XamlViewer
        {
            get
            {
                return _xamlViewer
                       ?? ( _xamlViewer = new XmlViewer { XmlDocument = TemplateXamlXml } ) ;
            }
            set => _xamlViewer = value ;
        }

        
        public TemplateInfo ( object key , DataTemplate template )
        {
            Key          = key ;
            Template     = template ;
            TemplateXaml = XamlWriter.Save ( template ) ;
            var x = new XmlDocument();
            x.LoadXml ( TemplateXaml ) ;
            TemplateXamlXml = x ;
        }

        public TemplateInfo ( object key , string templateXaml )
        {
            Key = key ;
            TemplateXaml = templateXaml ;
            Template = ( DataTemplate ) XamlReader.Parse ( templateXaml ) ;
            var x = new XmlDocument();
            x.LoadXml(TemplateXaml);
            TemplateXamlXml = x;
        }
    }
}