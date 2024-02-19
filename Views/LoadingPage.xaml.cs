using DVisit.Helpers;

namespace DVisit.Views
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            //Helpers.LoginData.Clear();
            //Preferences.Default.Set("enrolled", false);
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                //Helpers.LoginData.Clear();
                //Preferences.Default.Set("enrolled", false);
                if (await App.IsAuthenticated())
                {
                    await HeaderUtil.AddUserInfoAsync();
                    await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
                }
                else
                {
                    await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
                }

                /*
                var server = await Helpers.SettingsData.GetZkServerAddress();
                if (server == string.Empty)
                    await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
                else
                {
                    //Helpers.LoginData.Clear();
                    //Preferences.Default.Set("enrolled", false);
                    if (await App.IsAuthenticated())
                    {
                        await HeaderUtil.AddUserInfoAsync();
                        await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");
                    }
                }
                */
            });
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current?.Quit();
            return true;
        }
    }
}
