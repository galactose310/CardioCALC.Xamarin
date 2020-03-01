using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CardioCALC.UI.Mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public abstract partial class ViewPageBase : ContentPage
	{
		// Set focus to the next Entry after one have been completed
		public void OnEntryCompleted(object sender, EventArgs e)
		{
			if (sender is Entry entry)
			{
				if (entry.Parent is Layout<View> parent)
				{
					Entry nextEntry = SeekNextEntry(parent, parent.Children.IndexOf(entry));
					
					if (nextEntry != null) nextEntry.Focus();
				}
			}
		}

		// Get the next visible and enabled Entry, following parent/child hierarchy
		public Entry SeekNextEntry(Layout<View> parent, int currentIndex)
		{
			var stackList = parent.Children;

			for (int i = currentIndex + 1; i < stackList.Count; i++)
			{
				if (stackList.ElementAt(i) is Entry nextEntry && nextEntry.IsVisible && nextEntry.IsEnabled)
					return nextEntry;

				if (stackList.ElementAt(i) is Layout<View> childLayout && childLayout.IsVisible && childLayout.IsEnabled)
					return SeekNextEntry(childLayout, 0);
			}

			if (parent.Parent is Layout<View> parentLayout && parentLayout.IsVisible && parentLayout.IsEnabled)
				return SeekNextEntry(parentLayout, parentLayout.Children.IndexOf(parent));

			return null;
		}
	}
}