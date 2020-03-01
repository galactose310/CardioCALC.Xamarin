using System;

namespace CardioCALC
{
	class BodyMassIndex
	{
		public double Height;
		public double Weight;

		public double Result
		{
			get => Math.Round(10 * this.Weight / Math.Pow(this.Height / 100, 2)) / 10;
		}

		public BodyMassIndex()
		{
		}

		public BodyMassIndex(double height, double weight)
		{
			this.Height = height;
			this.Weight = weight;
		}
	}
}
