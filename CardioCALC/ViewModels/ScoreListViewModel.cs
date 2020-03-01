using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace CardioCALC
{
	public class ScoreListViewModel : ViewModelBase
	{
		// XML file defining scores and categories
		private const string XMLFile = "CardioCALC.Models.ScoreList.xml";

		// Lists of categories and scores : Categories<Score> (not ordered) / all scores (order AZ) / favorites (order AZ)
		public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();
		public ObservableCollection<Score> Scores { get; set; } = new ObservableCollection<Score>();
		public ObservableCollection<Score> FavoriteScores { get; set; } = new ObservableCollection<Score>();

		// Favorites is a collection of string ==> access to Singleton instance
		public ObservableCollection<string> Favorites { get => FavoritesManager.Instance.Favorites; }
		
		// Singleton instance for this viewmodel ==> shared between ScoreListPage & FavoritesListPage
		public static ScoreListViewModel Instance { get; private set; } = new ScoreListViewModel();

		// Static and private constructors for Singleton instance
		static ScoreListViewModel()
		{ }

		private ScoreListViewModel()
		{
			this.InitializeScoreList();

			// Suscribe to events for locale resources changes
			this.ScoreResources.PropertyChanged += OnResourcesChanged;
		}

		// If locale changes, reload all the collection
		private void OnResourcesChanged(object sender, PropertyChangedEventArgs e)
		{
			this.InitializeScoreList();
		}

		// Toggle favorite status (raised from ICommand in XAML)
		public void ToggleFavorites(Score score)
		{
			score.IsFavorite = !score.IsFavorite;

			if (score.IsFavorite)
			{
				this.Favorites.Add(score.PageName);
				this.FavoriteScores.AddSorted(score);
			}
			else
			{
				this.Favorites.Remove(score.PageName);
				this.FavoriteScores.Remove(score);
			}
		}

		// Initialize the collection of categories and scores
		private void InitializeScoreList()
		{
			this.Categories.Clear();
			this.Scores.Clear();
			this.FavoriteScores.Clear();

			List<Score> scoreList = new List<Score>();
			foreach(Category category in this.GetCategoriesFromXmlFile())
			{
				this.Categories.Add(category);
				foreach (Score score in category) scoreList.Add(score);
			}

			foreach (Score score in scoreList.OrderBy(score => score.DisplayName).ToList())
			{
				this.Scores.Add(score);
				if (score.IsFavorite) this.FavoriteScores.AddSorted(score);
			}
		}

		// Retrive list of categories and score from an XML file (embedded resource)
		private List<Category> GetCategoriesFromXmlFile()
		{
			List<Category> categoriesList = new List<Category>();

			// Read XML file and close it
			XDocument xml = new XDocument();
			Stream stream = this.GetType().Assembly.GetManifestResourceStream(XMLFile);
			using (var reader = new System.IO.StreamReader(stream))
			{
				xml = XDocument.Load(reader);
			}

			// At each XML node, build one category and its scores
			foreach (XElement category in xml.Descendants("category"))
			{
				IEnumerable<Score> scoreList = from s in category.Descendants("score")
											   select new Score()
											   {
												   PageName = s.Attribute("name").Value,
												   DisplayName = this.ScoreResources[s.Attribute("name").Value],
												   Detail = this.ScoreResources[s.Attribute("name").Value + "_Detail"],
												   IsFavorite = this.Favorites.Contains(s.Attribute("name").Value)
											   };

				categoriesList.Add(new Category(this.ScoreResources[category.Attribute("name").Value], scoreList));
			}

			// List of caterogies ; each category is an ObservableCollection of scores
			return categoriesList;
		}
	}
}