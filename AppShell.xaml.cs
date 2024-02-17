using VisitApp.Helpers;

namespace VisitApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CaptureCardPage), typeof(CaptureCardPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));

            var server = Helpers.SettingsData.GetZkServerAddress().Result;
            if (server == string.Empty)
                this.GoToAsync($"///{nameof(LoginPage)}");
            else
            {
                //Helpers.LoginData.Clear();
                //Preferences.Default.Set("enrolled", false);
                if (IsAuthenticated().Result)
                {
                    HeaderUtil.AddUserInfoAsync().Wait();
                    this.GoToAsync($"///{nameof(MainPage)}");
                }
                else
                {
                    this.GoToAsync($"/{nameof(LoginPage)}");
                }
            }
        }

        private async void LogOut_Clicked(object sender, EventArgs e)
        {
            App.ClearUserInformation();

            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");

            Shell.Current.FlyoutIsPresented = false;
        }

        private static async Task<bool> IsAuthenticated()
        {
            return await Helpers.LoginData.IsAuthenticated();
        }
    }
}
