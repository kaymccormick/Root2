namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskState<T> where T : INodeData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subnodeData"></param>
        public TaskState(T subnodeData)
        {
            Node = subnodeData;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Node { get; set; }
    }
}