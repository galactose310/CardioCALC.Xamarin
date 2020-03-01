using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CardioCALC
{
	public class Score : HasPropertyChanging, IComparable<Score>
	{
		public string PageName { get; set; }

		private string _displayName;
		public string DisplayName { get => _displayName; set => _displayName = SetFieldValueAndNotify(value); }

		private string _detail;
		public string Detail { get => _detail; set => _detail = SetFieldValueAndNotify(value); }

		private bool _isFavorite;
		public bool IsFavorite { get => _isFavorite; set => _isFavorite = SetFieldValueAndNotify(value); }

		public Score()
		{ }

		public Score(string pageName, string displayName, string detail, bool? isFavorite = null)
		{
			this.PageName = pageName;
			this.DisplayName = displayName;
			this.Detail = detail;
			this.IsFavorite = isFavorite == null ? false : (bool)isFavorite;
		}

		// Compare Scores <=> compare their DisplayName in AZ order
		public int CompareTo(Score other)
		{
			return this.DisplayName.CompareTo(other.DisplayName);
		}
	}

	public class Category : ObservableCollection<Score>
	{
		public string Name { get; set; }

		public Category(string name, IEnumerable<Score> scores)
		{
			Name = name;
			foreach (Score score in scores) this.Add(score);
		}

		public Category(string name)
		{
			Name = name;
		}
	}
}
