using DG.Tweening.Core.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace DG.Tweening.Core
{
	public static class TweenManager
	{
		public enum CapacityIncreaseMode
		{
			TweenersAndSequences,
			TweenersOnly,
			SequencesOnly
		}
		private const int _DefaultMaxTweeners = 200;
		private const int _DefaultMaxSequences = 50;
		private const string _MaxTweensReached = "Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup";
		public static int maxActive = 200;
		public static int maxTweeners = 200;
		public static int maxSequences = 50;
		public static bool hasActiveTweens;
		public static bool hasActiveDefaultTweens;
		public static bool hasActiveLateTweens;
		public static bool hasActiveFixedTweens;
		public static int totActiveTweens;
		public static int totActiveDefaultTweens;
		public static int totActiveLateTweens;
		public static int totActiveFixedTweens;
		public static int totActiveTweeners;
		public static int totActiveSequences;
		public static int totPooledTweeners;
		public static int totPooledSequences;
		public static int totTweeners;
		public static int totSequences;
		public static bool isUpdateLoop;
		private static Tween[] _activeTweens = new Tween[200];
		private static Tween[] _pooledTweeners = new Tween[200];
		private static readonly Stack<Tween> _PooledSequences = new Stack<Tween>();
		private static readonly List<Tween> _KillList = new List<Tween>(200);
		private static int _maxActiveLookupId = -1;
		private static bool _requiresActiveReorganization;
		private static int _reorganizeFromId = -1;
		private static int _minPooledTweenerId = -1;
		private static int _maxPooledTweenerId = -1;
		public static TweenerCore<T1, T2, TPlugOptions> GetTweener<T1, T2, TPlugOptions>() where TPlugOptions : struct
		{
			TweenerCore<T1, T2, TPlugOptions> tweenerCore;
			if (TweenManager.totPooledTweeners > 0)
			{
				Type typeFromHandle = typeof(T1);
				Type typeFromHandle2 = typeof(T2);
				Type typeFromHandle3 = typeof(TPlugOptions);
				for (int i = TweenManager._maxPooledTweenerId; i > TweenManager._minPooledTweenerId - 1; i--)
				{
					Tween tween = TweenManager._pooledTweeners[i];
					if (tween != null && tween.typeofT1 == typeFromHandle && tween.typeofT2 == typeFromHandle2 && tween.typeofTPlugOptions == typeFromHandle3)
					{
						tweenerCore = (TweenerCore<T1, T2, TPlugOptions>)tween;
						TweenManager.AddActiveTween(tweenerCore);
						TweenManager._pooledTweeners[i] = null;
						if (TweenManager._maxPooledTweenerId != TweenManager._minPooledTweenerId)
						{
							if (i == TweenManager._maxPooledTweenerId)
							{
								TweenManager._maxPooledTweenerId--;
							}
							else if (i == TweenManager._minPooledTweenerId)
							{
								TweenManager._minPooledTweenerId++;
							}
						}
						TweenManager.totPooledTweeners--;
						return tweenerCore;
					}
				}
				if (TweenManager.totTweeners >= TweenManager.maxTweeners)
				{
					TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId] = null;
					TweenManager._maxPooledTweenerId--;
					TweenManager.totPooledTweeners--;
					TweenManager.totTweeners--;
				}
			}
			else if (TweenManager.totTweeners >= TweenManager.maxTweeners - 1)
			{
				int num = TweenManager.maxTweeners;
				int num2 = TweenManager.maxSequences;
				TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.TweenersOnly);
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", num + "/" + num2).Replace("#1", TweenManager.maxTweeners + "/" + TweenManager.maxSequences));
				}
			}
			tweenerCore = new TweenerCore<T1, T2, TPlugOptions>();
			TweenManager.totTweeners++;
			TweenManager.AddActiveTween(tweenerCore);
			return tweenerCore;
		}
		public static Sequence GetSequence()
		{
			Sequence sequence;
			if (TweenManager.totPooledSequences > 0)
			{
				sequence = (Sequence)TweenManager._PooledSequences.Pop();
				TweenManager.AddActiveTween(sequence);
				TweenManager.totPooledSequences--;
				return sequence;
			}
			if (TweenManager.totSequences >= TweenManager.maxSequences - 1)
			{
				int num = TweenManager.maxTweeners;
				int num2 = TweenManager.maxSequences;
				TweenManager.IncreaseCapacities(TweenManager.CapacityIncreaseMode.SequencesOnly);
				if (Debugger.logPriority >= 1)
				{
					Debugger.LogWarning("Max Tweens reached: capacity has automatically been increased from #0 to #1. Use DOTween.SetTweensCapacity to set it manually at startup".Replace("#0", num + "/" + num2).Replace("#1", TweenManager.maxTweeners + "/" + TweenManager.maxSequences));
				}
			}
			sequence = new Sequence();
			TweenManager.totSequences++;
			TweenManager.AddActiveTween(sequence);
			return sequence;
		}
		public static void SetUpdateType(Tween t, UpdateType updateType, bool isIndependentUpdate)
		{
			if (!t.active || t.updateType == updateType)
			{
				t.updateType = updateType;
				t.isIndependentUpdate = isIndependentUpdate;
				return;
			}
			if (t.updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens--;
				TweenManager.hasActiveDefaultTweens = (TweenManager.totActiveDefaultTweens > 0);
			}
			else if (t.updateType == UpdateType.Fixed)
			{
				TweenManager.totActiveFixedTweens--;
				TweenManager.hasActiveFixedTweens = (TweenManager.totActiveFixedTweens > 0);
			}
			else
			{
				TweenManager.totActiveLateTweens--;
				TweenManager.hasActiveLateTweens = (TweenManager.totActiveLateTweens > 0);
			}
			t.updateType = updateType;
			t.isIndependentUpdate = isIndependentUpdate;
			if (updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens++;
				TweenManager.hasActiveDefaultTweens = true;
				return;
			}
			if (updateType == UpdateType.Fixed)
			{
				TweenManager.totActiveFixedTweens++;
				TweenManager.hasActiveFixedTweens = true;
				return;
			}
			TweenManager.totActiveLateTweens++;
			TweenManager.hasActiveLateTweens = true;
		}
		public static void AddActiveTweenToSequence(Tween t)
		{
			TweenManager.RemoveActiveTween(t);
		}
		public static int DespawnAll()
		{
			int result = TweenManager.totActiveTweens;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null)
				{
					TweenManager.Despawn(tween, false);
				}
			}
			TweenManager.ClearTweenArray(TweenManager._activeTweens);
			TweenManager.hasActiveTweens = (TweenManager.hasActiveDefaultTweens = (TweenManager.hasActiveLateTweens = (TweenManager.hasActiveFixedTweens = false)));
			TweenManager.totActiveTweens = (TweenManager.totActiveDefaultTweens = (TweenManager.totActiveLateTweens = (TweenManager.totActiveFixedTweens = 0)));
			TweenManager.totActiveTweeners = (TweenManager.totActiveSequences = 0);
			TweenManager._maxActiveLookupId = (TweenManager._reorganizeFromId = -1);
			TweenManager._requiresActiveReorganization = false;
			return result;
		}
		public static void Despawn(Tween t, bool modifyActiveLists = true)
		{
			if (t.onKill != null)
			{
				Tween.OnTweenCallback(t.onKill);
			}
			if (modifyActiveLists)
			{
				TweenManager.RemoveActiveTween(t);
			}
			if (t.isRecyclable)
			{
				switch (t.tweenType)
				{
				case TweenType.Tweener:
					if (TweenManager._maxPooledTweenerId == -1)
					{
						TweenManager._maxPooledTweenerId = TweenManager.maxTweeners - 1;
						TweenManager._minPooledTweenerId = TweenManager.maxTweeners - 1;
					}
					if (TweenManager._maxPooledTweenerId < TweenManager.maxTweeners - 1)
					{
						TweenManager._pooledTweeners[TweenManager._maxPooledTweenerId + 1] = t;
						TweenManager._maxPooledTweenerId++;
						if (TweenManager._minPooledTweenerId > TweenManager._maxPooledTweenerId)
						{
							TweenManager._minPooledTweenerId = TweenManager._maxPooledTweenerId;
						}
					}
					else
					{
						int i = TweenManager._maxPooledTweenerId;
						while (i > -1)
						{
							if (TweenManager._pooledTweeners[i] == null)
							{
								TweenManager._pooledTweeners[i] = t;
								if (i < TweenManager._minPooledTweenerId)
								{
									TweenManager._minPooledTweenerId = i;
								}
								if (TweenManager._maxPooledTweenerId < TweenManager._minPooledTweenerId)
								{
									TweenManager._maxPooledTweenerId = TweenManager._minPooledTweenerId;
									break;
								}
								break;
							}
							else
							{
								i--;
							}
						}
					}
					TweenManager.totPooledTweeners++;
					break;
				case TweenType.Sequence:
				{
					TweenManager._PooledSequences.Push(t);
					TweenManager.totPooledSequences++;
					Sequence sequence = (Sequence)t;
					int count = sequence.sequencedTweens.Count;
					for (int j = 0; j < count; j++)
					{
						TweenManager.Despawn(sequence.sequencedTweens[j], false);
					}
					break;
				}
				}
			}
			else
			{
				switch (t.tweenType)
				{
				case TweenType.Tweener:
					TweenManager.totTweeners--;
					break;
				case TweenType.Sequence:
				{
					TweenManager.totSequences--;
					Sequence sequence2 = (Sequence)t;
					int count2 = sequence2.sequencedTweens.Count;
					for (int k = 0; k < count2; k++)
					{
						TweenManager.Despawn(sequence2.sequencedTweens[k], false);
					}
					break;
				}
				}
			}
			t.active = false;
			t.Reset();
		}
		public static void PurgeAll()
		{
			for (int i = 0; i < TweenManager.totActiveTweens; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.onKill != null)
				{
					Tween.OnTweenCallback(tween.onKill);
				}
			}
			TweenManager.ClearTweenArray(TweenManager._activeTweens);
			TweenManager.hasActiveTweens = (TweenManager.hasActiveDefaultTweens = (TweenManager.hasActiveLateTweens = (TweenManager.hasActiveFixedTweens = false)));
			TweenManager.totActiveTweens = (TweenManager.totActiveDefaultTweens = (TweenManager.totActiveLateTweens = (TweenManager.totActiveFixedTweens = 0)));
			TweenManager.totActiveTweeners = (TweenManager.totActiveSequences = 0);
			TweenManager._maxActiveLookupId = (TweenManager._reorganizeFromId = -1);
			TweenManager._requiresActiveReorganization = false;
			TweenManager.PurgePools();
			TweenManager.ResetCapacities();
			TweenManager.totTweeners = (TweenManager.totSequences = 0);
		}
		public static void PurgePools()
		{
			TweenManager.totTweeners -= TweenManager.totPooledTweeners;
			TweenManager.totSequences -= TweenManager.totPooledSequences;
			TweenManager.ClearTweenArray(TweenManager._pooledTweeners);
			TweenManager._PooledSequences.Clear();
			TweenManager.totPooledTweeners = (TweenManager.totPooledSequences = 0);
			TweenManager._minPooledTweenerId = (TweenManager._maxPooledTweenerId = -1);
		}
		public static void ResetCapacities()
		{
			TweenManager.SetCapacities(200, 50);
		}
		public static void SetCapacities(int tweenersCapacity, int sequencesCapacity)
		{
			if (tweenersCapacity < sequencesCapacity)
			{
				tweenersCapacity = sequencesCapacity;
			}
			TweenManager.maxActive = tweenersCapacity + sequencesCapacity;
			TweenManager.maxTweeners = tweenersCapacity;
			TweenManager.maxSequences = sequencesCapacity;
			Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
			Array.Resize<Tween>(ref TweenManager._pooledTweeners, tweenersCapacity);
			TweenManager._KillList.Capacity = TweenManager.maxActive;
		}
		public static int Validate()
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			int num = 0;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (!tween.Validate())
				{
					num++;
					TweenManager.MarkForKilling(tween);
				}
			}
			if (num > 0)
			{
				TweenManager.DespawnTweens(TweenManager._KillList, false);
				int num2 = TweenManager._KillList.Count - 1;
				for (int j = num2; j > -1; j--)
				{
					TweenManager.RemoveActiveTween(TweenManager._KillList[j]);
				}
				TweenManager._KillList.Clear();
			}
			return num;
		}
		public static void Update(UpdateType updateType, float deltaTime, float independentTime)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			TweenManager.isUpdateLoop = true;
			bool flag = false;
			int num = TweenManager._maxActiveLookupId + 1;
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.updateType == updateType)
				{
					if (!tween.active)
					{
						flag = true;
						TweenManager.MarkForKilling(tween);
					}
					else if (tween.isPlaying)
					{
						tween.creationLocked = true;
						float num2 = (tween.isIndependentUpdate ? independentTime : deltaTime) * tween.timeScale;
						if (!tween.delayComplete)
						{
							num2 = tween.UpdateDelay(tween.elapsedDelay + num2);
							if (num2 <= -1f)
							{
								flag = true;
								TweenManager.MarkForKilling(tween);
								goto IL_1CB;
							}
							if (num2 <= 0f)
							{
								goto IL_1CB;
							}
						}
						if (!tween.startupDone && !tween.Startup())
						{
							flag = true;
							TweenManager.MarkForKilling(tween);
						}
						else
						{
							float num3 = tween.position;
							bool flag2 = num3 >= tween.duration;
							int num4 = tween.completedLoops;
							if (tween.duration <= 0f)
							{
								num3 = 0f;
								num4 = ((tween.loops == -1) ? (tween.completedLoops + 1) : tween.loops);
							}
							else
							{
								if (tween.isBackwards)
								{
									num3 -= num2;
									while (num3 < 0f)
									{
										if (num4 <= 0)
										{
											break;
										}
										num3 += tween.duration;
										num4--;
									}
								}
								else
								{
									num3 += num2;
									while (num3 >= tween.duration && (tween.loops == -1 || num4 < tween.loops))
									{
										num3 -= tween.duration;
										num4++;
									}
								}
								if (flag2)
								{
									num4--;
								}
								if (tween.loops != -1 && num4 >= tween.loops)
								{
									num3 = tween.duration;
								}
							}
							bool flag3 = Tween.DoGoto(tween, num3, num4, UpdateMode.Update);
							if (flag3)
							{
								flag = true;
								TweenManager.MarkForKilling(tween);
							}
						}
					}
				}
				IL_1CB:;
			}
			if (flag)
			{
				TweenManager.DespawnTweens(TweenManager._KillList, false);
				int num5 = TweenManager._KillList.Count - 1;
				for (int j = num5; j > -1; j--)
				{
					TweenManager.RemoveActiveTween(TweenManager._KillList[j]);
				}
				TweenManager._KillList.Clear();
			}
			TweenManager.isUpdateLoop = false;
		}
		public static int FilteredOperation(OperationType operationType, FilterType filterType, object id, bool optionalBool, float optionalFloat, object optionalObj = null)
		{
			int num = 0;
			bool flag = false;
			for (int i = TweenManager._maxActiveLookupId; i > -1; i--)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.active)
				{
					bool flag2 = false;
					switch (filterType)
					{
					case FilterType.All:
						flag2 = true;
						break;
					case FilterType.TargetOrId:
						flag2 = (id.Equals(tween.id) || id.Equals(tween.target));
						break;
					case FilterType.TargetAndId:
						flag2 = (id.Equals(tween.id) && optionalObj != null && optionalObj.Equals(tween.target));
						break;
					}
					if (flag2)
					{
						switch (operationType)
						{
						case OperationType.Complete:
						{
							bool autoKill = tween.autoKill;
							if (TweenManager.Complete(tween, false))
							{
								num += ((!optionalBool) ? 1 : (autoKill ? 1 : 0));
								if (autoKill)
								{
									if (TweenManager.isUpdateLoop)
									{
										tween.active = false;
									}
									else
									{
										flag = true;
										TweenManager._KillList.Add(tween);
									}
								}
							}
							break;
						}
						case OperationType.Despawn:
							num++;
							if (TweenManager.isUpdateLoop)
							{
								tween.active = false;
							}
							else
							{
								TweenManager.Despawn(tween, false);
								flag = true;
								TweenManager._KillList.Add(tween);
							}
							break;
						case OperationType.Flip:
							if (TweenManager.Flip(tween))
							{
								num++;
							}
							break;
						case OperationType.Goto:
							TweenManager.Goto(tween, optionalFloat, optionalBool, UpdateMode.Goto);
							num++;
							break;
						case OperationType.Pause:
							if (TweenManager.Pause(tween))
							{
								num++;
							}
							break;
						case OperationType.Play:
							if (TweenManager.Play(tween))
							{
								num++;
							}
							break;
						case OperationType.PlayForward:
							if (TweenManager.PlayForward(tween))
							{
								num++;
							}
							break;
						case OperationType.PlayBackwards:
							if (TweenManager.PlayBackwards(tween))
							{
								num++;
							}
							break;
						case OperationType.Rewind:
							if (TweenManager.Rewind(tween, optionalBool))
							{
								num++;
							}
							break;
						case OperationType.SmoothRewind:
							if (TweenManager.SmoothRewind(tween))
							{
								num++;
							}
							break;
						case OperationType.Restart:
							if (TweenManager.Restart(tween, optionalBool))
							{
								num++;
							}
							break;
						case OperationType.TogglePause:
							if (TweenManager.TogglePause(tween))
							{
								num++;
							}
							break;
						case OperationType.IsTweening:
							if (!tween.isComplete || !tween.autoKill)
							{
								num++;
							}
							break;
						}
					}
				}
			}
			if (flag)
			{
				int num2 = TweenManager._KillList.Count - 1;
				for (int j = num2; j > -1; j--)
				{
					TweenManager.RemoveActiveTween(TweenManager._KillList[j]);
				}
				TweenManager._KillList.Clear();
			}
			return num;
		}
		public static bool Complete(Tween t, bool modifyActiveLists = true)
		{
			if (t.loops == -1)
			{
				return false;
			}
			if (!t.isComplete)
			{
				Tween.DoGoto(t, t.duration, t.loops, UpdateMode.Goto);
				t.isPlaying = false;
				if (t.autoKill)
				{
					if (TweenManager.isUpdateLoop)
					{
						t.active = false;
					}
					else
					{
						TweenManager.Despawn(t, modifyActiveLists);
					}
				}
				return true;
			}
			return false;
		}
		public static bool Flip(Tween t)
		{
			t.isBackwards = !t.isBackwards;
			return true;
		}
		public static void ForceInit(Tween t)
		{
			if (t.startupDone)
			{
				return;
			}
			if (!t.Startup())
			{
				if (TweenManager.isUpdateLoop)
				{
					t.active = false;
					return;
				}
				TweenManager.RemoveActiveTween(t);
			}
		}
		public static bool Goto(Tween t, float to, bool andPlay = false, UpdateMode updateMode = UpdateMode.Goto)
		{
			bool isPlaying = t.isPlaying;
			t.isPlaying = andPlay;
			t.delayComplete = true;
			t.elapsedDelay = t.delay;
			int num = Mathf.FloorToInt(to / t.duration);
			float num2 = to % t.duration;
			if (t.loops != -1 && num >= t.loops)
			{
				num = t.loops;
				num2 = t.duration;
			}
			else if (num2 >= t.duration)
			{
				num2 = 0f;
			}
			bool flag = Tween.DoGoto(t, num2, num, updateMode);
			if (!andPlay && isPlaying && !flag && t.onPause != null)
			{
				Tween.OnTweenCallback(t.onPause);
			}
			return flag;
		}
		public static bool Pause(Tween t)
		{
			if (t.isPlaying)
			{
				t.isPlaying = false;
				if (t.onPause != null)
				{
					Tween.OnTweenCallback(t.onPause);
				}
				return true;
			}
			return false;
		}
		public static bool Play(Tween t)
		{
			if (!t.isPlaying && ((!t.isBackwards && !t.isComplete) || (t.isBackwards && (t.completedLoops > 0 || t.position > 0f))))
			{
				t.isPlaying = true;
				if (t.playedOnce && t.onPlay != null)
				{
					Tween.OnTweenCallback(t.onPlay);
				}
				return true;
			}
			return false;
		}
		public static bool PlayBackwards(Tween t)
		{
			if (!t.isBackwards)
			{
				t.isBackwards = true;
				TweenManager.Play(t);
				return true;
			}
			return TweenManager.Play(t);
		}
		public static bool PlayForward(Tween t)
		{
			if (t.isBackwards)
			{
				t.isBackwards = false;
				TweenManager.Play(t);
				return true;
			}
			return TweenManager.Play(t);
		}
		public static bool Restart(Tween t, bool includeDelay = true)
		{
			bool isPlaying = t.isPlaying;
			t.isBackwards = false;
			TweenManager.Rewind(t, includeDelay);
			t.isPlaying = true;
			if (isPlaying && t.playedOnce && t.onPlay != null)
			{
				Tween.OnTweenCallback(t.onPlay);
			}
			return true;
		}
		public static bool Rewind(Tween t, bool includeDelay = true)
		{
			bool isPlaying = t.isPlaying;
			t.isPlaying = false;
			bool result = false;
			if (t.delay > 0f)
			{
				if (includeDelay)
				{
					result = (t.delay > 0f && t.elapsedDelay > 0f);
					t.elapsedDelay = 0f;
					t.delayComplete = false;
				}
				else
				{
					result = (t.elapsedDelay < t.delay);
					t.elapsedDelay = t.delay;
					t.delayComplete = true;
				}
			}
			if (t.position > 0f || t.completedLoops > 0 || !t.startupDone)
			{
				result = true;
				if (!Tween.DoGoto(t, 0f, 0, UpdateMode.Goto) && isPlaying && t.onPause != null)
				{
					Tween.OnTweenCallback(t.onPause);
				}
			}
			return result;
		}
		public static bool SmoothRewind(Tween t)
		{
			bool result = false;
			if (t.delay > 0f)
			{
				result = (t.elapsedDelay < t.delay);
				t.elapsedDelay = t.delay;
				t.delayComplete = true;
			}
			if (t.position > 0f || t.completedLoops > 0 || !t.startupDone)
			{
				result = true;
				if (t.loopType == LoopType.Incremental)
				{
					t.PlayBackwards();
				}
				else
				{
					t.Goto(t.ElapsedDirectionalPercentage() * t.duration, false);
					t.PlayBackwards();
				}
			}
			else
			{
				t.isPlaying = false;
			}
			return result;
		}
		public static bool TogglePause(Tween t)
		{
			if (t.isPlaying)
			{
				return TweenManager.Pause(t);
			}
			return TweenManager.Play(t);
		}
		public static int TotalPooledTweens()
		{
			return TweenManager.totPooledTweeners + TweenManager.totPooledSequences;
		}
		public static int TotalPlayingTweens()
		{
			if (!TweenManager.hasActiveTweens)
			{
				return 0;
			}
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			int num = 0;
			for (int i = 0; i < TweenManager._maxActiveLookupId + 1; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && tween.isPlaying)
				{
					num++;
				}
			}
			return num;
		}
		public static List<Tween> GetActiveTweens(bool playing)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			List<Tween> list = new List<Tween>(num);
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween.isPlaying == playing)
				{
					list.Add(tween);
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			return null;
		}
		public static List<Tween> GetTweensById(object id, bool playingOnly)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			List<Tween> list = new List<Tween>(num);
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween != null && object.Equals(id, tween.id) && (!playingOnly || tween.isPlaying))
				{
					list.Add(tween);
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			return null;
		}
		public static List<Tween> GetTweensByTarget(object target, bool playingOnly)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			if (TweenManager.totActiveTweens <= 0)
			{
				return null;
			}
			int num = TweenManager.totActiveTweens;
			List<Tween> list = new List<Tween>(num);
			for (int i = 0; i < num; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween.target == target && (!playingOnly || tween.isPlaying))
				{
					list.Add(tween);
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			return null;
		}
		private static void MarkForKilling(Tween t)
		{
			t.active = false;
			TweenManager._KillList.Add(t);
		}
		private static void AddActiveTween(Tween t)
		{
			if (TweenManager._requiresActiveReorganization)
			{
				TweenManager.ReorganizeActiveTweens();
			}
			t.active = true;
			t.updateType = DOTween.defaultUpdateType;
			t.isIndependentUpdate = DOTween.defaultTimeScaleIndependent;
			t.activeId = (TweenManager._maxActiveLookupId = TweenManager.totActiveTweens);
			TweenManager._activeTweens[TweenManager.totActiveTweens] = t;
			TweenManager.hasActiveDefaultTweens = true;
			TweenManager.totActiveDefaultTweens++;
			TweenManager.totActiveTweens++;
			if (t.tweenType == TweenType.Tweener)
			{
				TweenManager.totActiveTweeners++;
			}
			else
			{
				TweenManager.totActiveSequences++;
			}
			TweenManager.hasActiveTweens = true;
		}
		private static void ReorganizeActiveTweens()
		{
			if (TweenManager.totActiveTweens <= 0)
			{
				TweenManager._maxActiveLookupId = -1;
				TweenManager._requiresActiveReorganization = false;
				TweenManager._reorganizeFromId = -1;
				return;
			}
			if (TweenManager._reorganizeFromId == TweenManager._maxActiveLookupId)
			{
				TweenManager._maxActiveLookupId--;
				TweenManager._requiresActiveReorganization = false;
				TweenManager._reorganizeFromId = -1;
				return;
			}
			int num = 1;
			int num2 = TweenManager._maxActiveLookupId + 1;
			TweenManager._maxActiveLookupId = TweenManager._reorganizeFromId - 1;
			for (int i = TweenManager._reorganizeFromId + 1; i < num2; i++)
			{
				Tween tween = TweenManager._activeTweens[i];
				if (tween == null)
				{
					num++;
				}
				else
				{
					tween.activeId = (TweenManager._maxActiveLookupId = i - num);
					TweenManager._activeTweens[i - num] = tween;
					TweenManager._activeTweens[i] = null;
				}
			}
			TweenManager._requiresActiveReorganization = false;
			TweenManager._reorganizeFromId = -1;
		}
		private static void DespawnTweens(List<Tween> tweens, bool modifyActiveLists = true)
		{
			int count = tweens.Count;
			for (int i = 0; i < count; i++)
			{
				TweenManager.Despawn(tweens[i], modifyActiveLists);
			}
		}
		private static void RemoveActiveTween(Tween t)
		{
			int activeId = t.activeId;
			t.activeId = -1;
			TweenManager._requiresActiveReorganization = true;
			if (TweenManager._reorganizeFromId == -1 || TweenManager._reorganizeFromId > activeId)
			{
				TweenManager._reorganizeFromId = activeId;
			}
			TweenManager._activeTweens[activeId] = null;
			if (t.updateType == UpdateType.Normal)
			{
				TweenManager.totActiveDefaultTweens--;
				TweenManager.hasActiveDefaultTweens = (TweenManager.totActiveDefaultTweens > 0);
			}
			else if (t.updateType == UpdateType.Fixed)
			{
				TweenManager.totActiveFixedTweens--;
				TweenManager.hasActiveFixedTweens = (TweenManager.totActiveFixedTweens > 0);
			}
			else
			{
				TweenManager.totActiveLateTweens--;
				TweenManager.hasActiveLateTweens = (TweenManager.totActiveLateTweens > 0);
			}
			TweenManager.totActiveTweens--;
			TweenManager.hasActiveTweens = (TweenManager.totActiveTweens > 0);
			if (t.tweenType == TweenType.Tweener)
			{
				TweenManager.totActiveTweeners--;
				return;
			}
			TweenManager.totActiveSequences--;
		}
		private static void ClearTweenArray(Tween[] tweens)
		{
			int num = tweens.Length;
			for (int i = 0; i < num; i++)
			{
				tweens[i] = null;
			}
		}
		private static void IncreaseCapacities(TweenManager.CapacityIncreaseMode increaseMode)
		{
			int num = 0;
			int num2 = Mathf.Max((int)((float)TweenManager.maxTweeners * 1.5f), 200);
			int num3 = Mathf.Max((int)((float)TweenManager.maxSequences * 1.5f), 50);
			switch (increaseMode)
			{
			case TweenManager.CapacityIncreaseMode.TweenersOnly:
				num += num2;
				TweenManager.maxTweeners += num2;
				Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
				break;
			case TweenManager.CapacityIncreaseMode.SequencesOnly:
				num += num3;
				TweenManager.maxSequences += num3;
				break;
			default:
				num += num2;
				TweenManager.maxTweeners += num2;
				TweenManager.maxSequences += num3;
				Array.Resize<Tween>(ref TweenManager._pooledTweeners, TweenManager.maxTweeners);
				break;
			}
			TweenManager.maxActive = TweenManager.maxTweeners + TweenManager.maxSequences;
			Array.Resize<Tween>(ref TweenManager._activeTweens, TweenManager.maxActive);
			if (num > 0)
			{
				TweenManager._KillList.Capacity += num;
			}
		}
	}
}
