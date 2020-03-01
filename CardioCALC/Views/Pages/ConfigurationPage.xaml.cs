using Xamarin.Forms.Xaml;
using CardioCALC.UI.Mobile;

namespace CardioCALC
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ConfigurationPage : ViewPageBase
	{
		protected ConfigurationViewModel ViewModel { get; } = new ConfigurationViewModel();

		public ConfigurationPage()
		{
			InitializeComponent();

			this.BindingContext = ViewModel;
		}
	}
}