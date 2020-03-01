using System;
using System.Collections.Generic;

namespace CardioCALC
{
	/// <summary>
	///		Exception definition for empty properties found in a ViewModel
	/// </summary>
	public class PropertyNullException : ArgumentNullException
	{
		// List of empty properties
		public ICollection<string> NullPropertiesList { get; private set; }

		// Partial string to initialize message
		protected string MessageIfOne { get; set; }
		protected string MessageIfMany { get; set; }

		// Default message to be printed
		public override string Message
		{
			get
			{
				string propertiesToString = "";
				foreach (string property in this.NullPropertiesList)
				{
					propertiesToString += $"{property}, ";
				}
				return $"{(this.NullPropertiesList.Count > 1 ? this.MessageIfMany : this.MessageIfOne)} {propertiesToString.Substring(0, propertiesToString.Length - 2) }.";
			}
		}

		// Constructor : fatal error if exception not properly set
		public PropertyNullException(ICollection<string> propertiesList, string messageIfOne = null, string messageIfMany = null)
		{
			this.MessageIfOne = messageIfOne == null ? "The following field is not correctly filled : " : messageIfOne;
			this.MessageIfMany = messageIfMany == null ? "The following fields are not correctly filled : " : messageIfMany;

			if (propertiesList.Count < 1)
				throw new Exception("Undefined properties list in PropertyNullException.");

			this.NullPropertiesList = propertiesList;
		}
	}
}