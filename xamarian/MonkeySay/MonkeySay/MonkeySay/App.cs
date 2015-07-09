using System;
using MonkeySay.Views;
using Xamarin.Forms;

namespace MonkeySay
{
	public class App : Application
	{
        private static App _instanceApp;
		public App ()
		{
		    _instanceApp = this;

			// The root page of your application
            _mainPage = new MonkeyPage();
		    MainPage = _mainPage;
		}

        public static App Instance
        {
            get { return _instanceApp; }
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

	    //private ContentPage _bonusPage;
        public void ChangePageToBonus()
	    {
            MainPage = new BonusReorganisePage();
	        //MainPage = _bonusPage ?? (_bonusPage = new BonusReorganisePage());
	    }

	    private ContentPage _mainPage;
	    public void ChangePageToMain()
	    {
	        MainPage = _mainPage ?? (_mainPage = new MonkeyPage());
	    }
	}
}
