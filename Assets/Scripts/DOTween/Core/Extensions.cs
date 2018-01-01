using DG.Tweening.Core.Enums;
using System;
namespace DG.Tweening.Core
{
	public static class Extensions
	{
		public static T SetSpecialStartupMode<T>(this T t, SpecialStartupMode mode) where T : Tween
		{
			t.specialStartupMode = mode;
			return t;
		}
		public static TweenerCore<T1, T2, TPlugOptions> NoFrom<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct
		{
			t.isFromAllowed = false;
			return t;
		}
		public static TweenerCore<T1, T2, TPlugOptions> Blendable<T1, T2, TPlugOptions>(this TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct
		{
			t.isBlendable = true;
			return t;
		}
	}
}
