using MonkeySay.Views;
using Xamarin.Forms;

namespace MonkeySay
{
	public class App : Application
	{
		public App ()
		{
			// The root page of your application
			MainPage = new MonkeyPage();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
