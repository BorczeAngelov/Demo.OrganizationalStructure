using Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm;
using System.Windows;

namespace Demo.OrganizationalStructure.Client.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly OrgaSHubClientTwoWayComm _orgaSHubClientTwoWayComm;

        public MainWindow()
        {
            InitializeComponent();

            _orgaSHubClientTwoWayComm = new OrgaSHubClientTwoWayComm();
        }

        private async void Connect(object sender, RoutedEventArgs e)
        {
            await _orgaSHubClientTwoWayComm.ConnectWithServerHub();
        }
    }
}
