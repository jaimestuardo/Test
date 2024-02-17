using VisitApp.Controls;

namespace VisitApp.Helpers
{
    public class HeaderUtil
    {
        public async static Task AddUserInfoAsync()
        {
            await App.SetUserInformation();
            AppShell.Current.FlyoutHeader = new HeaderControl();
        }
    }
}
