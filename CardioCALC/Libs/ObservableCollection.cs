using System.Collections.Generic;

namespace System.Collections.ObjectModel
{
	public static class ObservableCollection
	{
		public static void AddSorted<T>(this ObservableCollection<T> source, T item, IComparer<T> comparer = null)
		{
			if (comparer == null)
				comparer = Comparer<T>.Default;

			int i = 0;
			while (i < source.Count && comparer.Compare(source[i], item) < 0)
				i++;
			source.Insert(i, item);
		}
	}
}
