using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Xaml;
using System.Xml;
using KayMcCormick.Dev;
using XamlReader = System.Windows.Markup.XamlReader;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public static class XamlFuncs
    {
        public static void ParseXaml(string f, string s = null)
        {
            Stack<string> elems = new Stack<string>();
            var httpSchemasMicrosoftComWinfxXaml = "http://schemas.microsoft.com/winfx/2006/xaml";
            if (s == null)
            {
                s = File.ReadAllText(f);
            }

            using (var r = new StringReader(s))
            {
                {
                    var xmlNameTable = new NameTable();

                    var xmlReaderSettings = new XmlReaderSettings() {NameTable = xmlNameTable};
                    XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlNameTable);
                    XmlParserContext ctx = new XmlParserContext(xmlNameTable, nsMgr, "en-US", XmlSpace.Default);
                    XmlReader xmlReader = XmlReader.Create(r, xmlReaderSettings, ctx);
                    string ns = null;
                    string class_ = null;
                    bool done = false;
                    while (xmlReader.Read())
                    {


                        switch (xmlReader.NodeType)
                        {
                            case XmlNodeType.Element:
                                elems.Push(xmlReader.Name);
                                if (elems.Count == 1)
                                {
                                    class_ = xmlReader.GetAttribute("Class", httpSchemasMicrosoftComWinfxXaml);
                                    var local = xmlReader.GetAttribute("local", "http://www.w3.org/2000/xmlns/");
                                    // var num = xmlReader.AttributeCount;
                                    // while (xmlReader.MoveToNextAttribute())
                                    // {
                                    // DebugUtils.WriteLine(xmlReader.NamespaceURI);
                                    // DebugUtils.WriteLine(xmlReader.LocalName);
                                    // DebugUtils.WriteLine(xmlReader.Prefix);
                                    // DebugUtils.WriteLine(xmlReader.Name);
                                    // DebugUtils.WriteLine(xmlReader.Value);

                                    // }

                                    if (local != null)
                                    {
                                        var clrNamespace = "clr-namespace:";
                                        if (local.StartsWith(clrNamespace))
                                        {
                                            ns = local.Substring(clrNamespace.Length);
                                        }
                                    }

                                    done = true;
                                }

                                break;
                        }

                        if (done)
                            break;
                    }

                    object inst = null;
                    if (class_ != null)
                    {
                        var t = Type.GetType(class_);
                        if (t == null)
                        {
                            var ts = AppDomain.CurrentDomain.GetAssemblies()
                                .SelectMany(a => a.GetExportedTypes().Where(tt => tt.FullName == class_));
                            t = ts.FirstOrDefault();
                        }

                        inst = Activator.CreateInstance(t);
                    }

                    using (var r2 = new StringReader(s))
                    {
                        xmlReader = XmlReader.Create(r2);
                        XamlObjectWriterSettings writerSettings = new XamlObjectWriterSettings() { };

                        //writerSettings.RootObjectInstance = inst;
                        var xamlSchemaContext = XamlReader.GetWpfSchemaContext();

                        XamlObjectWriter x = new XamlObjectWriter(xamlSchemaContext, writerSettings);

                        var brushType = xamlSchemaContext.GetXamlType(typeof(LinearGradientBrush));

                        var reader = new XamlReader();
                        var xamlXmlReaderSettings = new XamlXmlReaderSettings();
                        xamlXmlReaderSettings.LocalAssembly = typeof(AnalysisControlsModule).Assembly;
                        XamlXmlReader rrr = new XamlXmlReader(xmlReader, xamlXmlReaderSettings);
                        var results = new List<LinearGradientBrush>();
                        try
                        {
                            while (rrr.Read())
                            {
                                //x.WriteNode(rrr);
                                switch (rrr.NodeType)
                                {
                                    case XamlNodeType.StartObject:
                                        if (rrr.Type.Name == nameof(LinearGradientBrush)) 
                                        {

                                            XamlObjectWriter x00 = new XamlObjectWriter(xamlSchemaContext,
                                                new XamlObjectWriterSettings(writerSettings)
                                                    {RootObjectInstance = new LinearGradientBrush()});

                                            var x1 = rrr.ReadSubtree();
                                            while (x1.Read())
                                            {
                                                x00.WriteNode(x1);
                                            }

                                            DebugUtils.WriteLine(x1.Value.ToString());
                                            results.Add((LinearGradientBrush) x1.Value);
                                        }

                                        DebugUtils.WriteLine(rrr.Type.Name);
                                        // DebugUtils.WriteLine(rrr.Value.ToString());
                                        break;
                                    case XamlNodeType.None:
                                        break;
                                    case XamlNodeType.GetObject:
                                        break;
                                    case XamlNodeType.EndObject:
                                        // if (rrr.Type == brushType)
                                        // {
                                        //     var brush = x.Result;
                                        //     DebugUtils.WriteLine(x.Result.ToString());
                                        // }

                                        break;
                                    case XamlNodeType.StartMember:
                                        DebugUtils.WriteLine(rrr.Member.Name);
                                        break;
                                    case XamlNodeType.EndMember:
                                        break;
                                    case XamlNodeType.Value:
                                        break;
                                    case XamlNodeType.NamespaceDeclaration:
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            DebugUtils.WriteLine(ex.ToString());
                        }
                    }

                }
            }
        }
    }
}