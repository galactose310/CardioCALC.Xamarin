using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CardioCALC.UI.Mobile;

namespace CardioCALC
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FavoritesListPage : ViewPageBase
	{
		protected ScoreListViewModel ViewModel { get; } = ScoreListViewModel.Instance;

		public FavoritesListPage()
		{
			InitializeComponent();

			this.BindingContext = ViewModel;
		}

		// Command to add/remove favorites from the list
		public ICommand ToogleFavorites => new Command<Score>((fav) =>
		{
			this.ViewModel.ToggleFavorites(fav);
		});
	}
}