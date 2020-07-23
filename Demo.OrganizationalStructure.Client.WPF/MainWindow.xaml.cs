using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System.Windows;

namespace Demo.OrganizationalStructure.Client.WPF
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _mainWindowVM;

        public MainWindow()
        {
            InitializeComponent();
            _mainWindowVM = new MainWindowVM();
            DataContext = _mainWindowVM;

            _mainWindowVM.ShowMessageBox += ShowMessageBox;
        }

        private void ShowMessageBox(string message, string caption)
        {
            Activate();
            MessageBox.Show(this, message, caption);
        }
    }
}
