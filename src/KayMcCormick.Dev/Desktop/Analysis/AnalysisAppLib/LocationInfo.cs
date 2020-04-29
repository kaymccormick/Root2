namespace AnalysisAppLib
{
    public sealed class LocationInfo
    {
        public string FileName { get ; }

        public int CharStart { get ; }

        public int LineStart { get ; }

        public int CharEnd { get ; }

        public int LineEnd { get ; }

        // ReSharper disable once UnusedMember.Global
        public LocationInfo (
            string metadataModuleMetadataName
            , int    sourceSpanStart
            , int    sourceSpanEnd
        )
        {
            MetadataModuleMetadataName = metadataModuleMetadataName ;
            SourceSpanStart            = sourceSpanStart ;
            SourceSpanEnd              = sourceSpanEnd ;
        }

        public LocationInfo (
            string fileName
            , int    charStart
            , int    lineStart
            , int    charEnd
            , int    lineEnd
            , string metadataModuleMetadataName
            , int    sourceSpanStart
            , int    sourceSpanEnd
        )
        {
            FileName                   = fileName ;
            CharStart                  = charStart ;
            LineStart                  = lineStart ;
            CharEnd                    = charEnd ;
            LineEnd                    = lineEnd ;
            MetadataModuleMetadataName = metadataModuleMetadataName ;
            SourceSpanStart            = sourceSpanStart ;
            SourceSpanEnd              = sourceSpanEnd ;
        }

        public string MetadataModuleMetadataName { get ; }

        public int SourceSpanStart { get ; }

        public int SourceSpanEnd { get ; }
    }
}