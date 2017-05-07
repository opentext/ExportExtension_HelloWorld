using System;
using System.Collections.Generic;
using ExportExtensionCommon;
using System.Globalization;

namespace CaptureCenter.HelloWorld
{
    [Serializable]
    public class HelloWorldSettings : SIEESettings
    {
        #region Contruction
        public HelloWorldSettings()
        {
            // Connection tab
            Servername = "";
            Username = Password = "";
            loginPossible = false;

            // Folder tab
            SelectedCultureInfoName = CultureInfo.CurrentCulture.Name;

            // Document tab
            UseSpecification = true;
            Specification = "<BATCHID>_<DOCUMENTNUMBER>";
        }
        #endregion

        #region Properties Connection
        private string servername;
        public string Servername
        {
            get { return servername; }
            set { SetField(ref servername, value); }
        }

        private string username;
        public string Username
        {
            get { return username; }
            set { SetField(ref username, value); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { SetField(ref password, value); ; }
        }

        private bool loginPossible;
        public bool LoginPossible
        {
            get { return loginPossible; }
            set { SetField(ref loginPossible, value); }
        }
        #endregion

        #region Properties Folder
        private List<string> serializedFolderPath;
        public List<string> SerializedFolderPath
        {
            get { return serializedFolderPath; }
            set { SetField(ref serializedFolderPath, value); }
        }

        private string selectedCultureInfoName;
        public string SelectedCultureInfoName
        {
            get { return selectedCultureInfoName; }
            set { SetField(ref selectedCultureInfoName, value); }
        }
        #endregion

        #region Properties Document
        private bool useInputFileName;
        public bool UseInputFileName
        {
            get { return useInputFileName; }
            set { SetField(ref useInputFileName, value); }
        }

        private bool useSpecification;
        public bool UseSpecification
        {
            get { return useSpecification; }
            set { SetField(ref useSpecification, value); RaisePropertyChanged(specification_Name); }
        }

        private string specification_Name = "Specification";
        private string specification;
        public string Specification
        {
            get { return useSpecification ? specification : null; }
            set { SetField(ref specification, value); }
        }
        #endregion

        #region Functions
        public void InitializeHelloWorldlient(IHelloWorldClient helloWorldClient)
        {
            helloWorldClient.Username = Username;
            helloWorldClient.SetPassword(PasswordEncryption.Decrypt(Password));
            helloWorldClient.Culture = new CultureInfo(SelectedCultureInfoName);
        }

        public string GetFolderName()
        {
            return ((HelloWorldFolder)TVIViewModel.GetSelectedItem((SerializedFolderPath), typeof(HelloWorldFolder))).FolderPath;
        }

        public override SIEEFieldlist CreateSchema()
        {
            SIEEFieldlist schema = new SIEEFieldlist();
            schema.Add(new SIEEField { Name = "SampleField", ExternalId = "SampleField" });
            schema.Add(new SIEEField { Name = "SomeOtherField", ExternalId = "SomeOtherField" });
            return schema;
        }
        
        public override object Clone()
        {
            return this.MemberwiseClone() as HelloWorldSettings;
        }

        public override string GetDocumentNameSpec()
        {
            return Specification;
        }
        #endregion
    }
}