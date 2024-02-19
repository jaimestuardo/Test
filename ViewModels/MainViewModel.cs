namespace DVisit.ViewModels;

public partial class MainViewModel(VisitorDataService service) : BaseViewModel
{
    readonly VisitorDataService dataService = service;

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    ObservableCollection<TransactionItem>? visitors;

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
