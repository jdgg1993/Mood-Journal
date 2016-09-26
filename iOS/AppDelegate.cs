using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace Moodify.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App());

			UINavigationBar.Appearance.BarTintColor = UIColor.FromRGB(33, 150, 243);

			return base.FinishedLaunching(app, options);
		}
	}
}
