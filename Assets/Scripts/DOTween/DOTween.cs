using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace DG.Tweening
{
	public class DOTween
	{
		public static readonly string Version;
		public static bool useSafeMode;
		public static bool showUnityEditorReport;
		public static float timeScale;
		public static bool useSmoothDeltaTime;
		private static LogBehaviour _logBehaviour;
		public static bool drawGizmos;
		public static UpdateType defaultUpdateType;
		public static bool defaultTimeScaleIndependent;
		public static AutoPlay defaultAutoPlay;
		public static bool defaultAutoKill;
		public static LoopType defaultLoopType;
		public static bool defaultRecyclable;
		public static Ease defaultEaseType;
		public static float defaultEaseOvershootOrAmplitude;
		public static float defaultEasePeriod;
		public static DOTweenComponent instance;
		public static bool isUnityEditor;
		public static bool isDebugBuild;
		public static int maxActiveTweenersReached;
		public static int maxActiveSequencesReached;
		public static readonly List<TweenCallback> GizmosDelegates;
		public static bool initialized;
		public static bool isQuitting;
		public static LogBehaviour logBehaviour
		{
			get
			{
				return DOTween._logBehaviour;
			}
			set
			{
				DOTween._logBehaviour = value;
				Debugger.SetLogPriority(DOTween._logBehaviour);
			}
		}
		static DOTween()
		{
			DOTween.Version = "1.0.770";
			DOTween.useSafeMode = true;
			DOTween.showUnityEditorReport = false;
			DOTween.timeScale = 1f;
			DOTween._logBehaviour = LogBehaviour.ErrorsOnly;
			DOTween.drawGizmos = true;
			DOTween.defaultUpdateType = UpdateType.Normal;
			DOTween.defaultTimeScaleIndependent = false;
			DOTween.defaultAutoPlay = AutoPlay.All;
			DOTween.defaultAutoKill = true;
			DOTween.defaultLoopType = LoopType.Restart;
			DOTween.defaultEaseType = Ease.OutQuad;
			DOTween.defaultEaseOvershootOrAmplitude = 1.70158f;
			DOTween.defaultEasePeriod = 0f;
			DOTween.GizmosDelegates = new List<TweenCallback>();
			DOTween.isUnityEditor = Application.isEditor;
		}
		public static IDOTweenInit Init(bool? recycleAllByDefault = null, bool? useSafeMode = null, LogBehaviour? logBehaviour = null)
		{
			if (DOTween.initialized)
			{
				return DOTween.instance;
			}
			if (!Application.isPlaying || DOTween.isQuitting)
			{
				return null;
			}
			DOTweenSettings settings = Resources.Load(DOTweenSettings.AssetName) as DOTweenSettings;
			return DOTween.Init(settings, recycleAllByDefault, useSafeMode, logBehaviour);
		}
		private static void AutoInit()
		{
			DOTweenSettings settings = Resources.Load(DOTweenSettings.AssetName) as DOTweenSettings;
			DOTween.Init(settings, null, null, null);
		}
		private static IDOTweenInit Init(DOTweenSettings settings, bool? recycleAllByDefault, bool? useSafeMode, LogBehaviour? logBehaviour)
		{
			DOTween.initialized = true;
			if (recycleAllByDefault.HasValue)
			{
				DOTween.defaultRecyclable = recycleAllByDefault.Value;
			}
			if (useSafeMode.HasValue)
			{
				DOTween.useSafeMode = useSafeMode.Value;
			}
			if (logBehaviour.HasValue)
			{
				DOTween.logBehaviour = logBehaviour.Value;
			}
			DOTweenComponent.Create();
			if (settings != null)
			{
				if (!useSafeMode.HasValue)
				{
					DOTween.useSafeMode = settings.useSafeMode;
				}
				if (!logBehaviour.HasValue)
				{
					DOTween.logBehaviour = settings.logBehaviour;
				}
				if (!recycleAllByDefault.HasValue)
				{
					DOTween.defaultRecyclable = settings.defaultRecyclable;
				}
				DOTween.timeScale = settings.timeScale;
				DOTween.useSmoothDeltaTime = settings.useSmoothDeltaTime;
				DOTween.defaultRecyclable = ((!recycleAllByDefault.HasValue) ? settings.defaultRecyclable : recycleAllByDefault.Value);
				DOTween.showUnityEditorReport = settings.showUnityEditorReport;
				DOTween.drawGizmos = settings.drawGizmos;
				DOTween.defaultAutoPlay = settings.defaultAutoPlay;
				DOTween.defaultUpdateType = settings.defaultUpdateType;
				DOTween.defaultTimeScaleIndependent = settings.defaultTimeScaleIndependent;
				DOTween.defaultEaseType = settings.defaultEaseType;
				DOTween.defaultEaseOvershootOrAmplitude = settings.defaultEaseOvershootOrAmplitude;
				DOTween.defaultEasePeriod = settings.defaultEasePeriod;
				DOTween.defaultAutoKill = settings.defaultAutoKill;
				DOTween.defaultLoopType = settings.defaultLoopType;
			}
			if (Debugger.logPriority >= 2)
			{
				Debugger.Log(string.Concat(new object[]
				{
					"DOTween initialization (useSafeMode: ",
					DOTween.useSafeMode,
					", recycling: ",
					DOTween.defaultRecyclable ? "ON" : "OFF",
					", logBehaviour: ",
					DOTween.logBehaviour,
					")"
				}));
			}
			return DOTween.instance;
		}
		public static void SetTweensCapacity(int tweenersCapacity, int sequencesCapacity)
		{
			TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
		}
		public static void Clear(bool destroy = false)
		{
			TweenManager.PurgeAll();
			PluginsManager.PurgeAll();
			if (!destroy)
			{
				return;
			}
			DOTween.initialized = false;
			DOTween.useSafeMode = false;
			DOTween.showUnityEditorReport = false;
			DOTween.drawGizmos = true;
			DOTween.timeScale = 1f;
			DOTween.useSmoothDeltaTime = false;
			DOTween.logBehaviour = LogBehaviour.ErrorsOnly;
			DOTween.defaultEaseType = Ease.OutQuad;
			DOTween.defaultEaseOvershootOrAmplitude = 1.70158f;
			DOTween.defaultEasePeriod = 0f;
			DOTween.defaultUpdateType = UpdateType.Normal;
			DOTween.defaultTimeScaleIndependent = false;
			DOTween.defaultAutoPlay = AutoPlay.All;
			DOTween.defaultLoopType = LoopType.Restart;
			DOTween.defaultAutoKill = true;
			DOTween.defaultRecyclable = false;
			DOTween.maxActiveTweenersReached = (DOTween.maxActiveSequencesReached = 0);
			DOTweenComponent.DestroyInstance();
		}
		public static void ClearCachedTweens()
		{
			TweenManager.PurgePools();
		}
		public static int Validate()
		{
			return TweenManager.Validate();
		}
		public static TweenerCore<float, float, FloatOptions> To(DOGetter<float> getter, DOSetter<float> setter, float endValue, float duration)
		{
			return DOTween.ApplyTo<float, float, FloatOptions>(getter, setter, endValue, duration, null);
		}
		public static Tweener To(DOGetter<int> getter, DOSetter<int> setter, int endValue, float duration)
		{
			return DOTween.ApplyTo<int, int, NoOptions>(getter, setter, endValue, duration, null);
		}
		public static Tweener To(DOGetter<uint> getter, DOSetter<uint> setter, uint endValue, float duration)
		{
			return DOTween.ApplyTo<uint, uint, NoOptions>(getter, setter, endValue, duration, null);
		}
		public static Tweener To(DOGetter<long> getter, DOSetter<long> setter, long endValue, float duration)
		{
			return DOTween.ApplyTo<long, long, NoOptions>(getter, setter, endValue, duration, null);
		}
		public static Tweener To(DOGetter<ulong> getter, DOSetter<ulong> setter, ulong endValue, float duration)
		{
			return DOTween.ApplyTo<ulong, ulong, NoOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<string, string, StringOptions> To(DOGetter<string> getter, DOSetter<string> setter, string endValue, float duration)
		{
			return DOTween.ApplyTo<string, string, StringOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<Vector2, Vector2, VectorOptions> To(DOGetter<Vector2> getter, DOSetter<Vector2> setter, Vector2 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector2, Vector2, VectorOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<Vector3, Vector3, VectorOptions> To(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<Vector4, Vector4, VectorOptions> To(DOGetter<Vector4> getter, DOSetter<Vector4> setter, Vector4 endValue, float duration)
		{
			return DOTween.ApplyTo<Vector4, Vector4, VectorOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> To(DOGetter<Quaternion> getter, DOSetter<Quaternion> setter, Vector3 endValue, float duration)
		{
			return DOTween.ApplyTo<Quaternion, Vector3, QuaternionOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<Color, Color, ColorOptions> To(DOGetter<Color> getter, DOSetter<Color> setter, Color endValue, float duration)
		{
			return DOTween.ApplyTo<Color, Color, ColorOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<Rect, Rect, RectOptions> To(DOGetter<Rect> getter, DOSetter<Rect> setter, Rect endValue, float duration)
		{
			return DOTween.ApplyTo<Rect, Rect, RectOptions>(getter, setter, endValue, duration, null);
		}
		public static Tweener To(DOGetter<RectOffset> getter, DOSetter<RectOffset> setter, RectOffset endValue, float duration)
		{
			return DOTween.ApplyTo<RectOffset, RectOffset, NoOptions>(getter, setter, endValue, duration, null);
		}
		public static TweenerCore<T1, T2, TPlugOptions> To<T1, T2, TPlugOptions>(ABSTweenPlugin<T1, T2, TPlugOptions> plugin, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration) where TPlugOptions : struct
		{
			return DOTween.ApplyTo<T1, T2, TPlugOptions>(getter, setter, endValue, duration, plugin);
		}
		public static TweenerCore<Vector3, Vector3, VectorOptions> ToAxis(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float endValue, float duration, AxisConstraint axisConstraint = AxisConstraint.X)
		{
			TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.ApplyTo<Vector3, Vector3, VectorOptions>(getter, setter, new Vector3(endValue, endValue, endValue), duration, null);
			tweenerCore.plugOptions.axisConstraint = axisConstraint;
			return tweenerCore;
		}
		public static Tweener ToAlpha(DOGetter<Color> getter, DOSetter<Color> setter, float endValue, float duration)
		{
			return DOTween.ApplyTo<Color, Color, ColorOptions>(getter, setter, new Color(0f, 0f, 0f, endValue), duration, null).SetOptions(true);
		}
		public static Tweener To(DOSetter<float> setter, float startValue, float endValue, float duration)
		{
			return DOTween.To(() => startValue, delegate(float x)
			{
				startValue = x;
				setter(startValue);
			}, endValue, duration).NoFrom<float, float, FloatOptions>();
		}
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Punch(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3 direction, float duration, int vibrato = 10, float elasticity = 1f)
		{
			if (elasticity > 1f)
			{
				elasticity = 1f;
			}
			else if (elasticity < 0f)
			{
				elasticity = 0f;
			}
			float num = direction.magnitude;
			int num2 = (int)((float)vibrato * duration);
			if (num2 < 2)
			{
				num2 = 2;
			}
			float num3 = num / (float)num2;
			float[] array = new float[num2];
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)(i + 1) / (float)num2;
				float num6 = duration * num5;
				num4 += num6;
				array[i] = num6;
			}
			float num7 = duration / num4;
			for (int j = 0; j < num2; j++)
			{
				array[j] *= num7;
			}
			Vector3[] array2 = new Vector3[num2];
			for (int k = 0; k < num2; k++)
			{
				if (k < num2 - 1)
				{
					if (k == 0)
					{
						array2[k] = direction;
					}
					else if (k % 2 != 0)
					{
						array2[k] = -Vector3.ClampMagnitude(direction, num * elasticity);
					}
					else
					{
						array2[k] = Vector3.ClampMagnitude(direction, num);
					}
					num -= num3;
				}
				else
				{
					array2[k] = Vector3.zero;
				}
			}
			return DOTween.ToArray(getter, setter, array2, array).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>().SetSpecialStartupMode(SpecialStartupMode.SetPunch);
		}
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, float strength = 3f, int vibrato = 10, float randomness = 90f, bool ignoreZAxis = true)
		{
			return DOTween.Shake(getter, setter, duration, new Vector3(strength, strength, strength), vibrato, randomness, ignoreZAxis, false);
		}
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, Vector3 strength, int vibrato = 10, float randomness = 90f)
		{
			return DOTween.Shake(getter, setter, duration, strength, vibrato, randomness, false, true);
		}
		private static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> Shake(DOGetter<Vector3> getter, DOSetter<Vector3> setter, float duration, Vector3 strength, int vibrato, float randomness, bool ignoreZAxis, bool vectorBased)
		{
			float num = vectorBased ? strength.magnitude : strength.x;
			int num2 = (int)((float)vibrato * duration);
			if (num2 < 2)
			{
				num2 = 2;
			}
			float num3 = num / (float)num2;
			float[] array = new float[num2];
			float num4 = 0f;
			for (int i = 0; i < num2; i++)
			{
				float num5 = (float)(i + 1) / (float)num2;
				float num6 = duration * num5;
				num4 += num6;
				array[i] = num6;
			}
			float num7 = duration / num4;
			for (int j = 0; j < num2; j++)
			{
				array[j] *= num7;
			}
			float num8 = UnityEngine.Random.Range(0f, 360f);
			Vector3[] array2 = new Vector3[num2];
			for (int k = 0; k < num2; k++)
			{
				if (k < num2 - 1)
				{
					if (k > 0)
					{
						num8 = num8 - 180f + UnityEngine.Random.Range(-randomness, randomness);
					}
					if (vectorBased)
					{
						Quaternion rotation = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomness, randomness), Vector3.up);
                        Vector3 vector = rotation * DG.Tweening.Core.Utils.Vector3FromAngle(num8, num);
						vector.x = Vector3.ClampMagnitude(vector, strength.x).x;
						vector.y = Vector3.ClampMagnitude(vector, strength.y).y;
						vector.z = Vector3.ClampMagnitude(vector, strength.z).z;
						array2[k] = vector;
						num -= num3;
						strength = Vector3.ClampMagnitude(strength, num);
					}
					else
					{
						if (ignoreZAxis)
						{
                            array2[k] = DG.Tweening.Core.Utils.Vector3FromAngle(num8, num);
						}
						else
						{
							Quaternion rotation2 = Quaternion.AngleAxis(UnityEngine.Random.Range(-randomness, randomness), Vector3.up);
                            array2[k] = rotation2 * DG.Tweening.Core.Utils.Vector3FromAngle(num8, num);
						}
						num -= num3;
					}
				}
				else
				{
					array2[k] = Vector3.zero;
				}
			}
			return DOTween.ToArray(getter, setter, array2, array).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>().SetSpecialStartupMode(SpecialStartupMode.SetShake);
		}
		public static TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> ToArray(DOGetter<Vector3> getter, DOSetter<Vector3> setter, Vector3[] endValues, float[] durations)
		{
			int num = durations.Length;
			if (num != endValues.Length)
			{
				Debugger.LogError("To Vector3 array tween: endValues and durations arrays must have the same length");
				return null;
			}
			Vector3[] array = new Vector3[num];
			float[] array2 = new float[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = endValues[i];
				array2[i] = durations[i];
			}
			float num2 = 0f;
			for (int j = 0; j < num; j++)
			{
				num2 += array2[j];
			}
			TweenerCore<Vector3, Vector3[], Vector3ArrayOptions> tweenerCore = DOTween.ApplyTo<Vector3, Vector3[], Vector3ArrayOptions>(getter, setter, array, num2, null).NoFrom<Vector3, Vector3[], Vector3ArrayOptions>();
			tweenerCore.plugOptions.durations = array2;
			return tweenerCore;
		}
		public static TweenerCore<Color2, Color2, ColorOptions> To(DOGetter<Color2> getter, DOSetter<Color2> setter, Color2 endValue, float duration)
		{
			return DOTween.ApplyTo<Color2, Color2, ColorOptions>(getter, setter, endValue, duration, null);
		}
		public static Sequence Sequence()
		{
			DOTween.InitCheck();
			Sequence sequence = TweenManager.GetSequence();
			DG.Tweening.Sequence.Setup(sequence);
			return sequence;
		}
		public static int CompleteAll()
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, false, 0f, null);
		}
		public static int Complete(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int CompleteAndReturnKilledTot()
		{
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.All, null, true, 0f, null);
		}
		public static int CompleteAndReturnKilledTot(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Complete, FilterType.TargetOrId, targetOrId, true, 0f, null);
		}
		public static int FlipAll()
		{
			return TweenManager.FilteredOperation(OperationType.Flip, FilterType.All, null, false, 0f, null);
		}
		public static int Flip(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Flip, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int GotoAll(float to, bool andPlay = false)
		{
			return TweenManager.FilteredOperation(OperationType.Goto, FilterType.All, null, andPlay, to, null);
		}
		public static int Goto(object targetOrId, float to, bool andPlay = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Goto, FilterType.TargetOrId, targetOrId, andPlay, to, null);
		}
		public static int KillAll(bool complete = false)
		{
			int num = complete ? DOTween.CompleteAndReturnKilledTot() : 0;
			return num + TweenManager.DespawnAll();
		}
		public static int Kill(object targetOrId, bool complete = false)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			int num = complete ? DOTween.CompleteAndReturnKilledTot(targetOrId) : 0;
			return num + TweenManager.FilteredOperation(OperationType.Despawn, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int PauseAll()
		{
			return TweenManager.FilteredOperation(OperationType.Pause, FilterType.All, null, false, 0f, null);
		}
		public static int Pause(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Pause, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int PlayAll()
		{
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.All, null, false, 0f, null);
		}
		public static int Play(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int Play(object target, object id)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Play, FilterType.TargetAndId, id, false, 0f, target);
		}
		public static int PlayBackwardsAll()
		{
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.All, null, false, 0f, null);
		}
		public static int PlayBackwards(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayBackwards, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int PlayForwardAll()
		{
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.All, null, false, 0f, null);
		}
		public static int PlayForward(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.PlayForward, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int RestartAll(bool includeDelay = true)
		{
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.All, null, includeDelay, 0f, null);
		}
		public static int Restart(object targetOrId, bool includeDelay)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetOrId, targetOrId, includeDelay, 0f, null);
		}
		public static int Restart(object target, object id, bool includeDelay)
		{
			if (target == null || id == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Restart, FilterType.TargetAndId, id, includeDelay, 0f, target);
		}
		public static int RewindAll(bool includeDelay = true)
		{
			return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.All, null, includeDelay, 0f, null);
		}
		public static int Rewind(object targetOrId, bool includeDelay = true)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.Rewind, FilterType.TargetOrId, targetOrId, includeDelay, 0f, null);
		}
		public static int SmoothRewindAll()
		{
			return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.All, null, false, 0f, null);
		}
		public static int SmoothRewind(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.SmoothRewind, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static int TogglePauseAll()
		{
			return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.All, null, false, 0f, null);
		}
		public static int TogglePause(object targetOrId)
		{
			if (targetOrId == null)
			{
				return 0;
			}
			return TweenManager.FilteredOperation(OperationType.TogglePause, FilterType.TargetOrId, targetOrId, false, 0f, null);
		}
		public static bool IsTweening(object targetOrId)
		{
			return TweenManager.FilteredOperation(OperationType.IsTweening, FilterType.TargetOrId, targetOrId, false, 0f, null) > 0;
		}
		public static int TotalPlayingTweens()
		{
			return TweenManager.TotalPlayingTweens();
		}
		public static List<Tween> PlayingTweens()
		{
			return TweenManager.GetActiveTweens(true);
		}
		public static List<Tween> PausedTweens()
		{
			return TweenManager.GetActiveTweens(false);
		}
		public static List<Tween> TweensById(object id, bool playingOnly = false)
		{
			if (id == null)
			{
				return null;
			}
			return TweenManager.GetTweensById(id, playingOnly);
		}
		public static List<Tween> TweensByTarget(object target, bool playingOnly = false)
		{
			return TweenManager.GetTweensByTarget(target, playingOnly);
		}
		private static void InitCheck()
		{
			if (DOTween.initialized || !Application.isPlaying || DOTween.isQuitting)
			{
				return;
			}
			DOTween.AutoInit();
		}
		private static TweenerCore<T1, T2, TPlugOptions> ApplyTo<T1, T2, TPlugOptions>(DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration, ABSTweenPlugin<T1, T2, TPlugOptions> plugin = null) where TPlugOptions : struct
		{
			DOTween.InitCheck();
			TweenerCore<T1, T2, TPlugOptions> tweener = TweenManager.GetTweener<T1, T2, TPlugOptions>();
			if (!Tweener.Setup<T1, T2, TPlugOptions>(tweener, getter, setter, endValue, duration, plugin))
			{
				TweenManager.Despawn(tweener, true);
				return null;
			}
			return tweener;
		}
	}
}
