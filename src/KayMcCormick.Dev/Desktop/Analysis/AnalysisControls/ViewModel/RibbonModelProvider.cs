using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace AnalysisControls.ViewModel
{
    [CmdletProvider("Test", ProviderCapabilities.None)]
    internal class RibbonModelProvider : NavigationCmdletProvider
    {
        /// <inheritdoc />
        protected override string GetChildName(string path)
        {
            return base.GetChildName(path);
        } /// <inheritdoc />
        protected override void MoveItem(string path, string destination)
        {
            base.MoveItem(path, destination);
        }

        /// <inheritdoc />
        protected override object MoveItemDynamicParameters(string path, string destination)
        {
            return base.MoveItemDynamicParameters(path, destination);
        }

        /// <inheritdoc />
        protected override string NormalizeRelativePath(string path, string basePath)
        {
            return base.NormalizeRelativePath(path, basePath);
        }

        /// <inheritdoc />
        protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
        {
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        /// <inheritdoc />
        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            var pd = (Main1Model)Host.PrivateData.BaseObject;

            return base.CopyItemDynamicParameters(path, destination, recurse);
        }

        /// <inheritdoc />
        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            var pd = (Main1Model)Host.PrivateData.BaseObject;
            foreach (var pdDocument in pd.Documents)
            {
                WriteItemObject(pd, path + "/" + pdDocument.ToString(), false);
            }
            return base.GetChildItemsDynamicParameters(path, recurse);
        }

        /// <inheritdoc />
        protected override object GetChildNamesDynamicParameters(string path)
        {
            return base.GetChildNamesDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override bool HasChildItems(string path)
        {
            return base.HasChildItems(path);
        }

        /// <inheritdoc />
        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return base.NewItemDynamicParameters(path, itemTypeName, newItemValue);
        }

        /// <inheritdoc />
        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            return base.RemoveItemDynamicParameters(path, recurse);
        }

        /// <inheritdoc />
        protected override void RenameItem(string path, string newName)
        {
            base.RenameItem(path, newName);
        }

        /// <inheritdoc />
        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            return base.RenameItemDynamicParameters(path, newName);
        }

        /// <inheritdoc />
        protected override void ClearItem(string path)
        {
            base.ClearItem(path);
        }

        /// <inheritdoc />
        protected override object ClearItemDynamicParameters(string path)
        {
            return base.ClearItemDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override string[] ExpandPath(string path)
        {
            return base.ExpandPath(path);
        }

        /// <inheritdoc />
        protected override object GetItemDynamicParameters(string path)
        {
            return base.GetItemDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override void InvokeDefaultAction(string path)
        {
            base.InvokeDefaultAction(path);
        }

        /// <inheritdoc />
        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            return base.InvokeDefaultActionDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override bool ItemExists(string path)
        {
            return true;
            return base.ItemExists(path);
        }

        /// <inheritdoc />
        protected override object ItemExistsDynamicParameters(string path)
        {
            return base.ItemExistsDynamicParameters(path);
        }

        /// <inheritdoc />
        protected override void SetItem(string path, object value)
        {
            base.SetItem(path, value);
        }

        /// <inheritdoc />
        protected override object SetItemDynamicParameters(string path, object value)
        {
            return base.SetItemDynamicParameters(path, value);
        }

        /// <inheritdoc />
        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            return base.RemoveDrive(drive);
        }

        /// <inheritdoc />
        public override string GetResourceString(string baseName, string resourceId)
        {
            return base.GetResourceString(baseName, resourceId);
        }

        /// <inheritdoc />
        protected override ProviderInfo Start(ProviderInfo providerInfo)
        {
            return base.Start(providerInfo);
        }

        /// <inheritdoc />
        protected override object StartDynamicParameters()
        {
            return base.StartDynamicParameters();
        }

        /// <inheritdoc />
        protected override void Stop()
        {
            base.Stop();
        }

        /// <inheritdoc />
        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        /// <inheritdoc />
        public override char AltItemSeparator { get; }

        /// <inheritdoc />
        public override char ItemSeparator { get; }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString();
        }

        /// <inheritdoc />
        protected override string GetParentPath(string path, string root)
        {
            return base.GetParentPath(path, root);
        }

        /// <inheritdoc />
        protected override bool IsItemContainer(string path)
        {
            return true;
            return base.IsItemContainer(path);
        }

        /// <inheritdoc />
        protected override string MakePath(string parent, string child)
        {
            return base.MakePath(parent, child);
        }

        /// <inheritdoc />
        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            base.CopyItem(path, copyPath, recurse);
        }

        /// <inheritdoc />
        protected override void GetChildItems(string path, bool recurse)
        {
            var pd = (Main1Model)Host.PrivateData.BaseObject;
            foreach (var pdDocument in pd.Documents)
            {
                if (pdDocument is DocModel dm)
                {
                    var p = MakePath(path, dm.Title);
                    WriteItemObject(new PSObject(dm),p, false);
                }
            }

            

        }

        /// <inheritdoc />
        protected override void GetChildItems(string path, bool recurse, uint depth)
        {
            base.GetChildItems(path, recurse, depth);
        }

        /// <inheritdoc />
        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            base.GetChildNames(path, returnContainers);
        }

        /// <inheritdoc />
        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            base.NewItem(path, itemTypeName, newItemValue);
        }

        /// <inheritdoc />
        protected override void RemoveItem(string path, bool recurse)
        {
            base.RemoveItem(path, recurse);
        }

        /// <inheritdoc />
        protected override void GetItem(string path)
        {

            base.GetItem(path);
        }

        /// <inheritdoc />
        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            CustomDriveInfo info = new CustomDriveInfo(drive,Host.PrivateData);
            
            return base.NewDrive(drive);
        }

        /// <inheritdoc />
        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            var initializeDefaultDrives = new Collection<PSDriveInfo>();
            initializeDefaultDrives.Add(new CustomDriveInfo(
                new PSDriveInfo("Test1", ProviderInfo, @"/", "test1", PSCredential.Empty), Host.PrivateData));
            return initializeDefaultDrives;
        }


        /// <inheritdoc />
        protected override bool IsValidPath(string path)
        {
            return true;
        }
    }
}