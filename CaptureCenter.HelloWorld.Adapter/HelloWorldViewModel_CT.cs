using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ExportExtensionCommon;
using System.IO;

namespace CaptureCenter.HelloWorld
{
    public class HelloWorldViewModel_CT :ModelBase
    {
        #region Construction
        private HelloWorldViewModel vm;
        private HelloWorldSettings settings;

        public HelloWorldViewModel_CT(HelloWorldViewModel vm)
        {
            this.vm = vm;
            settings = vm.HelloWorldSettings;
        }

        public void Initialize(UserControl control)
        {
            findPasswordBox(control);
        }

        public bool ActivateTab() { return true; }
        #endregion

        #region Properties ConnectionTab
        public string Servername
        {
            get { return settings.Servername; }
            set { settings.Servername = value; SendPropertyChanged(); }
        }

        string Username_name = "Username";
        public string Username
        {
            get { return settings.Username; }
            set { settings.Username = value; SendPropertyChanged(); }
        }
        #endregion

        #region Password
        string Password_name = "Password";
        public string Password
        {
            get {
                if (settings.Password == null) return string.Empty;
                return PasswordEncryption.Decrypt(settings.Password);
            }
            set {
                settings.Password = PasswordEncryption.Encrypt(value);
                SendPropertyChanged("Password");
            }
        }

        private PasswordBox passwordBox;
        private void findPasswordBox(UserControl control)
        {
            passwordBox = (PasswordBox)LogicalTreeHelper.FindLogicalNode(control, "passwordBox");
        }
        public void PasswordChangedHandler()
        {
            Password = SIEEUtils.GetUsecuredString(passwordBox.SecurePassword);
        }
        #endregion

        #region Functions Connection
        public bool IsConnectionRelevant(string property)
        {
            return
                property == Username_name ||
                property == Password_name;
        }

        public void LoginButtonHandler()
        {
        }
        public void ShowVersion()
        {
            SIEEMessageBox.Show("HelloWorld  connector Version 0.7", "Version", MessageBoxImage.Information);
        }

        private ConnectionTestResultDialog connectionTestResultDialog;
        private ConnectionTestHandler ConnectionTestHandler;

        // Set up objects, start tests (running in the backgroud) and launch the dialog
        public void TestButtonHandler()
        {
            VmTestResultDialog vmConnectionTestResultDialog = new VmTestResultDialog();
            ConnectionTestHandler = new HelloWorldConnectionTestHandler(vmConnectionTestResultDialog);
            ConnectionTestHandler.CallingViewModel = this;

            connectionTestResultDialog = new ConnectionTestResultDialog(ConnectionTestHandler);
            connectionTestResultDialog.DataContext = vmConnectionTestResultDialog;
            connectionTestResultDialog.ShowInTaskbar = false;

            //The test environment is Winforms, we then set the window to topmost.
            //In OCC we we can set the owner property
            if (Application.Current == null)
                connectionTestResultDialog.Topmost = true;
            else
                connectionTestResultDialog.Owner = Application.Current.MainWindow;

            ConnectionTestHandler.LaunchTests();
            connectionTestResultDialog.ShowDialog();
        }

        #endregion

    }
}
