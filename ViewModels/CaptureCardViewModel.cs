namespace VisitApp.ViewModels
{
    [QueryProperty(nameof(Visitor), "Visitor")]
    public partial class CaptureCardViewModel : BaseViewModel
    {
        readonly VisitorDataService dataService;
        readonly IDispatcherTimer timer;

        [ObservableProperty]
        private bool isManualButtonVisible = true;

        [ObservableProperty]
        private bool isAllowedMessageVisible;

        [ObservableProperty]
        private bool isDeniedMessageVisible;

        [ObservableProperty]
        private VisitorItem visitor;

        public CaptureCardViewModel(VisitorDataService service)
        {
            dataService = service;

            timer = Application.Current.Dispatcher.CreateTimer();
            timer.Interval = TimeSpan.FromSeconds(6);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsAllowedMessageVisible = false;
                IsDeniedMessageVisible = false;
                IsManualButtonVisible = true;
            });
        }

        protected override async void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Visitor))
            {
                IsManualButtonVisible = Visitor == null;
                IsAllowedMessageVisible = Visitor != null && Visitor.valid;
                IsDeniedMessageVisible = Visitor != null && !Visitor.valid;

                if (IsAllowedMessageVisible)
                    await dataService.AllowAccessByCard(Visitor.documentNumber);

                if (Visitor != null)
                    timer.Start();
            }
            else
                base.OnPropertyChanged(e);
        }
    }
}
