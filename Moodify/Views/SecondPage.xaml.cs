using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace Moodify
{
	public partial class SecondPage : ContentPage
	{
		public SecondPage()
		{
			InitializeComponent();

            GetLocation();
        }

        private async void GetLocation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

                Debug.WriteLine("Position Status: {0}", position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", position.Longitude);
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error: {0}", e.Message);
            }
        }
	}
}
