using VisitApp.Helpers;

namespace VisitApp.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        /*
        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            var server = await Helpers.SettingsData.GetZkServerAddress();
            if (server == string.Empty)
                await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
            else
            {
            //Helpers.LoginData.Clear();
            //Preferences.Default.Set("enrolled", false);
            if (await IsAuthenticated())
            {
                await HeaderUtil.AddUserInfoAsync();
                await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"/{nameof(LoginPage)}");
            }
            //}
        }
        */

        private static async Task<bool> IsAuthenticated()
        {
            return await Helpers.LoginData.IsAuthenticated();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.Quit();
            return true;
        }
    }
}
