namespace DVisit.Services;

public class VisitorDataService(RestUtilityService api)
{
    readonly RestUtilityService _api = api;

    public async Task<IEnumerable<TransactionItem>> GetLastItems()
    {
        var result = (await _api.GetDataAsync<List<TransactionItem>>("/api/Visitor/GetLastVisitors"))?.OrderByDescending(c => c.DateAndTime);

        return result?.ToList() ?? [];
    }

    public async Task<VisitorItem ?> GetByCard(string card)
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
