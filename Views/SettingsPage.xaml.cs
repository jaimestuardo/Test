namespace DVisit.Views
{
    public partial class SettingsPage : ContentPage
    {
        readonly SettingsViewModel settingsModel;

        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = settingsModel = viewModel;
        }

        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            await settingsModel.SetServer(Server.Text.Trim());
        }
    }
}
