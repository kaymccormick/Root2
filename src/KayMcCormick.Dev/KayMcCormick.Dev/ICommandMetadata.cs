namespace KayMcCormick.Dev
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDescriptiveMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        string Title { get; }
        /// <summary>
        /// 
        /// </summary>
        string Description { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ICommandMetadata : IDescriptiveMetadata
    {
        /// <summary>
        /// 
        /// </summary>
        string CommandId { get ; set ; }

    }

    /// <summary>
    /// 
    /// </summary>
    public interface IViewMetadata : IDescriptiveMetadata
    {
        
    }
}
