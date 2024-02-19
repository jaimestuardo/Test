namespace DVisit.Views
{
    public partial class CaptureCardPage : ContentPage
    {
        readonly CaptureCardViewModel captureCardModel;

        public CaptureCardPage(CaptureCardViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = captureCardModel = viewModel;

            Shell.SetTabBarIsVisible(this, false);

            cameraView.BarCodeDetectionEnabled = true;
            cameraView.CamerasLoaded += CameraView_CamerasLoaded;
            cameraView.BarcodeDetected += CameraView_BarcodeDetected;
        }

        private void CameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var reading = args.Result[0].Text;
            });
        }

        private async void ManualButton_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"/{nameof(RegisterPage)}");
        }

        private void CameraView_CamerasLoaded(object? sender, EventArgs e)
        {
            if (cameraView.NumCamerasDetected > 0)
            {
                if (cameraView.NumMicrophonesDetected > 0)
                    cameraView.Microphone = cameraView.Microphones.First();
                cameraView.Camera = cameraView.Cameras.First();
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Task.Delay(500);
                    await cameraView.StopCameraAsync();
                    await cameraView.StartCameraAsync();
                    /*
                    if (await cameraView.StartCameraAsync() == CameraResult.Success)
                    {

                    }
                    */
                });
            }
        }
    }
}
