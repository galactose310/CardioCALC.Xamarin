namespace CardioCALC
{
	public class PhhfGroupScore
	{
		public bool Diabetes { get; set; }
		public bool AtrialFibrillation { get; set; }
		public double RightVentricleArea { get; set; }
		public double LeftAtriumArea { get; set; }
		public double LeftVentricleMass { get; set; }

		public int Result
		{
			get
			{
				int score = 0; // Initialize score

				if (this.Diabetes) score += 1;
				if (this.AtrialFibrillation) score += 2;
				if (this.RightVentricleArea < 27) score += 2;

				if (15 <= this.LeftAtriumArea && this.LeftAtriumArea < 19) score += 1;
				else if (19 <= this.LeftAtriumArea && this.LeftAtriumArea < 24) score += 2;
				else if (this.LeftAtriumArea >= 24) score += 3;

				if (46 < this.LeftVentricleMass && this.LeftVentricleMass <= 62) score += 1;
				else if (62 < this.LeftVentricleMass && this.LeftVentricleMass <= 81) score += 2;
				else if (this.LeftVentricleMass > 81) score += 3;

				return score;
			}
		}

		public PhhfGroupScore()
		{
		}

		public PhhfGroupScore(bool diabetes, bool atrialFibrillation, double rightVentricleArea, double leftAtriumArea, double leftVentricleMass)
		{
			this.Diabetes = diabetes;
			this.AtrialFibrillation = atrialFibrillation;
			this.RightVentricleArea = rightVentricleArea;
			this.LeftAtriumArea = leftAtriumArea;
			this.LeftVentricleMass = leftVentricleMass;
		}
	}
}
