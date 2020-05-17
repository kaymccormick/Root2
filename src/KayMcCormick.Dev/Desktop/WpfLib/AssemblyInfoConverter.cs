using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Data;
using KayMcCormick.Dev;

namespace KayMcCormick.Lib.Wpf
{
    public class AssemblyInfoConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Assembly a = (Assembly) value;
            if (a == null)
            {
                return null;
            }
            if ((String) parameter == "Name")

            {
                return a.GetName();
            }

            if ((string) parameter == "Location")
            {
                return a.IsDynamic ? null : a.Location;
            }

            if ((String) parameter == "Company")
            {
                return a.GetCustomAttribute < AssemblyCompanyAttribute>()?.Company;
            }
            return CreateNamespaceNodes(value);
        }

        public static List<NamespaceNode> CreateNamespaceNodes(object value)
        {
            var a = (Assembly) value;
            var rootNode = new NamespaceNode();
            var d = new Dictionary<string, NamespaceNode>();

            foreach (var aExportedType in a.ExportedTypes)
            {
                var ns = aExportedType.Namespace ?? "";
                var x = new Queue<string>(ns.Split('.'));
                int xi = 0;
                string prefix = "";
                NamespaceNode nsn = null;
                var curNode = rootNode;
                while (x.Any())
                {
                    var y = x.Dequeue();
                    if (curNode.Ns.TryGetValue(y, out var xxNamespaceNode))
                    {

                    }
                    else
                    {
                        xxNamespaceNode = new NamespaceNode() {ElementName = y, Position = xi, Prefix = prefix};
                        curNode.Ns[y] = xxNamespaceNode;
                    }

                    curNode = xxNamespaceNode;
                    nsn = xxNamespaceNode;
                    prefix += y + ".";
                    xi++;
                }

                nsn.Children.Add(new NamespaceNode()
                    {ElementName = aExportedType.Name, Position = xi, Prefix = prefix, Entity = aExportedType});
            }

            return rootNode.Ns.Values.ToList();
            
            // var qq111 = (from type in a.ExportedTypes
                // let elements = type.Namespace.Split('.').ToArray().Concat(Enumerable.Repeat(type.Name, 1)).ToArray()
                // let at = new AssemblyType {Assembly = type.Assembly, Type = type, Elements = elements.ToArray()}
                // from i in Enumerable.Range(0, elements.Count())
                // let nn = new NamespaceNode
                // {
                    // Position = i, ElementName = elements[i], Prefix = string.Join(".", elements.Take(i)) + "."
                // }
                // let info = new {Node = nn, AT = at}
                // select info).ToList();
            // var q1 = from qx in qq111
                    // where qx.Node.Position == 0
                    // group qx by qx.Node.ElementName
                    // into g1
                    // select new
                    // {
                        // g1.Key,
                        // List = qq111.Where(zxzx => zxzx.Node.Position == 1 && zxzx.Node.Prefix == g1.Key + ".").ToList()
                    // }
                ;
            // foreach (var x1 in q1)
            // {
                // DebugUtils.WriteLine($"{x1.Key}: {String.Join("\n", x1.List)}\n****\n");
            // }
                // group info by info.Node into groups where groups.Key.Position == 0
                         // select new {x=groups.Key.ElementName, y=groups.ToList()}
     // ).ToList();

            // foreach (var zzzx in qq111)
            // {
                // foreach (var x1 in zzzx.y.Where(y111 => y111.Node.Position == 1))
                // {
                    // DebugUtils.WriteLine($"{zzzx.x}: {x1.Node.ElementName}");
                // }
            // }
            
        
            // foreach (var grouping in qq111.Where(xxxx => xxxx.Key.Position == 0))
            // {
                
            // }
            // foreach (var x1 in qq111)
            // {
            //     DebugUtils.WriteLine($"{x1.Key}");
            //     foreach (var x2 in x1)
            //     {
            //         DebugUtils.WriteLine($"\t{x2.AT.Type.FullName}");
            //     }
            // }

            // var ns0 = a.ExportedTypes.Select(t => t.Namespace).Distinct().ToList();


            // var qq = (from ns in ns0
                // let x11 = new {Namespace = ns, Elements = ns.Split('.')}
                // select x11).SelectMany(y =>
                // y.Elements.Select((z, i) => new {y.Namespace, ElementName = z, Position = i}));

            // var pl = qq.Select(x1 => new NamespaceNode()
                // {Namespace = x1.Namespace, ElementName = x1.ElementName, Position = x1.Position});
            // foreach (var namespaceNode1 in pl.GroupBy<NamespaceNode, string, NamespaceNode>(node => node.Prefix,
                // (s, nodes) =>
                // {
                    // var namespaceNode = new NamespaceNode
                    // {
                        // Prefix = s
                    // };
                    // namespaceNode.Children.AddRange(nodes);
                    // return namespaceNode;
                // }))
            // {
                // DebugUtils.WriteLine(namespaceNode1.Prefix);
                // DebugUtils.WriteLine(namespaceNode1.Namespace
                // );
            // }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}