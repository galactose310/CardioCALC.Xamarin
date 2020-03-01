using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace CardioCALC
{
	public abstract class ViewModelBase : HasPropertyChanging
	{
		// Resources providers
		public ResourceProvider Resources { get; private set; }
		public ResourceProvider ScoreResources { get; private set; }

		// Initializing a new ViewModel : setting services 
		public ViewModelBase()
		{
			this.Resources = new ResourceProvider(typeof(Locale.Resources));
			this.ScoreResources = new ResourceProvider(typeof(Locale.Scores));
		}

		// From a list of properties names, returns which ones are Null
		public List<string> GetNullPropertiesList(params string[] properties)
		{
			List<string> nullPropertiesList = new List<string>();

			foreach (string property in properties)
			{
				// Find the property ; if not found, prop == null ==> do nothing
				PropertyInfo prop = this.GetType().GetProperty(property);

				// If property value is null, add its locale name to the list
				if (prop != null && IsNullable(prop.PropertyType) && prop.GetValue(this, null) == null)
					nullPropertiesList.Add(this.Resources[property]);
			}

			return nullPropertiesList;
		}

		// Chekcs if a type is Nullable<T>
		public static bool IsNullable(Type type)
		{
			return Nullable.GetUnderlyingType(type) != null;
		}

		// Command for clickable hyperlinks
		public ICommand OpenNavigatorToURL => new Command<string>((url) =>
		{
			Device.OpenUri(new Uri(url));
		});
	}
}
