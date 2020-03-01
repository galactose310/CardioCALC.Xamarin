using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using CardioCALC.UI.Mobile;

[assembly: ExportRenderer(typeof(CustomSwitch), typeof(CardioCALC.Droid.CustomSwitchRenderer))]
namespace CardioCALC.Droid
{
	public class CustomSwitchRenderer : SwitchRenderer
	{
		private CustomSwitch view;

		public CustomSwitchRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Switch> e)
		{
			base.OnElementChanged(e);
			if (e.OldElement != null || e.NewElement == null)
				return;
			view = (CustomSwitch)Element;
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBean)
			{
				this.Control.SwitchMinWidth = System.Convert.ToInt32(view.MinimumWidthRequest);

				if (this.Control != null)
				{
					if (this.Control.Checked)
					{
						this.SetColorToOnSwitch();
					}
					else
					{
						this.SetColorToOffSwitch();
					}
				}
			}

			this.Control.CheckedChange += (sender, e2) => {
				((IElementController)base.Element).SetValueFromRenderer(Xamarin.Forms.Switch.IsToggledProperty, Control.Checked);
				if (this.Control.Checked)
					this.SetColorToOnSwitch();
				else
					this.SetColorToOffSwitch();
			};
		}


		protected void SetColorToOnSwitch()
		{
			this.Control.ThumbDrawable.SetTint(view.SwitchOnThumbColor.ToAndroid());
			this.Control.TrackDrawable.SetTint(view.SwitchOnColor.ToAndroid());
		}

		protected void SetColorToOffSwitch()
		{
			this.Control.ThumbDrawable.SetTint(view.SwitchOffThumbColor.ToAndroid());
			this.Control.TrackDrawable.SetTint(view.SwitchOffColor.ToAndroid());
		}
	}
}