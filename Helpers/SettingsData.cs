namespace DVisit.Helpers
{
    public class SettingsData
    {
        public static async Task<string> GetZkServerAddress()
        {
            var server = await SecureStorage.GetAsync("ZkServerAddress") ?? string.Empty;
            return server;
        }

        public static async Task SetZkServerAddress(string serverAddress)
        {
            await SecureStorage.SetAsync("ZkServerAddress", serverAddress);
        }
    }
}
