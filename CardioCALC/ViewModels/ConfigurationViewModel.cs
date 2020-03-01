using System;
using System.Globalization;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CardioCALC
{
	public class ConfigurationViewModel : ViewModelBase
	{
		public ICommand ChangeLocale => new Command<string>((locale) =>
		{
			CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(locale);
			CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
			Preferences.Set("locale", CultureInfo.CurrentCulture.Name);

			MessagingCenter.Send<object, CultureChangedMessage>(this, string.Empty, new CultureChangedMessage(locale));
		});
	}
}
