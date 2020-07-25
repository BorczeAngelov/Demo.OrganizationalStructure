using Demo.OrganizationalStructure.Client.WPF.HubClientTwoWayComm;
using Demo.OrganizationalStructure.Client.WPF.Utils;
using System;
using System.ComponentModel;

namespace Demo.OrganizationalStructure.Client.WPF.ViewModel
{
    public class MainWindowVM : ObservableBase
    {
        private readonly OrgaSHubClientTwoWayComm _orgaSHubClientTwoWayComm;

        public OrganizationalStructureVM OrganizationalStructureVM { get; private set; }

        private bool _isConnected;

        public event Action<string, string> ShowMessageBox;

        internal MainWindowVM()
        {
            _orgaSHubClientTwoWayComm = new OrgaSHubClientTwoWayComm();
            OrganizationalStructureVM = new OrganizationalStructureVM(_orgaSHubClientTwoWayComm);

            ConnectCommand = new DelegateCommand(Connect, arg => !IsConnected);

            PropertyChanged += RefreshCommandsWhenIsConnectedChanges;
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
            }
            catch (Exception ex)
            {
                ShowMessageBox?.Invoke(ex.Message, "Error");
            }
        }

        private void RefreshCommandsWhenIsConnectedChanges(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsConnected))
            {
                ConnectCommand.RaiseCanExecuteChanged();
            }
        }
    }
}
