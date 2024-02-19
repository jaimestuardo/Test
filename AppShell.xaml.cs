namespace DVisit
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CaptureCardPage), typeof(CaptureCardPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
        }

        private async void LogOut_Clicked(object sender, EventArgs e)
        {
            App.ClearUserInformation();

            await Shell.Current.GoToAsync($"///{nameof(LoginPage)}");

            Shell.Current.FlyoutIsPresented = false;
        }
    }
}
