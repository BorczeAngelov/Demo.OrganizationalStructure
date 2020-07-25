using Demo.OrganizationalStructure.Client.WPF.ViewModel;
using System.Windows;

namespace Demo.OrganizationalStructure.Client.WPF.View
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

            Loaded += AutomaticConnect;
        }

        private void ShowMessageBox(string message, string caption)
        {
            Activate();
            MessageBox.Show(this, message, caption);
        }

        private void AutomaticConnect(object sender, RoutedEventArgs e)
        {
            _mainWindowVM.ConnectCommand.Execute(null);
        }
    }
}
