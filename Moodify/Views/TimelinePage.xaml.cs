using Moodify.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Moodify.Views
{
    public partial class TimelinePage : ContentPage
    {
        public TimelinePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //TimelineView.ItemsSource = App.Database.GetItems();
            
            //var result = await AzureManager.DefaultManager.CurrentClient.InvokeApiAsync("moodRecord", HttpMethod.Get, null);

            //Debug.WriteLine(result);

            TimelineView.ItemsSource = await AzureManager.DefaultManager.GetTodoItemsAsync(true);
        }
    }
}
