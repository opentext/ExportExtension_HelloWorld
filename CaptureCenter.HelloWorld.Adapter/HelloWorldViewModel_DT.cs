﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;
using ExportExtensionCommon;

namespace CaptureCenter.HelloWorld
{
    public class HelloWorldViewModel_DT : ModelBase
    {
        #region Construction
        private HelloWorldViewModel vm;
        private HelloWorldSettings settings;
        private NameSpecParser nsp;

        public HelloWorldViewModel_DT(HelloWorldViewModel vm)
        {
            this.vm = vm;
            settings = vm.HelloWorldSettings;
            nsp = new NameSpecParser();
            Result = nsp.Convert(settings.Specification);
        }

        public void Initialize(UserControl control) { }

        public bool ActivateTab(SIEEFieldlist fieldlist)
        {
            SetFieldNames(fieldlist);
            return false;
        }
        #endregion

        #region Properties FolderTab
        public bool UseInputFileName
        {
            get { return settings.UseInputFileName; }
            set {
                settings.UseInputFileName = value;
                settings.UseSpecification = !value;
                SendPropertyChanged();
            }
        }

        public bool UseSpecification
        {
            get { return settings.UseSpecification; }
            set {
                settings.UseSpecification = value;
                settings.UseInputFileName = !value;
                SendPropertyChanged();
            }
        }

        public string Specification
        {
            get { return settings.Specification; }
            set {
                settings.Specification = value;
                Result = nsp.Convert(settings.Specification);
                SendPropertyChanged();
            }
        }

        private string result;
        public string Result
        {
            get { return result; }
            set { SetField(ref result, value); }
        }

        private List<string> fieldNames;
        public List<string> FieldNames
        {
            get { return fieldNames; }
            set { SetField(ref fieldNames, value); }
        }

        private string selectedFieldName;
        public string SelectedFieldName
        {
            get { return selectedFieldName; }
            set { SetField(ref selectedFieldName, value); }
        }
        #endregion

        #region Functions
        public void AddTokenToFileHandler(string token)
        {
            if (token == "Add")
                Specification += "<:" + SelectedFieldName + ">";
            else
                Specification += "<" + token + ">";
        }

        public void SetFieldNames(SIEEFieldlist fieldList)
        {
            List<string> newFieldNames = fieldList.GetScalarFieldNames();
            if (!SIEEUtils.StringListEqual(FieldNames, newFieldNames))
            {
                FieldNames = newFieldNames;
                if (FieldNames.Count > 0) SelectedFieldName = FieldNames[0];
            }
        }
        #endregion
    }
}
