using DVisit.Helpers;

namespace DVisit.ViewModels
{
    public partial class SettingsViewModel : BaseViewModel
    {
        [ObservableProperty]
        string zkServerAddress;

        public SettingsViewModel()
        {
            ZkServerAddress = SettingsData.GetZkServerAddress().Result;
        }

        public async Task SetServer(string serverAddress)
        {
            ZkServerAddress = serverAddress;
            await SettingsData.SetZkServerAddress(serverAddress);
        }
    }
}
