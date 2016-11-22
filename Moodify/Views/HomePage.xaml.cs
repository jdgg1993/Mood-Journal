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

            string userId = CrossSettings.Current.GetValueOrDefault("user", "");
            string token = CrossSettings.Current.GetValueOrDefault("token", "");

            if (!token.Equals("") && !userId.Equals(""))
            {
                MobileServiceUser user = new MobileServiceUser(userId);
                user.MobileServiceAuthenticationToken = token;

                AzureManager.DefaultManager.CurrentClient.CurrentUser = user;

                authenticated = true;
            }

            if (authenticated == true)
                this.loginButton.IsVisible = false;
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

                errorLabel.Text = "";

                string emotionKey = "88f748eefd944a5d8d337a1765414bba";

                EmotionServiceClient emotionClient = new EmotionServiceClient(emotionKey);

                var emotionResults = await emotionClient.RecognizeAsync(file.GetStream());

                UploadingIndicator.IsRunning = false;

                var temp = emotionResults[0].Scores;
                Emotion emo = new Emotion()
                {
                    anger = temp.Anger,
                    contempt = temp.Contempt,
                    disgust = temp.Disgust,
                    fear = temp.Fear,
                    happiness = temp.Happiness,
                    neutral = temp.Neutral,
                    sadness = temp.Sadness,
                    surprise = temp.Surprise,
                    createdAt = DateTime.Now
                };

                EmotionView.ItemsSource = temp.ToRankedList();

                AzureManager.DefaultManager.CurrentClient.InvokeApiAsync<Emotion, string>("moodRecord", emo);

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
