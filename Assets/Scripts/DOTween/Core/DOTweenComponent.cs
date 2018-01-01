using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace DG.Tweening.Core
{
	[AddComponentMenu("")]
	public class DOTweenComponent : MonoBehaviour, IDOTweenInit
	{
		public int inspectorUpdater;
		private float _unscaledTime;
		private float _unscaledDeltaTime;
		private bool _duplicateToDestroy;
		private void Awake()
		{
			this.inspectorUpdater = 0;
			this._unscaledTime = Time.realtimeSinceStartup;
		}
		private void Start()
		{
			if (DOTween.instance != this)
			{
				this._duplicateToDestroy = true;
				UnityEngine.Object.Destroy(base.gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
			}
		}
		private void Update()
		{
			this._unscaledDeltaTime = Time.realtimeSinceStartup - this._unscaledTime;
			if (TweenManager.hasActiveDefaultTweens)
			{
				TweenManager.Update(UpdateType.Normal, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
			}
			this._unscaledTime = Time.realtimeSinceStartup;
			if (DOTween.isUnityEditor)
			{
				this.inspectorUpdater++;
				if (DOTween.showUnityEditorReport && TweenManager.hasActiveTweens)
				{
					if (TweenManager.totActiveTweeners > DOTween.maxActiveTweenersReached)
					{
						DOTween.maxActiveTweenersReached = TweenManager.totActiveTweeners;
					}
					if (TweenManager.totActiveSequences > DOTween.maxActiveSequencesReached)
					{
						DOTween.maxActiveSequencesReached = TweenManager.totActiveSequences;
					}
				}
			}
		}
		private void LateUpdate()
		{
			if (TweenManager.hasActiveLateTweens)
			{
				TweenManager.Update(UpdateType.Late, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, this._unscaledDeltaTime * DOTween.timeScale);
			}
		}
		private void FixedUpdate()
		{
			if (TweenManager.hasActiveFixedTweens && Time.timeScale > 0f)
			{
				TweenManager.Update(UpdateType.Fixed, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) / Time.timeScale * DOTween.timeScale);
			}
		}
		private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			if (DOTween.useSafeMode)
			{
				DOTween.Validate();
			}
		}
		private void OnDrawGizmos()
		{
			if (!DOTween.drawGizmos || !DOTween.isUnityEditor)
			{
				return;
			}
			int count = DOTween.GizmosDelegates.Count;
			if (count == 0)
			{
				return;
			}
			for (int i = 0; i < count; i++)
			{
				DOTween.GizmosDelegates[i]();
			}
		}
		private void OnDestroy()
		{
			if (this._duplicateToDestroy)
			{
				return;
			}
			if (DOTween.showUnityEditorReport)
			{
				string message = string.Concat(new object[]
				{
					"REPORT > Max overall simultaneous active Tweeners/Sequences: ",
					DOTween.maxActiveTweenersReached,
					"/",
					DOTween.maxActiveSequencesReached
				});
				Debugger.LogReport(message);
			}
			if (DOTween.instance == this)
			{
				DOTween.instance = null;
			}
		}
		private void OnApplicationQuit()
		{
			DOTween.isQuitting = true;
		}
		public IDOTweenInit SetCapacity(int tweenersCapacity, int sequencesCapacity)
		{
			TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
			return this;
		}
		public IEnumerator WaitForCompletion(Tween t)
		{
			while (t.active && !t.isComplete)
			{
				yield return null;
			}
			yield break;
		}
		public IEnumerator WaitForRewind(Tween t)
		{
			while (t.active && (!t.playedOnce || t.position * (float)(t.completedLoops + 1) > 0f))
			{
				yield return null;
			}
			yield break;
		}
		public IEnumerator WaitForKill(Tween t)
		{
			while (t.active)
			{
				yield return null;
			}
			yield break;
		}
		public IEnumerator WaitForElapsedLoops(Tween t, int elapsedLoops)
		{
			while (t.active && t.completedLoops < elapsedLoops)
			{
				yield return null;
			}
			yield break;
		}
		public IEnumerator WaitForPosition(Tween t, float position)
		{
			while (t.active && t.position * (float)(t.completedLoops + 1) < position)
			{
				yield return null;
			}
			yield break;
		}
		public IEnumerator WaitForStart(Tween t)
		{
			while (t.active && !t.playedOnce)
			{
				yield return null;
			}
			yield break;
		}
		public static void Create()
		{
			if (DOTween.instance != null)
			{
				return;
			}
			GameObject gameObject = new GameObject("[DOTween]");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			DOTween.instance = gameObject.AddComponent<DOTweenComponent>();
		}
		public static void DestroyInstance()
		{
			if (DOTween.instance != null)
			{
				UnityEngine.Object.Destroy(DOTween.instance.gameObject);
			}
			DOTween.instance = null;
		}
	}
}
