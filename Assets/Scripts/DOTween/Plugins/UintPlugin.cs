using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
namespace DG.Tweening.Plugins
{
	public class UintPlugin : ABSTweenPlugin<uint, uint, NoOptions>
	{
		public override void Reset(TweenerCore<uint, uint, NoOptions> t)
		{
		}
		public override void SetFrom(TweenerCore<uint, uint, NoOptions> t, bool isRelative)
		{
			uint endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			t.setter(t.startValue);
		}
		public override uint ConvertToStartValue(TweenerCore<uint, uint, NoOptions> t, uint value)
		{
			return value;
		}
		public override void SetRelativeEndValue(TweenerCore<uint, uint, NoOptions> t)
		{
			t.endValue += t.startValue;
		}
		public override void SetChangeValue(TweenerCore<uint, uint, NoOptions> t)
		{
			t.changeValue = t.endValue - t.startValue;
		}
		public override float GetSpeedBasedDuration(NoOptions options, float unitsXSecond, uint changeValue)
		{
			float num = changeValue / unitsXSecond;
			if (num < 0f)
			{
				num = -num;
			}
			return num;
		}
		public override void EvaluateAndApply(NoOptions options, Tween t, bool isRelative, DOGetter<uint> getter, DOSetter<uint> setter, float elapsed, uint startValue, uint changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += (uint)((ulong)changeValue * (ulong)((long)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops)));
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += (uint)((ulong)changeValue * (ulong)((long)((t.loopType == LoopType.Incremental) ? t.loops : 1)) * (ulong)((long)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops)));
			}
			setter((uint)Math.Round((double)(startValue + changeValue * EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod))));
		}
	}
}
