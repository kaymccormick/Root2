using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Baml2006;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace KmDevWpfControls
{
    /// <summary>
    /// 
    /// </summary>
    public class Subnode1 : NodeBase1
    {
        private Task<TempLoadData1> _loadTask2;

        /// <summary>
        /// 
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object ResourceName { get; set; }

        /// <inheritdoc />
        public override bool CheckLoadItems(out NodeDataLoadState1 state)
        {
            state = DataState;
            switch (DataState)
            {
                case NodeDataLoadState1.NoAction:
                    return true;
                case NodeDataLoadState1.DataLoaded:
                    return true;
                case NodeDataLoadState1.RequiresAsync:
                    return false;
                case NodeDataLoadState1.LoadFailure:
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
        public override Subnode1 CreateSubnode()
        {
            return new Subnode1();
        }

        /// <inheritdoc />
        public override Task<TempLoadData1> CheckLoadItemsAsync()
        {
            if (_loadTask2 != null && _loadTask2.Status <= TaskStatus.Running) throw new InvalidOperationException("error");
            Items.Clear();
            _loadTask2 = Task.Factory.StartNew(LoadResourceData,
                new TaskState<Subnode1>(this), CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
            return _loadTask2;
        }

        /// <inheritdoc />
        public override void LoadResult(TempLoadData1 result)
        {
            Debug.WriteLine(result.ToString());
            switch (result.Value)
            {
                case BitmapSource img:
                    var imgc = new Image() {Source = img};
                    Items.Add(new Subnode1{Name=imgc});
                    break;
                case IEnumerable ee:
                    foreach (var o in ee)
                        if (o is SubnodeData sd)
                            Items.Add(new Subnode1() { Name = sd.Name, Value = sd.Value });
                    break;
            }
        }

        private static TempLoadData1 LoadResourceData(object state)
        {
            var st = (TaskState<Subnode1>) state;
            var subnode = st.Node;
            Debug.WriteLine($"{nameof(TempLoadData1)}: {subnode}");
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
                        Debug.WriteLine($"   Recreated Value: {binData}");
                        return new TempLoadData1() {Value = binData};

                    case "ResourceTypeCode.Int32":
                        var int32 = BitConverter.ToInt32(data, 0);
                        return new TempLoadData1() {Value = int32};

                    case "ResourceTypeCode.Boolean":
                        Debug.WriteLine($"   Recreated Value: {BitConverter.ToBoolean(data, 0)}");
                        break;
                    // .jpeg image stored as a stream.
                    case "ResourceTypeCode.Stream":
                        var offset = 4;

                        var size = BitConverter.ToInt32(data, 0);
                        var memoryStream = new MemoryStream(data, offset, size);
                        var ext = Path.GetExtension(subnode.Name.ToString()).ToLowerInvariant();
                        Debug.WriteLine(ext);
                        switch (ext)
                        {
                            case ".baml":
                                var object2 = subnode.Dispatcher.Invoke(() =>
                                {
                                    try
                                    {
                                        var reader1 = new Baml2006Reader(memoryStream);
                                        var object1 = XamlReader.Load(reader1);
                                        if (object1 is IDictionary rd)
                                            foreach (DictionaryEntry entry in rd)
                                            {
                                                var sb = new Subnode1() {Name = entry.Key, Value = entry.Value};
                                                subnode.Items.Add(sb);
                                            }

                                        return new TempLoadData1() {Value = object1};
                                    }
                                    catch (Exception ex)
                                    {
                                        return new TempLoadData1() {Exception = ex};
                                    }
                                }, DispatcherPriority.Send);
                                if (object2.Value is UIElement)
                                {
                                    subnode.Dispatcher.Invoke(() => { });
                                }

                                return object2;
                            case ".png":
                                var object3 = subnode.Dispatcher.Invoke(() =>
                                {
                                    try
                                    {
                                        Debug.WriteLine(".png");
                                        var png = new PngBitmapDecoder(memoryStream, BitmapCreateOptions.None,
                                            BitmapCacheOption.Default);
                                        BitmapSource src = png.Frames[0];
                                        var s = new SubnodeData();
                                        s.Value = src;
                                        s.Name = "Image";
                                        return new TempLoadData1() {Value = src};
                                    }
                                    catch (Exception ex)
                                    {
                                        return new TempLoadData1() {Exception = ex};
                                    }
                                });

                                return object3;


                            case ".jpg":
                            case ".jpeg":
                            case ".gif":
                            case ".ico":

                                break;
                        }

                        var load = new TempLoadData1()
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