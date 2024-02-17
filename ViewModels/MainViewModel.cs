namespace VisitApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    readonly VisitorDataService dataService;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    ObservableCollection<TransactionItem> visitors;

    public MainViewModel(VisitorDataService service)
    {
        dataService = service;
    }

    [RelayCommand]
    private async Task OnRefreshing()
    {
        IsRefreshing = true;

        try
        {
            await LoadDataAsync();
        }
        finally
        {
            IsRefreshing = false;
        }
    }

    public async Task LoadDataAsync()
    {
        Visitors = new ObservableCollection<TransactionItem>(await dataService.GetLastItems());
    }
}
