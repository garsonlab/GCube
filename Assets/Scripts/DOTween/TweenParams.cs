using DG.Tweening.Core.Easing;
using System;
using UnityEngine;
namespace DG.Tweening
{
	public class TweenParams
	{
		public static readonly TweenParams Params = new TweenParams();
		public object id;
		public object target;
		public UpdateType updateType;
		public bool isIndependentUpdate;
		public TweenCallback onStart;
		public TweenCallback onPlay;
		public TweenCallback onRewind;
		public TweenCallback onUpdate;
		public TweenCallback onStepComplete;
		public TweenCallback onComplete;
		public TweenCallback onKill;
		public TweenCallback<int> onWaypointChange;
		public bool isRecyclable;
		public bool isSpeedBased;
		public bool autoKill;
		public int loops;
		public LoopType loopType;
		public float delay;
		public bool isRelative;
		public Ease easeType;
		public EaseFunction customEase;
		public float easeOvershootOrAmplitude;
		public float easePeriod;
		public TweenParams()
		{
			this.Clear();
		}
		public TweenParams Clear()
		{
			this.id = (this.target = null);
			this.updateType = DOTween.defaultUpdateType;
			this.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
			this.onStart = (this.onPlay = (this.onRewind = (this.onUpdate = (this.onStepComplete = (this.onComplete = (this.onKill = null))))));
			this.onWaypointChange = null;
			this.isRecyclable = DOTween.defaultRecyclable;
			this.isSpeedBased = false;
			this.autoKill = DOTween.defaultAutoKill;
			this.loops = 1;
			this.loopType = DOTween.defaultLoopType;
			this.delay = 0f;
			this.isRelative = false;
			this.easeType = Ease.Unset;
			this.customEase = null;
			this.easeOvershootOrAmplitude = DOTween.defaultEaseOvershootOrAmplitude;
			this.easePeriod = DOTween.defaultEasePeriod;
			return this;
		}
		public TweenParams SetAutoKill(bool autoKillOnCompletion = true)
		{
			this.autoKill = autoKillOnCompletion;
			return this;
		}
		public TweenParams SetId(object id)
		{
			this.id = id;
			return this;
		}
		public TweenParams SetTarget(object target)
		{
			this.target = target;
			return this;
		}
		public TweenParams SetLoops(int loops, LoopType? loopType = null)
		{
			if (loops < -1)
			{
				loops = -1;
			}
			else if (loops == 0)
			{
				loops = 1;
			}
			this.loops = loops;
			if (loopType.HasValue)
			{
				this.loopType = loopType.Value;
			}
			return this;
		}
		public TweenParams SetEase(Ease ease, float? overshootOrAmplitude = null, float? period = null)
		{
			this.easeType = ease;
			this.easeOvershootOrAmplitude = (overshootOrAmplitude.HasValue ? overshootOrAmplitude.Value : DOTween.defaultEaseOvershootOrAmplitude);
			this.easePeriod = (period.HasValue ? period.Value : DOTween.defaultEasePeriod);
			this.customEase = null;
			return this;
		}
		public TweenParams SetEase(AnimationCurve animCurve)
		{
			this.easeType = Ease.INTERNAL_Custom;
			this.customEase = new EaseFunction(new EaseCurve(animCurve).Evaluate);
			return this;
		}
		public TweenParams SetEase(EaseFunction customEase)
		{
			this.easeType = Ease.INTERNAL_Custom;
			this.customEase = customEase;
			return this;
		}
		public TweenParams SetRecyclable(bool recyclable = true)
		{
			this.isRecyclable = recyclable;
			return this;
		}
		public TweenParams SetUpdate(bool isIndependentUpdate)
		{
			this.updateType = DOTween.defaultUpdateType;
			this.isIndependentUpdate = isIndependentUpdate;
			return this;
		}
		public TweenParams SetUpdate(UpdateType updateType, bool isIndependentUpdate = false)
		{
			this.updateType = updateType;
			this.isIndependentUpdate = isIndependentUpdate;
			return this;
		}
		public TweenParams OnStart(TweenCallback action)
		{
			this.onStart = action;
			return this;
		}
		public TweenParams OnPlay(TweenCallback action)
		{
			this.onPlay = action;
			return this;
		}
		public TweenParams OnRewind(TweenCallback action)
		{
			this.onRewind = action;
			return this;
		}
		public TweenParams OnUpdate(TweenCallback action)
		{
			this.onUpdate = action;
			return this;
		}
		public TweenParams OnStepComplete(TweenCallback action)
		{
			this.onStepComplete = action;
			return this;
		}
		public TweenParams OnComplete(TweenCallback action)
		{
			this.onComplete = action;
			return this;
		}
		public TweenParams OnKill(TweenCallback action)
		{
			this.onKill = action;
			return this;
		}
		public TweenParams OnWaypointChange(TweenCallback<int> action)
		{
			this.onWaypointChange = action;
			return this;
		}
		public TweenParams SetDelay(float delay)
		{
			this.delay = delay;
			return this;
		}
		public TweenParams SetRelative(bool isRelative = true)
		{
			this.isRelative = isRelative;
			return this;
		}
		public TweenParams SetSpeedBased(bool isSpeedBased = true)
		{
			this.isSpeedBased = isSpeedBased;
			return this;
		}
	}
}
