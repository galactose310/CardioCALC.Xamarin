using System;
using System.Globalization;
using Xamarin.Forms;

namespace CardioCALC.UI.Mobile
{
	// Invert a boolean value, to be used in XAML attributes like "IsVisible"
	public class InvertBooleanConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() != typeof(bool))
				throw new FormatException("InvertBooleanConverter: argument value is not of type Boolean.");

			return (bool)value == false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() != typeof(bool))
				throw new FormatException("InvertBooleanConverter: argument value is not of type Boolean.");

			return (bool)value == false;
		}
	}

	// Nullable double converter
	public class NullDoubleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null) return null;

			string stringValue = value as string;
			if (string.IsNullOrEmpty(stringValue)) return null;

			double doubleValue;
			if (double.TryParse(stringValue, NumberStyles.Any, culture, out doubleValue)
				|| double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out doubleValue))
				return doubleValue;

			return null;
		}
	}

	// Parse a Markdown text to a Xamarin.Forms FormattedString
	public class MarkdownConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() != typeof(string))
				throw new FormatException("MarkdownToFormattedString: argument value is not of type String.");

			return MarkdownParser.Parse(value.ToString());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}

	// Convert FavoriteScores count (int) in a boolean whether count is 0 or > 0
	public class FavoritesCountToBoolean : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value.GetType() != typeof(int))
				throw new FormatException("FavoritesCountToBoolean: argument value is not of type Int32.");

			return (int)value == 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value;
		}
	}
}

namespace CardioCALC.UI.Mobile
{
	public class CustomSwitch : Switch
	{
		public static readonly BindableProperty SwitchOffColorProperty
			= BindableProperty.Create(nameof(SwitchOffColor), typeof(Color), typeof(CustomSwitch), Color.Default);

		public Color SwitchOffColor
		{
			get { return (Color)GetValue(SwitchOffColorProperty); }
			set { SetValue(SwitchOffColorProperty, value); }
		}

		public static readonly BindableProperty SwitchOnColorProperty
			= BindableProperty.Create(nameof(SwitchOnColor), typeof(Color), typeof(CustomSwitch), Color.Default);

		public Color SwitchOnColor
		{
			get { return (Color)GetValue(SwitchOnColorProperty); }
			set { SetValue(SwitchOnColorProperty, value); }
		}

		public static readonly BindableProperty SwitchOnThumbColorProperty
			= BindableProperty.Create(nameof(SwitchOnThumbColor), typeof(Color), typeof(CustomSwitch), Color.Default);

		public Color SwitchOnThumbColor
		{
			get { return (Color)GetValue(SwitchOnThumbColorProperty); }
			set { SetValue(SwitchOnThumbColorProperty, value); }
		}

		public static readonly BindableProperty SwitchOffThumbColorProperty
			= BindableProperty.Create(nameof(SwitchOffThumbColor), typeof(Color), typeof(CustomSwitch), Color.Default);

		public Color SwitchOffThumbColor
		{
			get { return (Color)GetValue(SwitchOffThumbColorProperty); }
			set { SetValue(SwitchOffThumbColorProperty, value); }
		}
	}
}
