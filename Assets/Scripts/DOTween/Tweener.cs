using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
namespace DG.Tweening
{
	public abstract class Tweener : Tween
	{
		public bool hasManuallySetStartValue;
		public bool isFromAllowed = true;
		public Tweener()
		{
		}
		public abstract Tweener ChangeStartValue(object newStartValue, float newDuration = -1f);
		public abstract Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false);
		public abstract Tweener ChangeEndValue(object newEndValue, bool snapStartValue);
		public abstract Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f);
		public abstract Tweener SetFrom(bool relative);
		public static bool Setup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null) where TPlugOptions : struct
		{
			if (plugin != null)
			{
				t.tweenPlugin = plugin;
			}
			else
			{
				if (t.tweenPlugin == null)
				{
					t.tweenPlugin = PluginsManager.GetDefaultPlugin<T1, T2, TPlugOptions>();
				}
				if (t.tweenPlugin == null)
				{
					Debugger.LogError("No suitable plugin found for this type");
					return false;
				}
			}
			t.getter = getter;
			t.setter = setter;
			t.endValue = endValue;
			t.duration = duration;
			t.autoKill = DOTween.defaultAutoKill;
			t.isRecyclable = DOTween.defaultRecyclable;
			t.easeType = DOTween.defaultEaseType;
			t.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
			t.easePeriod = DOTween.defaultEasePeriod;
			t.loopType = DOTween.defaultLoopType;
			t.isPlaying = (DOTween.defaultAutoPlay == AutoPlay.All || DOTween.defaultAutoPlay == AutoPlay.AutoPlayTweeners);
			return true;
		}
		public static float DoUpdateDelay<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, float elapsed) where TPlugOptions : struct
		{
			float delay = t.delay;
			if (elapsed > delay)
			{
				t.elapsedDelay = delay;
				t.delayComplete = true;
				return elapsed - delay;
			}
			t.elapsedDelay = elapsed;
			return 0f;
		}
		public static bool DoStartup<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct
		{
			t.startupDone = true;
			if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
			{
				return false;
			}
			if (!t.hasManuallySetStartValue)
			{
				if (DOTween.useSafeMode)
				{
					try
					{
						t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
						goto IL_69;
					}
					catch
					{
						return false;
					}
				}
				t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
			}
			IL_69:
			if (t.isRelative)
			{
				t.tweenPlugin.SetRelativeEndValue(t);
			}
			t.tweenPlugin.SetChangeValue(t);
			Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
			if (t.duration <= 0f)
			{
				t.easeType = Ease.INTERNAL_Zero;
			}
			return true;
		}
		public static Tweener DoChangeStartValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, float newDuration) where TPlugOptions : struct
		{
			t.hasManuallySetStartValue = true;
			t.startValue = newStartValue;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}
		public static Tweener DoChangeEndValue<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newEndValue, float newDuration, bool snapStartValue) where TPlugOptions : struct
		{
			t.endValue = newEndValue;
			t.isRelative = false;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				if (snapStartValue)
				{
					if (DOTween.useSafeMode)
					{
						try
						{
							t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
							goto IL_7A;
						}
						catch
						{
							TweenManager.Despawn(t, true);
							return null;
						}
					}
					t.startValue = t.tweenPlugin.ConvertToStartValue(t, t.getter());
				}
				IL_7A:
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}
		public static Tweener DoChangeValues<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t, T2 newStartValue, T2 newEndValue, float newDuration) where TPlugOptions : struct
		{
			t.hasManuallySetStartValue = true;
			t.isRelative = (t.isFrom = false);
			t.startValue = newStartValue;
			t.endValue = newEndValue;
			if (t.startupDone)
			{
				if (t.specialStartupMode != SpecialStartupMode.None && !Tweener.DOStartupSpecials<T1, T2, TPlugOptions>(t))
				{
					return null;
				}
				t.tweenPlugin.SetChangeValue(t);
			}
			if (newDuration > 0f)
			{
				t.duration = newDuration;
				if (t.startupDone)
				{
					Tweener.DOStartupDurationBased<T1, T2, TPlugOptions>(t);
				}
			}
			Tween.DoGoto(t, 0f, 0, UpdateMode.IgnoreOnUpdate);
			return t;
		}
		private static bool DOStartupSpecials<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct
		{
			bool result;
			try
			{
				switch (t.specialStartupMode)
				{
				case SpecialStartupMode.SetLookAt:
					if (!SpecialPluginsUtils.SetLookAt(t as TweenerCore<Quaternion, Vector3, QuaternionOptions>))
					{
						result = false;
						return result;
					}
					break;
				case SpecialStartupMode.SetShake:
					if (!SpecialPluginsUtils.SetShake(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						result = false;
						return result;
					}
					break;
				case SpecialStartupMode.SetPunch:
					if (!SpecialPluginsUtils.SetPunch(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						result = false;
						return result;
					}
					break;
				case SpecialStartupMode.SetCameraShakePosition:
					if (!SpecialPluginsUtils.SetCameraShakePosition(t as TweenerCore<Vector3, Vector3[], Vector3ArrayOptions>))
					{
						result = false;
						return result;
					}
					break;
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		private static void DOStartupDurationBased<T1, T2, TPlugOptions>(TweenerCore<T1, T2, TPlugOptions> t) where TPlugOptions : struct
		{
			if (t.isSpeedBased)
			{
				t.duration = t.tweenPlugin.GetSpeedBasedDuration(t.plugOptions, t.duration, t.changeValue);
			}
			t.fullDuration = ((t.loops > -1) ? (t.duration * (float)t.loops) : float.PositiveInfinity);
		}
	}
}
