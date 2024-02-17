namespace VisitApp.Services;

public class VisitorDataService
{
    readonly RestUtilityService _api;

    public VisitorDataService(RestUtilityService api)
    {
        _api = api;
    }

    public async Task<IEnumerable<TransactionItem>> GetLastItems()
    {
        var result = (await _api.GetDataAsync<List<TransactionItem>>("/api/Visitor/GetLastVisitors")).OrderByDescending(c => c.DateAndTime);

        return result;
    }

    public async Task<VisitorItem> GetByCard(string card)
    {
        var result = await _api.GetDataAsync<VisitorItem>($"/api/Visitor/GetByCard/{card}");
        return result;
    }

    public async Task<bool> AllowAccessByCard(string card)
    {
        var result = await _api.GetDataAsync<bool>($"/api/Visitor/AllowAccessByCard/{card}");
        return result;
    }
}
