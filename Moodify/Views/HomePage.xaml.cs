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
		public HomePage()
		{
			InitializeComponent();
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
				Name = $"{DateTime.UtcNow}.jpg"
			});

			if (file == null)
				return;

			string emotionKey = "88f748eefd944a5d8d337a1765414bba";

			EmotionServiceClient emotionClient = new EmotionServiceClient(emotionKey);

			var emotionResults = await emotionClient.RecognizeAsync(file.GetStream());

			foreach (var type in emotionResults)
			{
				EmployeeView.ItemsSource = type.Scores.ToRankedList();
				foreach (var emo in type.Scores.ToRankedList())
				{
					System.Diagnostics.Debug.WriteLine(emo.Value);
				}
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
