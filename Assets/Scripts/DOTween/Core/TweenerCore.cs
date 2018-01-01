using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using System;
namespace DG.Tweening.Core
{
	public class TweenerCore<T1, T2, TPlugOptions> : Tweener where TPlugOptions : struct
	{
		private const string _TxtCantChangeSequencedValues = "You cannot change the values of a tween contained inside a Sequence";
		public T2 startValue;
		public T2 endValue;
		public T2 changeValue;
		public TPlugOptions plugOptions;
		public DOGetter<T1> getter;
		public DOSetter<T1> setter;
		public ABSTweenPlugin<T1, T2, TPlugOptions> tweenPlugin;
		public TweenerCore()
		{
			this.typeofT1 = typeof(T1);
			this.typeofT2 = typeof(T2);
			this.typeofTPlugOptions = typeof(TPlugOptions);
			this.tweenType = TweenType.Tweener;
			this.Reset();
		}
		public override Tweener ChangeStartValue(object newStartValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("You cannot change the values of a tween contained inside a Sequence");
				}
				return this;
			}
			Type type = newStartValue.GetType();
			if (type != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeStartValue: incorrect newStartValue type (is ",
						type,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			return Tweener.DoChangeStartValue<T1, T2, TPlugOptions>(this, (T2)((object)newStartValue), newDuration);
		}
		public override Tweener ChangeEndValue(object newEndValue, bool snapStartValue)
		{
			return this.ChangeEndValue(newEndValue, -1f, snapStartValue);
		}
		public override Tweener ChangeEndValue(object newEndValue, float newDuration = -1f, bool snapStartValue = false)
		{
			if (this.isSequenced)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("You cannot change the values of a tween contained inside a Sequence");
				}
				return this;
			}
			Type type = newEndValue.GetType();
			if (type != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeEndValue: incorrect newEndValue type (is ",
						type,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			return Tweener.DoChangeEndValue<T1, T2, TPlugOptions>(this, (T2)((object)newEndValue), newDuration, snapStartValue);
		}
		public override Tweener ChangeValues(object newStartValue, object newEndValue, float newDuration = -1f)
		{
			if (this.isSequenced)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("You cannot change the values of a tween contained inside a Sequence");
				}
				return this;
			}
			Type type = newStartValue.GetType();
			Type type2 = newEndValue.GetType();
			if (type != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeValues: incorrect value type (is ",
						type,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			if (type2 != this.typeofT2)
			{
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning(string.Concat(new object[]
					{
						"ChangeValues: incorrect value type (is ",
						type2,
						", should be ",
						this.typeofT2,
						")"
					}));
				}
				return this;
			}
			return Tweener.DoChangeValues<T1, T2, TPlugOptions>(this, (T2)((object)newStartValue), (T2)((object)newEndValue), newDuration);
		}
		public override Tweener SetFrom(bool relative)
		{
			this.tweenPlugin.SetFrom(this, relative);
			this.hasManuallySetStartValue = true;
			return this;
		}
		public sealed override void Reset()
		{
			base.Reset();
			if (this.tweenPlugin != null)
			{
				this.tweenPlugin.Reset(this);
			}
			this.plugOptions = default(TPlugOptions);
			this.getter = null;
			this.setter = null;
			this.hasManuallySetStartValue = false;
			this.isFromAllowed = true;
		}
		public override bool Validate()
		{
			try
			{
				this.getter();
			}
			catch
			{
				return false;
			}
			return true;
		}
		public override float UpdateDelay(float elapsed)
		{
			return Tweener.DoUpdateDelay<T1, T2, TPlugOptions>(this, elapsed);
		}
		public override bool Startup()
		{
			return Tweener.DoStartup<T1, T2, TPlugOptions>(this);
		}
		public override bool ApplyTween(float prevPosition, int prevCompletedLoops, int newCompletedSteps, bool useInversePosition, UpdateMode updateMode, UpdateNotice updateNotice)
		{
			float elapsed = useInversePosition ? (this.duration - this.position) : this.position;
			if (DOTween.useSafeMode)
			{
				try
				{
					this.tweenPlugin.EvaluateAndApply(this.plugOptions, this, this.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
					return false;
				}
				catch
				{
					return true;
				}
			}
			this.tweenPlugin.EvaluateAndApply(this.plugOptions, this, this.isRelative, this.getter, this.setter, elapsed, this.startValue, this.changeValue, this.duration, useInversePosition, updateNotice);
			return false;
		}
	}
}
