using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class RootNode : NodeBase
    {
        private Task<TempLoadData> _loadTask2;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool CheckLoadItems(out NodeDataLoadState state)
        {
            state = NodeDataLoadState.RequiresAsync;
            return false;
        }

        /// <inheritdoc />
        public override DataLoadStrategy LoadStrategy { get; set; }

        /// <inheritdoc />
        public override Subnode CreateSubnode()
        {
            return new Subnode();
        }

        /// <inheritdoc />
        public override Task<TempLoadData> CheckLoadItemsAsync()
        {
            if (_loadTask2 != null && _loadTask2.Status <= TaskStatus.Running) throw new AppInvalidOperationException();
            Items.Clear();
            _loadTask2 = Task.Factory.StartNew(LoadResources,
                new TaskState<RootNode>(this), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return _loadTask2;
        }

        private static TempLoadData LoadResources(object state1)
        {
            var st = (TaskState<RootNode>) state1;
            var node = st.Node;
            var res = new TempLoadData();
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
                                    DebugUtils.WriteLine(dictionaryEntry.Value.ToString());
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
                                    DebugUtils.WriteLine(ex.ToString());
                                }
                            }
                        }
                }

                res.Value = dta;
                DebugUtils.WriteLine("End of enumerate children");
                return res;
            }
            catch (Exception ex)
            {
                DebugUtils.WriteLine(ex.ToString());
            }

            return res;
        }

        /// <inheritdoc />
        public override void LoadResult(TempLoadData result)
        {
            DebugUtils.WriteLine("Loading data");
            Items.Clear();
            if (result.Value is IEnumerable x && !(result.Value is string))
            {
                foreach (var o in x)
                {
                    var subnodeData = (SubnodeData) o;
                    DebugUtils.WriteLine("Adding " + subnodeData.Name);
                    Items.Add(new Subnode()
                    {
                        Assembly = subnodeData.Assembly, Name = subnodeData.Name,
                        ResourceName = subnodeData.ResourceName
                    });
                }

                DebugUtils.WriteLine("Setting data loaded");
                DataState = NodeDataLoadState.DataLoaded;
            }
        }
    }
}