using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace KmDevWpfControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RootNode1 : AssemblyResourceNodeBase1
    {
        private Task<TempLoadData1> _loadTask2;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckLoadItems(out NodeDataLoadState1 state)
        {
            state = NodeDataLoadState1.RequiresAsync;
            return false;
        }

        /// <inheritdoc />
        public override DataLoadStrategy LoadStrategy { get; set; }

        /// <inheritdoc />
        public override Subnode1 CreateSubnode()
        {
            return new Subnode1();
        }

        /// <inheritdoc />
        public override Task<TempLoadData1> CheckLoadItemsAsync()
        {
            if (_loadTask2 != null && _loadTask2.Status <= TaskStatus.Running) throw new InvalidOperationException("error");
            Items.Clear();
            _loadTask2 = Task.Factory.StartNew(LoadResources,
                new TaskState<RootNode1>(this), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return _loadTask2;
        }

        private static TempLoadData1 LoadResources(object state1)
        {
            var st = (TaskState<RootNode1>) state1;
            var node = st.Node;
            var res = new TempLoadData1();
            try
            {
                var dta = new List<SubnodeData>();
                using (var stream = node.Assembly.GetManifestResourceStream(node.Name.ToString()))
                {
                    if (stream != null)
                        using (var reader = new ResourceReader(stream))
                        {
                            foreach (var dictionaryEntry in reader.Cast<DictionaryEntry>())
                            {
                                var value = dictionaryEntry.Value;
                                if (((string) dictionaryEntry.Key).EndsWith(".png"))
                                {
                                    Debug.WriteLine(dictionaryEntry.Value.ToString());
                                }
                                try
                                {
                                    var data = new SubnodeData
                                    {
                                        Name = dictionaryEntry.Key as string,
                                        Value = value,
                                        ResourceName = node.Name,
                                        Assembly = node.Assembly
                                    };
                                    dta.Add(data);
                                }
                                catch (Exception ex)
                                {
                                    // DebugUtils.WriteLine(ex.ToString());
                                }
                            }
                        }
                }

                res.Value = dta;
                // DebugUtils.WriteLine("End of enumerate children");
                return res;
            }
            catch (Exception ex)
            {
                // DebugUtils.WriteLine(ex.ToString());
            }

            return res;
        }

        /// <inheritdoc />
        public override void LoadResult(TempLoadData1 result)
        {
            // DebugUtils.WriteLine("Loading data");
            Items.Clear();
            if (result.Value is IEnumerable x && !(result.Value is string))
            {
                foreach (var o in x)
                {
                    var subnodeData = (SubnodeData) o;
                    //DebugUtils.WriteLine("Adding " + subnodeData.Name);
                    Items.Add(new Subnode1()
                    {
                        Assembly = subnodeData.Assembly, Name = subnodeData.Name,
                        ResourceName = subnodeData.ResourceName
                    });
                }

                //DebugUtils.WriteLine("Setting data loaded");
                DataState = NodeDataLoadState1.DataLoaded;
            }
        }
    }

    public enum DataLoadStrategy
    {
        /// <summary>
        /// 
        /// </summary>
        None,

        /// <summary>
        /// 
        /// </summary>
        LoadAsync,

        /// <summary>
        /// 
        /// </summary>
        LoadSync
    }

    public class SubnodeData

    {
        /// <summary>
        /// 
        /// </summary>
        public object Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        public object ResourceName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Assembly Assembly { get; set; }

        public object Value { get; set; }
    }

    public class TaskState<T> where T : IAssemblyResourceNode
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