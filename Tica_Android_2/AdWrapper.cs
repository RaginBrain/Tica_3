﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;

namespace Tica_Android_2
{
	public static class AdWrapper
	{
		public static InterstitialAd ConstructFullPageAdd(Context con, string UnitID)
		{
			var ad = new InterstitialAd(con);
			ad.AdUnitId = UnitID;
			return ad;
		}

		public static AdView ConstructStandardBanner(Context con, AdSize adsize, string UnitID)
		{
			var ad = new AdView(con);
			ad.AdSize = adsize;
			ad.AdUnitId = UnitID;
			return ad;
		}

		public static InterstitialAd CustomBuild(this InterstitialAd ad)
		{
			var requestbuilder = new AdRequest.Builder();
			ad.LoadAd(requestbuilder.Build());
			return ad;
		}

		public static AdView CustomBuild(this AdView ad)
		{
			var requestbuilder = new AdRequest.Builder();
			ad.LoadAd(requestbuilder.Build());
			return ad;
		}

	}
}

