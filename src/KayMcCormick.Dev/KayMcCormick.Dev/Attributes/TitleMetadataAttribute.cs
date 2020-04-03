#region header
// Kay McCormick (mccor)
// 
// Deployment
// KayMcCormick.Lib.Wpf
// TitleMetadataAttribute.cs
// 
// 2020-03-16-10:50 AM
// 
// ---
#endregion
using System ;
using System.ComponentModel.Composition ;

namespace KayMcCormick.Dev.Attributes
{
    /// <summary>
    ///     Metadata attribute to indicate the title of a component or command.
    ///     Intended for user display. Right now is transferred from views to menu
    ///     commands.
    /// </summary>
    /// 
    [ MetadataAttribute ]
    // ReSharper disable once UnusedType.Global
    public class TitleMetadataAttribute : Attribute
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="title"></param>
        public TitleMetadataAttribute ( string title ) { Title = title ; }

        /// <summary>
        ///     Title
        /// </summary>
        public string Title { get ; }
    }

    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public class PurposeMetadataAttribute : Attribute
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="title"></param>
        public PurposeMetadataAttribute(string purpose) { Purpose = purpose; }

        /// <summary>
        ///     Title
        /// </summary>
        public string Purpose { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    [MetadataAttribute]
    public sealed class ConvertsTypeMetadataAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public Type ConvertsType { get ; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="convertsType"></param>
        public ConvertsTypeMetadataAttribute ( Type convertsType ) { ConvertsType = convertsType ; }
    }
}