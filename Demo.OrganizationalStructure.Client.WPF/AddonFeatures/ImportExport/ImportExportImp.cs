using Demo.OrganizationalStructure.Common.DataModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace Demo.OrganizationalStructure.Client.WPF.AddonFeatures.ImportExport
{
    internal class ImportExportImp
    {
        private readonly Window _owner;

        public ImportExportImp(Window owner)
        {
            _owner = owner;
        }

        internal void Export(Organisation organisation)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Filter = "Json files (*.json)|*.json";

            var continueSaving = saveFileDialog.ShowDialog(_owner) == true;
            if (continueSaving)
            {
                var fileName = saveFileDialog.FileName;
                var jsonValue = JsonConvert.SerializeObject(organisation, Formatting.Indented);
                File.WriteAllText(fileName, jsonValue);
            }
        }
    }
}
