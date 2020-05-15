using System.IO;

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

        /// <inheritdoc />
        public override string ToString()
        {
            return
                $"{nameof(Node)}: {Node}, {nameof(Data)}: {Data}, {nameof(Stream)}: {Stream}, {nameof(Length)}: {Length}, {nameof(MemoryStream)}: {MemoryStream}, {nameof(Value)}: {Value}";
        }
    }
}