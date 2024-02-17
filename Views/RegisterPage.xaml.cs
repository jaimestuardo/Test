namespace VisitApp.Views
{
    public partial class RegisterPage : ContentPage
    {
        private bool isProcessing;
        readonly VisitorDataService dataService;

        public RegisterPage(VisitorDataService service)
        {
            dataService = service;

            InitializeComponent();

            Shell.SetTabBarIsVisible(this, false);
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            ID.Text = string.Empty;
            ID.Focus();
            base.OnNavigatedTo(args);
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (!isProcessing)
            {

                isProcessing = true;
                var card = ID.Text.Trim();
                if (card == string.Empty)
                {
                    await DisplayAlert("Error", "Debes ingresar el ID o pasaporte de la visita.", "Intentar de nuevo");
                }
                else
                {
                    Waiting.IsRunning = aiLayout.IsVisible = true;
                    var visitor = await dataService.GetByCard(card);
                    await Shell.Current.GoToAsync("..", new Dictionary<string, object> { { "Visitor", visitor } });
                    isProcessing = false;
                    Waiting.IsRunning = aiLayout.IsVisible = false;
                }
            }
        }
    }
}
