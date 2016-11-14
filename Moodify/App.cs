using Moodify.Data;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Moodify
{
	public partial class App : Application
	{

        public static IAuthenticate Authenticator { get; private set; }
        public static NavigationPage NavigationPage { get; private set; }
		public static RootPage RootPage;

        public static string ApplicationURL = @"https://mojournal.azurewebsites.net";
        static TimelineDatabase database;

        public static bool MenuIsPresented
		{
			get
			{
				return RootPage.IsPresented;
			}
			set
			{
				RootPage.IsPresented = value;
			}
		}

		public App()
		{
			var menuPage = new MenuPage();
			NavigationPage = new NavigationPage(new HomePage());
			NavigationPage.BarTextColor = Color.White;
			RootPage = new RootPage();
			RootPage.Master = menuPage;
			RootPage.Detail = NavigationPage;
			MainPage = RootPage;
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}

        public interface IAuthenticate
        {
            Task<bool> Authenticate();
        }

        public static void Init(IAuthenticate authenticator)
        {
            Authenticator = authenticator;
        }

        public static TimelineDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new TimelineDatabase();
                }
                return database;
            }
        }
    }
}
