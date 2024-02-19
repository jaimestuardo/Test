namespace DVisit.Views
{
    public partial class LoginPage : ContentPage
    {
        private bool isProcessing;
        readonly RestUtilityService _api;

        public LoginPage(RestUtilityService rest)
        {
            InitializeComponent();
            _api = rest;

            Username.Text = Password.Text = string.Empty;
            Loaded += MainPage_Loaded;
        }

        private void MainPage_Loaded(object? sender, EventArgs e)
        {
            Username.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current?.Quit();
            return true;
        }

        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (!isProcessing)
            {
                isProcessing = true;
                var username = Username.Text.Trim();
                var password = Password.Text.Trim();
                if (username == string.Empty || password == string.Empty)
                {
                    await DisplayAlert("Error", "Debes ingresar tu nombre de usuario y contraseña.", "Intentar de nuevo");
                }
                else
                {
                    Waiting.IsRunning = aiLayout.IsVisible = true;
                    if (await IsCredentialCorrect(username, password))
                    {
                        await Shell.Current.GoToAsync($"///{nameof(LoadingPage)}");
                    }
                    else
                    {
                        //await DisplayAlert("Login incorrecto", "Nombre de usuario o contraseñas incorrectos.", "Intentar de nuevo");
                    }
                    Waiting.IsRunning = aiLayout.IsVisible = false;
                }
                isProcessing = false;
            }
        }

        private async Task<bool> IsCredentialCorrect(string username, string password)
        {
            var result = await _api.PostDataAsync<AuthenticatedUser>("/api/Account/Login", new { usuario = username, clave = password });
            if (result != null)
            {
                await Helpers.LoginData.Set(
                    true,
                    result.id ?? string.Empty,
                    result.firstName ?? string.Empty,
                    result.lastName ?? string.Empty,
                    result.email ?? string.Empty
                );
            }

            return result != null;
        }
    }
}
