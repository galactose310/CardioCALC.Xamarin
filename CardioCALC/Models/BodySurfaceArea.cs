using System;

namespace CardioCALC
{
	class BodySurfaceArea
	{
		public double Height { get; set; }
		public double Weight { get; set; }

		public double Result
		{
			get => Math.Round(1000 * 0.0003207 * Math.Pow(this.Weight * 1000, (0.7285 - (0.0188 * Math.Log10(this.Weight * 1000)))) * Math.Pow(this.Height, 0.3)) / 1000;
		}

		public BodySurfaceArea()
		{
		}

		public BodySurfaceArea(double height, double weight)
		{
			this.Height = height;
			this.Weight = weight;
		}
	}
}
