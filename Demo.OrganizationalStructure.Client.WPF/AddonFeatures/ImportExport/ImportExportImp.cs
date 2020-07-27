using Demo.OrganizationalStructure.Common.DataModel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
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

        internal Organisation Import()
        {
            Organisation organisation = null;

            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json files (*.json)|*.json";

            var continueOpening = openFileDialog.ShowDialog(_owner) == true;
            if (continueOpening)
            {
                var fileName = openFileDialog.FileName;
                var jsonValue = File.ReadAllText(fileName);
                organisation = JsonConvert.DeserializeObject<Organisation>(jsonValue);
            }
            return organisation;
        }

        internal void Export(Organisation organisation)
        {
            var saveFileDialog = new SaveFileDialog();
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
