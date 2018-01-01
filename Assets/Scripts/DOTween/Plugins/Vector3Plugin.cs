using DG.Tweening.Core;
using DG.Tweening.Core.Easing;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using UnityEngine;
namespace DG.Tweening.Plugins
{
	public class Vector3Plugin : ABSTweenPlugin<Vector3, Vector3, VectorOptions>
	{
		public override void Reset(TweenerCore<Vector3, Vector3, VectorOptions> t)
		{
		}
		public override void SetFrom(TweenerCore<Vector3, Vector3, VectorOptions> t, bool isRelative)
		{
			Vector3 endValue = t.endValue;
			t.endValue = t.getter();
			t.startValue = (isRelative ? (t.endValue + endValue) : endValue);
			Vector3 pNewValue = t.endValue;
			AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
			switch (axisConstraint)
			{
			case AxisConstraint.X:
				pNewValue.x = t.startValue.x;
				goto IL_A0;
			case (AxisConstraint)3:
				break;
			case AxisConstraint.Y:
				pNewValue.y = t.startValue.y;
				goto IL_A0;
			default:
				if (axisConstraint == AxisConstraint.Z)
				{
					pNewValue.z = t.startValue.z;
					goto IL_A0;
				}
				break;
			}
			pNewValue = t.startValue;
			IL_A0:
			if (t.plugOptions.snapping)
			{
				pNewValue.x = (float)Math.Round((double)pNewValue.x);
				pNewValue.y = (float)Math.Round((double)pNewValue.y);
				pNewValue.z = (float)Math.Round((double)pNewValue.z);
			}
			t.setter(pNewValue);
		}
		public override Vector3 ConvertToStartValue(TweenerCore<Vector3, Vector3, VectorOptions> t, Vector3 value)
		{
			return value;
		}
		public override void SetRelativeEndValue(TweenerCore<Vector3, Vector3, VectorOptions> t)
		{
			t.endValue += t.startValue;
		}
		public override void SetChangeValue(TweenerCore<Vector3, Vector3, VectorOptions> t)
		{
			AxisConstraint axisConstraint = t.plugOptions.axisConstraint;
			switch (axisConstraint)
			{
			case AxisConstraint.X:
				t.changeValue = new Vector3(t.endValue.x - t.startValue.x, 0f, 0f);
				return;
			case (AxisConstraint)3:
				break;
			case AxisConstraint.Y:
				t.changeValue = new Vector3(0f, t.endValue.y - t.startValue.y, 0f);
				return;
			default:
				if (axisConstraint == AxisConstraint.Z)
				{
					t.changeValue = new Vector3(0f, 0f, t.endValue.z - t.startValue.z);
					return;
				}
				break;
			}
			t.changeValue = t.endValue - t.startValue;
		}
		public override float GetSpeedBasedDuration(VectorOptions options, float unitsXSecond, Vector3 changeValue)
		{
			return changeValue.magnitude / unitsXSecond;
		}
		public override void EvaluateAndApply(VectorOptions options, Tween t, bool isRelative, DOGetter<Vector3> getter, DOSetter<Vector3> setter, float elapsed, Vector3 startValue, Vector3 changeValue, float duration, bool usingInversePosition, UpdateNotice updateNotice)
		{
			if (t.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)(t.isComplete ? (t.completedLoops - 1) : t.completedLoops);
			}
			if (t.isSequenced && t.sequenceParent.loopType == LoopType.Incremental)
			{
				startValue += changeValue * (float)((t.loopType == LoopType.Incremental) ? t.loops : 1) * (float)(t.sequenceParent.isComplete ? (t.sequenceParent.completedLoops - 1) : t.sequenceParent.completedLoops);
			}
			float num = EaseManager.Evaluate(t.easeType, t.customEase, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
			AxisConstraint axisConstraint = options.axisConstraint;
			switch (axisConstraint)
			{
			case AxisConstraint.X:
			{
				Vector3 pNewValue = getter();
				pNewValue.x = startValue.x + changeValue.x * num;
				if (options.snapping)
				{
					pNewValue.x = (float)Math.Round((double)pNewValue.x);
				}
				setter(pNewValue);
				return;
			}
			case (AxisConstraint)3:
				break;
			case AxisConstraint.Y:
			{
				Vector3 pNewValue2 = getter();
				pNewValue2.y = startValue.y + changeValue.y * num;
				if (options.snapping)
				{
					pNewValue2.y = (float)Math.Round((double)pNewValue2.y);
				}
				setter(pNewValue2);
				return;
			}
			default:
				if (axisConstraint == AxisConstraint.Z)
				{
					Vector3 pNewValue3 = getter();
					pNewValue3.z = startValue.z + changeValue.z * num;
					if (options.snapping)
					{
						pNewValue3.z = (float)Math.Round((double)pNewValue3.z);
					}
					setter(pNewValue3);
					return;
				}
				break;
			}
			startValue.x += changeValue.x * num;
			startValue.y += changeValue.y * num;
			startValue.z += changeValue.z * num;
			if (options.snapping)
			{
				startValue.x = (float)Math.Round((double)startValue.x);
				startValue.y = (float)Math.Round((double)startValue.y);
				startValue.z = (float)Math.Round((double)startValue.z);
			}
			setter(startValue);
		}
	}
}
