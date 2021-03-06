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
	public class adlistener : AdListener
	{
		// Declare the delegate (if using non-generic pattern). 
		public delegate void AdLoadedEvent();
		public delegate void AdClosedEvent();
		public delegate void AdOpenedEvent();



		// Declare the event. 
		public event AdLoadedEvent AdLoaded;
		public event AdClosedEvent AdClosed;
		public event AdOpenedEvent AdOpened;

		public override void OnAdLoaded()
		{
			if (AdLoaded != null) this.AdLoaded();
			base.OnAdLoaded();
		}

		public override void OnAdClosed()
		{
			if (AdClosed != null) this.AdClosed();
			base.OnAdClosed();
		}
		public override void OnAdOpened()
		{
			if (AdOpened != null) this.AdOpened();
			base.OnAdOpened();
		}
	}
}

