using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace CardioCALC
{
	// Interface for ResourceProvider : must return a string "value" retrieve from a "key" string
	public interface IResourceProvider
	{
		ResourceManager ResourceManager { get; set; }
		CultureInfo Culture { get; set; }
		string GetString(string key, CultureInfo culture);
		string this[string key] { get; }
	}
}
