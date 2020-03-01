using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace CardioCALC
{
	public class HasPropertyChanging : INotifyPropertyChanged
	{
		// Will be called on every public property change
		public event PropertyChangedEventHandler PropertyChanged;

		// Returns value to a setter, and notify View the corresponding property changed
		protected T SetFieldValueAndNotify<T>(T value, [CallerMemberName]string propertyName = null)
		{
			// Exception if property does not exist
			if (this.GetType().GetProperty(propertyName) == null)
				throw new NotImplementedException($"Property {propertyName} does not exist.");

			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

			return value;
		}
	}
}
