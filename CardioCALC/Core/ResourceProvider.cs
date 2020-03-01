using System;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using Xamarin.Forms;

namespace CardioCALC
{
	public class CultureChangedMessage
	{
		public CultureInfo NewCultureInfo { get; private set; }

		public CultureChangedMessage(string lngName)
			: this(new CultureInfo(lngName))
		{ }

		public CultureChangedMessage(CultureInfo newCultureInfo)
		{
			NewCultureInfo = newCultureInfo;
		}
	}

	public class ResourceProvider : IResourceProvider, INotifyPropertyChanged
	{
		// Properties defining the ResourceManager
		public ResourceManager ResourceManager { get; set; }
		public CultureInfo Culture { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		public string this[string key]
		{
			get => this.GetString(key);
		}

		// Default constructor
		public ResourceProvider(CultureInfo culture = null)
		{
			this.Culture = (culture == null) ? CultureInfo.CurrentCulture : culture;
			MessagingCenter.Subscribe<object, CultureChangedMessage>(this, string.Empty, OnCultureChanged);
		}

		// Initialize the resource provider - Constructor for embedded resources
		public ResourceProvider(string resource, CultureInfo culture = null)
			: this(culture)
		{
			this.ResourceManager = new ResourceManager(resource, this.GetType().Assembly);
		}

		// Initialize the resource provider - Constructor for satellite assemblies
		public ResourceProvider(Type resource, CultureInfo culture = null)
			: this(culture)
		{
			this.ResourceManager = new ResourceManager(resource);
		}

		// Fired when culture has changed (receiving message from MessagingCenter)
		private void OnCultureChanged(object s, CultureChangedMessage ccm)
		{
			this.Culture = ccm.NewCultureInfo;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item"));
		}

		// Get a string resource from its identification key
		public string GetString(string key, CultureInfo culture = null)
		{
			try
			{
				if (key == null) return null;
				return this.ResourceManager.GetString(key, this.Culture);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}
	}
}
