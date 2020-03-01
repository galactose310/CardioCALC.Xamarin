using System;
using System.Collections.Generic;

namespace CardioCALC
{
	public class PhhfGroupViewModel : ViewModelBase
	{
		private bool _diabetes = false;
		public bool Diabetes { get => _diabetes; set => _diabetes = SetFieldValueAndNotify(value); }

		private bool _atrialFibrillation = false;
		public bool AtrialFibrillation { get => _atrialFibrillation; set => _atrialFibrillation = SetFieldValueAndNotify(value); }

		private double? _rightVentricleArea;
		public double? RightVentricleArea { get => _rightVentricleArea; set => _rightVentricleArea = SetFieldValueAndNotify(value); }

		private double? _leftAtriumArea;
		public double? LeftAtriumArea { get => _leftAtriumArea; set => _leftAtriumArea = SetFieldValueAndNotify(value); }

		private double? _leftVentricleMass;
		public double? LeftVentricleMass { get => _leftVentricleMass; set => _leftVentricleMass = SetFieldValueAndNotify(value); }

		private bool _lvMassIsKnown = true;
		public bool LVMassIsKnown { get => _lvMassIsKnown; set => _lvMassIsKnown = SetFieldValueAndNotify(value); }

		private double? _weight;
		public double? Weight { get => _weight; set => _weight = SetFieldValueAndNotify(value); }

		private double? _height;
		public double? Height { get => _height; set => _height = SetFieldValueAndNotify(value); }

		private double? _leftVentricleSeptumDiam;
		public double? LeftVentricleSeptumDiam { get => _leftVentricleSeptumDiam; set => _leftVentricleSeptumDiam = SetFieldValueAndNotify(value); }

		private double? _leftVentricleWallDiam;
		public double? LeftVentricleWallDiam { get => _leftVentricleWallDiam; set => _leftVentricleWallDiam = SetFieldValueAndNotify(value); }

		private double? _leftVentricleDiam;
		public double? LeftVentricleDiam { get => _leftVentricleDiam; set => _leftVentricleDiam = SetFieldValueAndNotify(value); }

		public int Result { get => this.GetScoreResult(); }

		public string Interpretation
		{
			get
			{
				if (this.Result <= 4) return this.ScoreResources["PhhfGroup_Interpretation_Precapillary"];
				if (this.Result >= 7) return this.ScoreResources["PhhfGroup_Interpretation_Postcapillary"];
				return this.ScoreResources["PhhfGroup_Interpretation_Intermediate"];
			}
		}
		
		// Calculate PhhfGroupScore and returns the result or exception if catched
		public int GetScoreResult()
		{
			try
			{
				return this.CalculatePhhfGroupScore();
			}
			catch (PropertyNullException exception)
			{
				throw exception;
			}
			catch (DivideByZeroException)
			{
				throw new DivideByZeroException(this.Resources["DivisionByZeroError"]);
			}
			catch (Exception exception)
			{
				throw exception;
			}
		}

		// Calculate bodysurface area if height and weight have been set
		private double CalculateBodySurface()
		{
			List<string> nullPropertiesList = this.GetNullPropertiesList(nameof(Height), nameof(Weight));

			if (nullPropertiesList.Count > 0)
				throw new PropertyNullException(nullPropertiesList,
												this.Resources["PleaseFillOneField"],
												this.Resources["PleaseFillMultipleFields"]);

			return new BodySurfaceArea(this.Height.Value, this.Weight.Value).Result;
		}

		// Calculate left ventricle mass if septum, lv diameter and posterior wall have been set
		private double CalculateLeftVentricleMass()
		{
			List<string> nullPropertiesList = this.GetNullPropertiesList(nameof(LeftVentricleSeptumDiam), nameof(LeftVentricleDiam), nameof(LeftVentricleWallDiam));

			if (nullPropertiesList.Count > 0)
				throw new PropertyNullException(nullPropertiesList,
												this.Resources["PleaseFillOneField"],
												this.Resources["PleaseFillMultipleFields"]);

			return new LeftVentricleMass(this.LeftVentricleSeptumDiam.Value, this.LeftVentricleDiam.Value, this.LeftVentricleWallDiam.Value).Result;
		}

		// Calculate and return PH-HFpEF Group score if all needed properties have been set
		private int CalculatePhhfGroupScore()
		{
			// Are there missing properties ? If yes, throw an exception
			List<string> nullPropertiesList = this.GetNullPropertiesList(nameof(this.LeftAtriumArea), nameof(this.RightVentricleArea));

			if (this.LVMassIsKnown)
				nullPropertiesList.AddRange(this.GetNullPropertiesList(nameof(this.LeftVentricleMass)));

			else
				nullPropertiesList.AddRange(this.GetNullPropertiesList(nameof(LeftVentricleSeptumDiam), nameof(LeftVentricleDiam), nameof(LeftVentricleWallDiam), nameof(Height), nameof(Weight)));

			if (nullPropertiesList.Count > 0)
				throw new PropertyNullException(nullPropertiesList,
												this.Resources["PleaseFillOneField"],
												this.Resources["PleaseFillMultipleFields"]);

			// If all properties have been set, calculate the score
			double LVMass = this.LVMassIsKnown ? this.LeftVentricleMass.Value : this.CalculateLeftVentricleMass() / this.CalculateBodySurface();

			return new PhhfGroupScore(this.Diabetes, this.AtrialFibrillation, this.RightVentricleArea.Value,
				this.LeftAtriumArea.Value, LVMass).Result;
		}
	}
}
