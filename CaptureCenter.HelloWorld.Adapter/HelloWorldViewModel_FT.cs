using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;
using ExportExtensionCommon;
using System.Xml.Serialization;
using System.IO;
using System.Globalization;

namespace CaptureCenter.HelloWorld
{
    public class HelloWorldViewModel_FT : ModelBase
    {
        #region Construction
        private HelloWorldViewModel vm;
        private HelloWorldSettings settings;

        public HelloWorldViewModel_FT(HelloWorldViewModel vm)
        {
            this.vm = vm;
            settings = vm.HelloWorldSettings;
            Cultures = CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(n => n.DisplayName).ToList();
        }

        public void Initialize(UserControl control) { }
 
        private bool initialized = false;
        public bool ActivateTab()
        {
            if (! initialized) InitializeFolderTree();
            initialized = true;
            return true;
        }
        #endregion

        #region Properties
        private SIEETreeView folders;
        public SIEETreeView Folders
        {
            get { return folders; }
            set { SetField(ref folders, value); }
        }

        private string selectedFolderDisplayName;
        public string SelectedFolderDisplayName
        {
            get { return selectedFolderDisplayName; }
            set { selectedFolderDisplayName = value; SendPropertyChanged(); }
        }

        private List<CultureInfo> cultures;
        public List<CultureInfo> Cultures
        {
            get { return cultures; }
            set { SetField(ref cultures, value); }
        }
        public CultureInfo SelectedCulture
        {
            get { return new CultureInfo(settings.SelectedCultureInfoName); }
            set
            {
                settings.SelectedCultureInfoName = value.Name;
                SendPropertyChanged();
            }
        }
        #endregion

        #region Functions
        // Called from user clicking somewhere in the tree
        public void SelectdFolderHandler(TVIViewModel foldeNode)
        {
            SelectedFolderDisplayName = foldeNode.GetDisplayNamePath();
            settings.SerializedFolderPath = foldeNode.GetSerializedPath();
        }

        public void InitializeFolderTree()
        {
            Folders = new SIEETreeView(vm);
            foreach (string drive in vm.HelloWorldClient.GetLogicalDrives())
                Folders.Add(new TVIViewModel(new HelloWorldFolder(null, vm.HelloWorldClient.GetDirectoryInfo(drive).DirectoryInfo), null, true));
            Folders.InitializeTree(settings.SerializedFolderPath, typeof(HelloWorldFolder));
            SelectdFolderHandler(Folders[0]);
        }
        #endregion
    }

    #region HelloWorldFolder
    public class HelloWorldFolder : TVIModel
    {
        #region Construction
        public HelloWorldFolder() { } // xml serialization

        public HelloWorldFolder(HelloWorldFolder parent, DirectoryInfo di)
        {
            DirInfo = di;
            if (parent == null)
                FolderPath = di.Name;
            else
            {
                FolderPath = parent.FolderPath + (parent.Depth == 0 ? "" : @"\") + di.Name;
            }
        }
        #endregion

        #region Properties
        private DirectoryInfo dirInfo;
        [XmlIgnore]
        public DirectoryInfo DirInfo
        {
            get { return dirInfo; }
            set { dirInfo = value; Id = dirInfo.Name; DisplayName = dirInfo.Name; }
        }
        public string FolderPath { get; set; }


        #endregion

        #region Functions
        public override List<TVIModel> GetChildren()
        {
            List<TVIModel> result = new List<TVIModel>();
            foreach (DirectoryInfo di in DirInfo.GetDirectories())
                result.Add(new HelloWorldFolder(this, di));
            return result;
        }

        public override string GetPathConcatenationString() { return @"\"; }
        public override string GetTypeName() { return "Folder"; }

        public override TVIModel Clone()
        {
            return this.MemberwiseClone() as HelloWorldFolder;
        }

        public override string GetPath(List<TVIModel> path, Pathtype pt)
        {
            string result = string.Empty;
            for (int i=0; i!=path.Count; i++)
            {
                result += path[i].Id;
                if (i > 0) result += GetPathConcatenationString();
            }
            return result;
        }

        public override bool IsSame(string id)
        {
            return (Id == id || Id + @"\" == id || Id == id + @"\");
        }
        #endregion
    }
    #endregion
}
