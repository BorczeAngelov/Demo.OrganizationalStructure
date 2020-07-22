using Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm;
using System;
using System.Windows;

namespace Demo.OrganizationalStructure.Client.WPF
{
    public partial class MainWindow : Window
    {
        private readonly OrgaSHubClientTwoWayComm _orgaSHubClientTwoWayComm;

        public MainWindow()
        {
            InitializeComponent();
            PingServerButton.IsEnabled = false;

            _orgaSHubClientTwoWayComm = new OrgaSHubClientTwoWayComm();

            _orgaSHubClientTwoWayComm.PongedDemo += ResponeToPongFromServer;
        }

        private async void Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                await _orgaSHubClientTwoWayComm.ConnectWithServerHub();
                ConnectButton.IsEnabled = false;
                PingServerButton.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void PingServer(object sender, RoutedEventArgs e)
        {
            await _orgaSHubClientTwoWayComm.ServerHubProxy.PingDemo();
        }

        private void ResponeToPongFromServer()
        {
            MessageBox.Show(this, "Pong", "Message from Server");
        }
    }
}
