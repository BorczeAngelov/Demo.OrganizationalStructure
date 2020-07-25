using Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm;
using Demo.OrganizationalStructure.Client.WPF.Utils;
using Demo.OrganizationalStructure.Common.Utils;
using System;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class MainWindowVM : ObservableBase
    {
        private readonly OrgaSHubClientTwoWayComm _orgaSHubClientTwoWayComm;

        public OrganizationalStructureVM OrganizationalStructureVM { get; }

        private bool _isConnected;

        public event Action<string, string> ShowMessageBox;

        internal MainWindowVM()
        {
            var serverHubUrl = DemoUrlConstants.LocalHostUrl + DemoUrlConstants.OrgaSHubEndpoint;
            _orgaSHubClientTwoWayComm = new OrgaSHubClientTwoWayComm(serverHubUrl);

            OrganizationalStructureVM = new OrganizationalStructureVM(_orgaSHubClientTwoWayComm);

            ConnectCommand = new DelegateCommand(Connect, arg => !IsConnected);
        }

        public DelegateCommand ConnectCommand { get; }

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged();
                }
            }
        }

        private async void Connect(object obj)
        {
            try
            {
                await _orgaSHubClientTwoWayComm.ConnectWithServerHub();
                IsConnected = true;
                ConnectCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                ShowMessageBox?.Invoke(ex.Message, "Error");
            }
        }
    }
}
