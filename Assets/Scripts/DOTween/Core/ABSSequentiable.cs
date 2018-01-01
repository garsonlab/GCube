using System;
namespace DG.Tweening.Core
{
	public abstract class ABSSequentiable
	{
		public TweenType tweenType;
		public float sequencedPosition;
		public float sequencedEndPosition;
		public TweenCallback onStart;
	}
}
