using System;

namespace CardioCALC
{
	class LeftVentricleMass
	{
		public double SeptumDiameter { get; set; }
		public double LeftVentricleDiameter { get; set; }
		public double LeftVentricleWallDiameter { get; set; }

		public double Result
		{
			get => Math.Round(1.04 * 0.8 * (Math.Pow(this.SeptumDiameter / 10 + this.LeftVentricleDiameter / 10 + this.LeftVentricleWallDiameter / 10, 3) - Math.Pow(this.LeftVentricleDiameter / 10, 3)) + 0.6, 2);
		}

		public LeftVentricleMass()
		{
		}

		public LeftVentricleMass(double septumDiam, double leftVentricleDiam, double leftVentricleWallDiam)
		{
			this.SeptumDiameter = septumDiam;
			this.LeftVentricleDiameter = leftVentricleDiam;
			this.LeftVentricleWallDiameter = leftVentricleWallDiam;
		}
	}
}
