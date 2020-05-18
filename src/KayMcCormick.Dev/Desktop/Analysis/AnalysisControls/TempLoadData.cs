using System;
using System.IO;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class TempLoadData
    {
        /// <summary>
        /// 
        /// </summary>
        public Subnode Node { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MemoryStream MemoryStream { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            try
            {
                return
                    $"{nameof(Node)}: {Node}, {nameof(Data)}: {Data}, {nameof(Stream)}: {Stream}, {nameof(Length)}: {Length}, {nameof(MemoryStream)}: {MemoryStream}, {nameof(Value)}: {Value}";
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
                return base.ToString();
            }
        }
    }
}