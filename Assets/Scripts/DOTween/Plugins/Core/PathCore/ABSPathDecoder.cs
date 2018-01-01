using System;
using UnityEngine;
namespace DG.Tweening.Plugins.Core.PathCore
{
	public abstract class ABSPathDecoder
	{
		public abstract void FinalizePath(Path p, Vector3[] wps, bool isClosedPath);
		public abstract Vector3 GetPoint(float perc, Vector3[] wps, Path p, ControlPoint[] controlPoints);
	}
}
