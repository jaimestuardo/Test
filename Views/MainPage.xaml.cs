namespace VisitApp.Views
{
    public partial class MainPage : ContentPage
    {
        readonly MainViewModel mainModel;

        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = mainModel = viewModel;

            Shell.SetTabBarIsVisible(this, false);
            Shell.SetFlyoutBehavior(this, FlyoutBehavior.Flyout);
        }

        private async void AddVisitorButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"/{nameof(CaptureCardPage)}");
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            Waiting.IsRunning = aiLayout.IsVisible = true;

            await mainModel.LoadDataAsync();

            Waiting.IsRunning = aiLayout.IsVisible = false;
        }
    }

}
