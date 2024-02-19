namespace DVisit.Controls
{
    public partial class HeaderControl : StackLayout
    {
        public HeaderControl()
        {
            InitializeComponent();

            LblFullName.Text = App.UserFullName;
            LblEmail.Text = App.UserEmail;
        }
    }
}
