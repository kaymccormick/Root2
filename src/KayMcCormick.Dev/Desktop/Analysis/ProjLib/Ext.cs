using System;
using System.Collections.Generic;
using System.IO ;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis ;

namespace ProjLib
{
    public static class Ext
    {
        public static string ShortenedPath ( this Document doc )
        {
            var strings = doc.FilePath.Split(Path.DirectorySeparatorChar);
            var numElems = 4 ;
            var p = strings.ToList()
                           .GetRange(
                                     strings.Length < numElems  ? 0 : strings.Length - numElems
                                   , strings.Length >= numElems ? 3 : strings.Length
                                    );
            return string.Join(Path.DirectorySeparatorChar.ToString(), p);
        }

        public static string RelativePath ( this Document doc )
        {
            return GetRelativePath ( doc.Project.FilePath , doc.FilePath ) ;
        }
        public static string GetRelativePath(string relativeTo, string path)
        {
            var uri = new Uri(relativeTo);
            var rel = Uri
                     .UnescapeDataString(uri.MakeRelativeUri(new Uri(path)).ToString())
                     .Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            if (rel.Contains(Path.DirectorySeparatorChar.ToString()) == false)
            {
                rel = $".{Path.DirectorySeparatorChar}{rel}";
            }

            return rel;
        }

    }
}
