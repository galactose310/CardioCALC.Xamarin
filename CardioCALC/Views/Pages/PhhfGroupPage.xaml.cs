using System;
using Xamarin.Forms.Xaml;
using CardioCALC.UI.Mobile;

namespace CardioCALC
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PhhfGroupPage : ViewPageBase
	{
		protected PhhfGroupViewModel ViewModel { get; } = new PhhfGroupViewModel();

		public PhhfGroupPage()
		{
			InitializeComponent();

			this.BindingContext = ViewModel;
		}

		protected void OnCalculateClicked(object sender, EventArgs e)
		{
			try
			{
				DisplayAlert(
					$"{ViewModel.Resources["PhhfGroup_Name"]} : {ViewModel.Result}",
					$"{ViewModel.Interpretation}",
					"OK");
			}
			catch (PropertyNullException exception)
			{
				//DisplayAlert(Properties.Resources.UserError, exception.Message, "OK");
				DisplayAlert(ViewModel.Resources["UserError"], exception.Message, "OK");
			}
			catch (DivideByZeroException exception)
			{
				DisplayAlert(ViewModel.Resources["UserError"], exception.Message, "OK");
			}
			catch (Exception exception)
			{
				DisplayAlert(ViewModel.Resources["UserError"], exception.Message, "OK");
			}
		}
	}
}