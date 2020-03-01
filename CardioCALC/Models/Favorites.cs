using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace CardioCALC
{
	public sealed class FavoritesManager
	{
		public ObservableCollection<string> Favorites { get; private set; }

		// Instance for Singleton pattern
		public static FavoritesManager Instance { get; } = new FavoritesManager();

		// Singleton pattern : no other instance permitted ==> static & private constructor
		static FavoritesManager()
		{ }

		private FavoritesManager()
		{
			this.LoadSavedFavorites();

			this.Favorites.CollectionChanged += OnFavoritesChanged;
		}

		// What to do when favorites list has been updated
		public void OnFavoritesChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			this.SaveFavorites();
		}

		// Load saved favorites (from prefs or db) at start-up ==> from string with ',' separator to collection
		private void LoadSavedFavorites()
		{
			this.Favorites = new ObservableCollection<string>(Xamarin.Essentials.Preferences.Get("favorites", string.Empty).Split(',').Where(item => item != string.Empty));
		}

		// Save favorites list (to prefs or db) ==> from collection to string with ',' separator
		private void SaveFavorites()
		{
			string favorites = null;
			foreach (string favorite in this.Favorites)
			{
				favorites += $"{favorite},";
			}
			Xamarin.Essentials.Preferences.Set("favorites", favorites);
		}
	}
}
