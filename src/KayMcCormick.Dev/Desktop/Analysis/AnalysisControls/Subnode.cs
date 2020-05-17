using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using KayMcCormick.Dev;

namespace AnalysisControls
{
    /// <summary>
    /// 
    /// </summary>
    public class Subnode : NodeBase
    {
        private Task<TempLoadData> _loadTask2;

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object ResourceName { get; set; }

        /// <inheritdoc />
        public override bool CheckLoadItems(out NodeDataLoadState state)
        {
            state = DataState;
            switch (DataState)
            {
                case NodeDataLoadState.NoAction:
                    return true;
                case NodeDataLoadState.DataLoaded:
                    return true;
                case NodeDataLoadState.RequiresAsync:
                    return false;
                case NodeDataLoadState.LoadFailure:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override DataLoadStrategy LoadStrategy { get; set; }

        /// <inheritdoc />
        public override Subnode CreateSubnode()
        {
            return new Subnode();
        }

        /// <inheritdoc />
        public override Task<TempLoadData> CheckLoadItemsAsync()
        {
            if (_loadTask2 != null && _loadTask2.Status <= TaskStatus.Running) throw new InvalidOperationException();
            Items.Clear();
            _loadTask2 = Task.Factory.StartNew(LoadResourceData,
                new TaskState<Subnode>(this), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return _loadTask2;
        }

        /// <inheritdoc />
        public override void LoadResult(TempLoadData result)
        {
            DebugUtils.WriteLine(result.ToString());
            if (!(result.Value is IEnumerable ee)) return;
            foreach (var o in ee)
                if (o is SubnodeData sd)
                    Items.Add(new Subnode() {Name = sd.Name, Value = sd.Value});
        }

        private static TempLoadData LoadResourceData(object state)
        {
            var st = (TaskState<Subnode>) state;
            var subnode = st.Node;
            DebugUtils.WriteLine($"{nameof(TempLoadData)}: {subnode}");
            var stream = subnode.Assembly.GetManifestResourceStream(subnode.ResourceName.ToString());
            if (stream == null) return null;
            using (var reader = new ResourceReader(stream))
            {
                reader.GetResourceData(subnode.Name.ToString(), out var dataType, out var data);
                switch (dataType)
                {
                    // Handle internally serialized string data (ResourceTypeCode members).
                    case "ResourceTypeCode.String":
                        var binaryReader = new BinaryReader(new MemoryStream(data));
                        var binData = binaryReader.ReadString();
                        DebugUtils.WriteLine($"   Recreated Value: {binData}");
                        return new TempLoadData() {Value = binData};

                    case "ResourceTypeCode.Int32":
                        var int32 = BitConverter.ToInt32(data, 0);
                        return new TempLoadData() {Value = int32};

                    case "ResourceTypeCode.Boolean":
                        DebugUtils.WriteLine($"   Recreated Value: {BitConverter.ToBoolean(data, 0)}");
                        break;
                    // .jpeg image stored as a stream.
                    case "ResourceTypeCode.Stream":
                        var offset = 4;

                        var size = BitConverter.ToInt32(data, 0);
                        var memoryStream = new MemoryStream(data, offset, size);
                        var ext = Path.GetExtension(subnode.Name.ToString()).ToLowerInvariant();
                        switch (ext)
                        {
                            case ".baml":
                            var object2 = subnode.Dispatcher.Invoke(() =>
                            {
                                var reader1 = new Baml2006Reader(memoryStream);
                                var object1 = XamlReader.Load(reader1);
                                var nodes = new List<SubnodeData>();
                                if (object1 is IDictionary rd)
                                    foreach (DictionaryEntry entry in rd)
                                    {
                                        var sb = new Subnode() {Name = entry.Key, Value = entry.Value};
                                        subnode.Items.Add(sb);
                                    }

                                return new TempLoadData() {Value = object1};
                            }, DispatcherPriority.Send);
                            if (object2.Value is UIElement)
                            {
                                subnode.Dispatcher.Invoke(() => { });
                            }

                            break;
                            case ".png":
                                var png = new PngBitmapDecoder(memoryStream, BitmapCreateOptions.None,
                                    BitmapCacheOption.Default);
                                BitmapSource src = png.Frames[0];
                                subnode.Value = src;
                                break;

                            case ".jpg":
                            case ".jpeg":
                            case ".gif":
                            case ".ico":

                                break;
                        }

                        var load = new TempLoadData()
                        {
                            Length = size,
                            MemoryStream = memoryStream,
                            Data = data
                        };
                        return load;
                }
            }

            return null;
        }
    }
}