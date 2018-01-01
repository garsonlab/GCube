using System;
using UnityEngine;
namespace DG.Tweening.Core
{
	public static class Debugger
	{
		public static int logPriority;
		public static void Log(object message)
		{
			Debug.Log("DOTWEEN :: " + message);
		}
		public static void LogWarning(object message)
		{
			Debug.LogWarning("DOTWEEN :: " + message);
		}
		public static void LogError(object message)
		{
			Debug.LogError("DOTWEEN :: " + message);
		}
		public static void LogReport(object message)
		{
			Debug.Log("<color=#00B500FF>DOTWEEN :: " + message + "</color>");
		}
		public static void LogInvalidTween(Tween t)
		{
			Debugger.LogWarning("This Tween has been killed and is now invalid");
		}
		public static void LogNestedTween(Tween t)
		{
			Debugger.LogWarning("This Tween was added to a Sequence and can't be controlled directly");
		}
		public static void LogNullTween(Tween t)
		{
			Debugger.LogWarning("Null Tween");
		}
		public static void LogNonPathTween(Tween t)
		{
			Debugger.LogWarning("This Tween is not a path tween");
		}
		public static void LogMissingMaterialProperty(string propertyName)
		{
			Debugger.LogWarning(string.Format("This material doesn't have a {0} property", propertyName));
		}
		public static void SetLogPriority(LogBehaviour logBehaviour)
		{
			switch (logBehaviour)
			{
			case LogBehaviour.Default:
				Debugger.logPriority = 1;
				return;
			case LogBehaviour.Verbose:
				Debugger.logPriority = 2;
				return;
			default:
				Debugger.logPriority = 0;
				return;
			}
		}
	}
}
