using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Plugin.Media;
using Xamarin.Forms;
using Microsoft.ProjectOxford.Emotion;

namespace Moodify
{
	public partial class HomePage : ContentPage
	{

        bool authenticated = false;

        public HomePage()
		{
			InitializeComponent();
		}

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (authenticated == true)
            {
                this.loginButton.IsVisible = false;
            }
        }

        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            if (authenticated == true)
                this.loginButton.IsVisible = false;
        }

        private async void TakePicture_Clicked(object sender, System.EventArgs e)
		{
			await CrossMedia.Current.Initialize();

			if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
			{
				await DisplayAlert("No Camera", ":( No camera available.", "OK");
				return;
			}

			var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front,
				Directory = "Moodify",
				Name = $"{DateTime.UtcNow}.jpg",
				CompressionQuality = 92
			});

			if (file == null)
				return;

			UploadingIndicator.IsRunning = true;

			string emotionKey = "88f748eefd944a5d8d337a1765414bba";

			EmotionServiceClient emotionClient = new EmotionServiceClient(emotionKey);

			var emotionResults = await emotionClient.RecognizeAsync(file.GetStream());

			UploadingIndicator.IsRunning = false;

			foreach (var type in emotionResults)
			{
				EmotionView.ItemsSource = type.Scores.ToRankedList();
			}

			image.Source = ImageSource.FromStream(() =>
			{
				var stream = file.GetStream();
				file.Dispose();
				return stream;
			});
		}
	}
}
