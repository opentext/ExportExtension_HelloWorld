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
    public class HelloWorldViewModel : SIEEViewModel
    {
        #region Construction
        public HelloWorldSettings HelloWorldSettings;
        public IHelloWorldClient HelloWorldClient;

        public HelloWorldViewModel_CT CT { get; set; }
        public HelloWorldViewModel_FT FT { get; set; }
        public HelloWorldViewModel_DT DT { get; set; }

        public HelloWorldViewModel(SIEESettings settings, IHelloWorldClient helloWorldClient)
        {
            HelloWorldSettings = settings as HelloWorldSettings;
            this.HelloWorldClient = helloWorldClient;

            CT = new HelloWorldViewModel_CT(this);
            FT = new HelloWorldViewModel_FT(this);
            DT = new HelloWorldViewModel_DT(this);

            SelectedTab = 0;
            IsRunning = false;
            DataLoaded = false;

            if (HelloWorldSettings.LoginPossible) LoginButtonHandler();

            CT.PropertyChanged += (s, e) =>
            {
                if (CT.IsConnectionRelevant(e.PropertyName))
                {
                    HelloWorldSettings.LoginPossible = false;
                    DataLoaded = false;
                    TabNamesReset();
                }
            };
        }

        public override void Initialize(UserControl control)
        {
            CT.Initialize(control);
            FT.Initialize(control);
            DT.Initialize(control);
            initializeTabnames(control);
        }

        public override SIEESettings Settings
        {
            get { return HelloWorldSettings; }
        }
        #endregion

        #region Properties (general)
        // The settings in this view model just control the visibility and accessibility of the various tabs
        private int selectedTab;
        public int SelectedTab
        {
            get { return selectedTab; }
            set { selectedTab = value; SendPropertyChanged(); }
        }
        private bool dataLoaded;
        public bool DataLoaded
        {
            get { return dataLoaded; }
            set { dataLoaded = value; SendPropertyChanged(); }
        }
        #endregion

        #region Event handler
        public void LoginButtonHandler()
        {
            IsRunning = true;
            HelloWorldSettings.LoginPossible = true;
            try { CT.LoginButtonHandler();}
            catch (Exception e)
            {
                DataLoaded = false;
                HelloWorldSettings.LoginPossible = false;
                SIEEMessageBox.Show(e.Message, "Login error", MessageBoxImage.Error);
            }
            finally { IsRunning = false; }
            DataLoaded = true;
            if (HelloWorldSettings.LoginPossible) SelectedTab = 1;
        }
        #endregion

        #region Tab activation
        public Dictionary<string, bool> Tabnames;
        // Retrieve tabitem names from user control
        private void initializeTabnames(UserControl control)
        {
            Tabnames = new Dictionary<string, bool>();
            TabControl tc = (TabControl)LogicalTreeHelper.FindLogicalNode(control, "mainTabControl");
            foreach (TabItem tabItem in LogicalTreeHelper.GetChildren(tc)) Tabnames[tabItem.Name] = false;
        }

        public void ActivateTab(string tabName)
        {
            if (Tabnames[tabName]) return;
            IsRunning = true;
            try
            {
                switch (tabName)
                {
                    case "connectionTabItem":   { Tabnames[tabName] = CT.ActivateTab(); break; }
                    case "folderTabItem":       { Tabnames[tabName] = FT.ActivateTab(); break; }
                    case "documentTabItem":     { Tabnames[tabName] = DT.ActivateTab(Settings.CreateSchema()); break; }
                }
            }
            catch (Exception e)
            {
                SIEEMessageBox.Show(e.Message, "Error in " + tabName, MessageBoxImage.Error);
                DataLoaded = false;
                SelectedTab = 0;
                TabNamesReset();
            }
            finally { IsRunning = false; }
        }

        private void TabNamesReset()
        {
            foreach (string tn in Tabnames.Keys.ToList()) Tabnames[tn] = false;
        }
        #endregion
    }
}
