namespace AnalysisAppLib
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LocationInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string FileName { get ; }

        /// <summary>
        /// 
        /// </summary>
        public int CharStart { get ; }

        /// <summary>
        /// 
        /// </summary>
        public int LineStart { get ; }

        /// <summary>
        /// 
        /// </summary>
        public int CharEnd { get ; }

        /// <summary>
        /// 
        /// </summary>
        public int LineEnd { get ; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadataModuleMetadataName"></param>
        /// <param name="sourceSpanStart"></param>
        /// <param name="sourceSpanEnd"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="charStart"></param>
        /// <param name="lineStart"></param>
        /// <param name="charEnd"></param>
        /// <param name="lineEnd"></param>
        /// <param name="metadataModuleMetadataName"></param>
        /// <param name="sourceSpanStart"></param>
        /// <param name="sourceSpanEnd"></param>
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

        /// <summary>
        /// 
        /// </summary>
        public string MetadataModuleMetadataName { get ; }

        /// <summary>
        /// 
        /// </summary>
        public int SourceSpanStart { get ; }

        /// <summary>
        /// 
        /// </summary>
        public int SourceSpanEnd { get ; }
    }
}