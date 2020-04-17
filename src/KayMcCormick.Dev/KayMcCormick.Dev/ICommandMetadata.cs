using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        string Title { get; set; }
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
