using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Plugin.Media;
using Xamarin.Forms;
using Microsoft.ProjectOxford.Emotion;
using Moodify.Model;
using System.Diagnostics;
using Microsoft.WindowsAzure.MobileServices;
using Plugin.Settings;

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

            this.loginButton.IsVisible = false;

            try
            {
                MobileServiceUser user = new MobileServiceUser(CrossSettings.Current.GetValueOrDefault("user", ""));
                user.MobileServiceAuthenticationToken = CrossSettings.Current.GetValueOrDefault("token", "");

                AzureManager.DefaultManager.CurrentClient.CurrentUser = user;

                this.loginButton.IsVisible = false;
            }
            catch
            {
                this.loginButton.IsVisible = true;
            }

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
            {
                this.loginButton.IsVisible = false;
                CrossSettings.Current.AddOrUpdateValue("user", AzureManager.DefaultManager.CurrentClient.CurrentUser.UserId);
                CrossSettings.Current.AddOrUpdateValue("token", AzureManager.DefaultManager.CurrentClient.CurrentUser.MobileServiceAuthenticationToken);

            }
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

            try
            {

                string emotionKey = "88f748eefd944a5d8d337a1765414bba";

                EmotionServiceClient emotionClient = new EmotionServiceClient(emotionKey);

                var emotionResults = await emotionClient.RecognizeAsync(file.GetStream());

                UploadingIndicator.IsRunning = false;

                var temp = emotionResults[0].Scores;
                Timeline emo = new Timeline()
                {
                    Anger = temp.Anger,
                    Contempt = temp.Contempt,
                    Disgust = temp.Disgust,
                    Fear = temp.Fear,
                    Happiness = temp.Happiness,
                    Neutral = temp.Neutral,
                    Sadness = temp.Sadness,
                    Surprise = temp.Surprise,
                    createdAt = DateTime.Now
                };

                EmotionView.ItemsSource = temp.ToRankedList();

                App.Database.SaveItem(emo);

                image.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });
            }
            catch (Exception ex)
            {
                errorLabel.Text = ex.Message;
            }
		}
	}
}
