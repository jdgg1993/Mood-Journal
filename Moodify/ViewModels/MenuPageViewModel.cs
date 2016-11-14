using Moodify.Views;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Moodify
{
	public class MenuPageViewModel
	{
		public ICommand GoHomeCommand { get; set; }
		public ICommand GoSecondCommand { get; set; }
        public ICommand GoTimelineCommand { get; set; }

        public MenuPageViewModel()
		{
			GoHomeCommand = new Command(GoHome);
			GoSecondCommand = new Command(GoSecond);
            GoTimelineCommand = new Command(GoTimeline);

        }

		void GoHome(object obj)
		{
			App.NavigationPage.Navigation.PopToRootAsync();
			App.MenuIsPresented = false;
		}

		void GoSecond(object obj)
		{
			App.NavigationPage.Navigation.PushAsync(new SecondPage());
			App.MenuIsPresented = false;
		}
        void GoTimeline(object obj)
        {
            App.NavigationPage.Navigation.PushAsync(new TimelinePage());
            App.MenuIsPresented = false;
        }
    }
}
