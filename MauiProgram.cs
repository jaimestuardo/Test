using Camera.MAUI;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.RegularExpressions;
using UraniumUI;

namespace VisitApp
{
    public static partial class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream("VisitApp.appsettings.json");

            var config = new ConfigurationBuilder()
                        .AddJsonStream(stream)
                        .Build();
            builder.Configuration.AddConfiguration(config);

            builder
                .UseMauiApp<App>()
                .UseMauiCameraView()
                .UseMauiCommunityToolkit()
                .UseUraniumUI()
                .UseUraniumUIMaterial()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("FontAwesome6FreeBrands.otf", "FontAwesomeBrands");
                    fonts.AddFont("FontAwesome6FreeRegular.otf", "FontAwesomeRegular");
                    fonts.AddFont("FontAwesome6FreeSolid.otf", "FontAwesomeSolid");
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services
                .AddSingleton<ShellViewModel>()
                .AddSingleton<MainViewModel>()
                .AddSingleton<CaptureCardViewModel>()
                .AddSingleton<SettingsViewModel>()
                .AddSingleton<MainPage>()
                .AddTransient<VisitorDataService>()
                .AddTransient(factory =>
                {
                    var rest = new RestUtilityService(factory.GetService<IConfiguration>());
                    rest.Exception += Api_Exception;
                    return rest;
                })
                .AddSingleton<LoadingPage>()
                .AddSingleton<LoginPage>()
                .AddSingleton<CaptureCardPage>()
                .AddSingleton<RegisterPage>()
                .AddSingleton<SettingsPage>();

            CultureInfo.CurrentCulture = CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("es-CL");

            return builder.Build();
        }

        static async private void Api_Exception(object sender, ThreadExceptionEventArgs e)
        {
            string message = e.Exception.Message;
            Regex regex = ExtractTitle();
            var match = regex.Match(message);
            await Application.Current.MainPage.DisplayAlert(match.Success ? match.Value : "Error", match.Success ? message.Replace("[" + match.Value + "]", string.Empty).Trim() : message, "Aceptar");
        }

        [GeneratedRegex("(?<=^\\[).+?(?=\\])")]
        private static partial Regex ExtractTitle();
    }
}
