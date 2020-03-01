using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CardioCALC
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MarkdownParser.HyperlinkTextColor = (Color)this.Resources["HyperlinkTextColor"];

			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(Preferences.Get("locale", CultureInfo.CurrentCulture.Name));

			MainPage = new MainPage();
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
	}
}
