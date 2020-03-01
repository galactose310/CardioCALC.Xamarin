using System.ComponentModel;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace CardioCALC
{
	// Learn more about making custom code visible in the Xamarin.Forms previewer
	// by visiting https://aka.ms/xamarinforms-previewer
	[DesignTimeVisible(false)]
	public partial class MainPage : Xamarin.Forms.TabbedPage
	{
		protected ConfigurationViewModel ViewModel { get; } = new ConfigurationViewModel();

		public MainPage()
		{
			On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);

			InitializeComponent();

			this.BindingContext = ViewModel;
		}
	}
}
