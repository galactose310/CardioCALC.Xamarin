using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CardioCALC.UI.Mobile;

namespace CardioCALC
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScoreListPage : ViewPageBase
	{
		protected ScoreListViewModel ViewModel { get; } = ScoreListViewModel.Instance;

		public ScoreListPage()
		{
			InitializeComponent();
			
			this.BindingContext = ViewModel;
		}

		// Command to add/remove favorites from the list
		public ICommand ToogleFavorites => new Command<Score>((fav) =>
		{
			this.ViewModel.ToggleFavorites(fav);
		});



		/// FONCTION POUR ANIMATION (scale ?) : Click sur image favorites ==> déclenche l'évènement + l'animation via la Commande ToogleFavorites ==> renvoie vers la fonction du viewmodel (comme déjà fait) + voir si ça peut repérer le "sender" pour caler l'animation dessus...
		
		/// PAR AILLEURS REVOIR LE VIEWMODELBASE ET LE VIEWBASE : pour la gestion des erreurs, propriété ViewModelError et propriété ViewModelErrorMessage ou même seulement la 1ère, gérée avec une fonction ResetError(); ==> Binding sur cette propriété dans le viewpagebase, qui déclenche un DisplayAlert() approprié ==> le DisplayAlert => "OK !" actionne la fonction ResetError() du viewmodel.
	}
}