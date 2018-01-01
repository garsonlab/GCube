using System;
using UnityEngine;
namespace DG.Tweening.Plugins.Core.PathCore
{
	[Serializable]
	public class Path
	{
		private static CatmullRomDecoder _catmullRomDecoder;
		private static LinearDecoder _linearDecoder;
		[SerializeField]
		public PathType type;
		[SerializeField]
		public int subdivisionsXSegment;
		[SerializeField]
		public int subdivisions;
		[SerializeField]
		public Vector3[] wps;
		[SerializeField]
		public ControlPoint[] controlPoints;
		[SerializeField]
		public float length;
		[SerializeField]
		public float[] wpLengths;
		[SerializeField]
		public bool isFinalized;
		[SerializeField]
		public float[] timesTable;
		[SerializeField]
		public float[] lengthsTable;
		public int linearWPIndex = -1;
		private ABSPathDecoder _decoder;
		private bool _changed;
		public Vector3[] nonLinearDrawWps;
		public Vector3 targetPosition;
		public Vector3? lookAtPosition;
		public Color gizmoColor = new Color(1f, 1f, 1f, 0.7f);
		public Path(PathType type, Vector3[] waypoints, int subdivisionsXSegment, Color? gizmoColor = null)
		{
			this.type = type;
			this.subdivisionsXSegment = subdivisionsXSegment;
			if (gizmoColor.HasValue)
			{
				this.gizmoColor = gizmoColor.Value;
			}
			this.AssignWaypoints(waypoints, true);
			this.AssignDecoder(type);
			if (DOTween.isUnityEditor)
			{
				DOTween.GizmosDelegates.Add(new TweenCallback(this.Draw));
			}
		}
		public void FinalizePath(bool isClosedPath, AxisConstraint lockPositionAxes, Vector3 currTargetVal)
		{
			if (lockPositionAxes != AxisConstraint.None)
			{
				bool flag = (lockPositionAxes & AxisConstraint.X) == AxisConstraint.X;
				bool flag2 = (lockPositionAxes & AxisConstraint.Y) == AxisConstraint.Y;
				bool flag3 = (lockPositionAxes & AxisConstraint.Z) == AxisConstraint.Z;
				for (int i = 0; i < this.wps.Length; i++)
				{
					Vector3 vector = this.wps[i];
					this.wps[i] = new Vector3(flag ? currTargetVal.x : vector.x, flag2 ? currTargetVal.y : vector.y, flag3 ? currTargetVal.z : vector.z);
				}
			}
			this._decoder.FinalizePath(this, this.wps, isClosedPath);
			this.isFinalized = true;
		}
		public Vector3 GetPoint(float perc, bool convertToConstantPerc = false)
		{
			if (convertToConstantPerc)
			{
				perc = this.ConvertToConstantPathPerc(perc);
			}
			return this._decoder.GetPoint(perc, this.wps, this, this.controlPoints);
		}
		public float ConvertToConstantPathPerc(float perc)
		{
			if (this.type == PathType.Linear)
			{
				return perc;
			}
			if (perc > 0f && perc < 1f)
			{
				float num = this.length * perc;
				float num2 = 0f;
				float num3 = 0f;
				float num4 = 0f;
				float num5 = 0f;
				int num6 = this.lengthsTable.Length;
				int i = 0;
				while (i < num6)
				{
					if (this.lengthsTable[i] > num)
					{
						num4 = this.timesTable[i];
						num5 = this.lengthsTable[i];
						if (i > 0)
						{
							num3 = this.lengthsTable[i - 1];
							break;
						}
						break;
					}
					else
					{
						num2 = this.timesTable[i];
						i++;
					}
				}
				perc = num2 + (num - num3) / (num5 - num3) * (num4 - num2);
			}
			if (perc > 1f)
			{
				perc = 1f;
			}
			else if (perc < 0f)
			{
				perc = 0f;
			}
			return perc;
		}
		public int GetWaypointIndexFromPerc(float perc, bool isMovingForward)
		{
			if (perc >= 1f)
			{
				return this.wps.Length - 1;
			}
			if (perc <= 0f)
			{
				return 0;
			}
			float num = this.length * perc;
			float num2 = 0f;
			int i = 0;
			int num3 = this.wpLengths.Length;
			while (i < num3)
			{
				num2 += this.wpLengths[i];
				if (num2 >= num)
				{
					if (num2 <= num)
					{
						return i;
					}
					if (!isMovingForward)
					{
						return i;
					}
					return i - 1;
				}
				else
				{
					i++;
				}
			}
			return 0;
		}
		public static Vector3[] GetDrawPoints(Path p, int drawSubdivisionsXSegment)
		{
			int num = p.wps.Length;
			if (p.type == PathType.Linear)
			{
				return p.wps;
			}
			int num2 = num * drawSubdivisionsXSegment;
			Vector3[] array = new Vector3[num2 + 1];
			for (int i = 0; i <= num2; i++)
			{
				float perc = (float)i / (float)num2;
				Vector3 point = p.GetPoint(perc, false);
				array[i] = point;
			}
			return array;
		}
		public static void RefreshNonLinearDrawWps(Path p)
		{
			int num = p.wps.Length;
			int num2 = num * 10;
			if (p.nonLinearDrawWps == null || p.nonLinearDrawWps.Length != num2 + 1)
			{
				p.nonLinearDrawWps = new Vector3[num2 + 1];
			}
			for (int i = 0; i <= num2; i++)
			{
				float perc = (float)i / (float)num2;
				Vector3 point = p.GetPoint(perc, false);
				p.nonLinearDrawWps[i] = point;
			}
		}
		public void Destroy()
		{
			if (DOTween.isUnityEditor)
			{
				DOTween.GizmosDelegates.Remove(new TweenCallback(this.Draw));
			}
			this.wps = null;
			this.wpLengths = (this.timesTable = (this.lengthsTable = null));
			this.nonLinearDrawWps = null;
			this.isFinalized = false;
		}
		public void AssignWaypoints(Vector3[] newWps, bool cloneWps = false)
		{
			if (cloneWps)
			{
				int num = newWps.Length;
				this.wps = new Vector3[num];
				for (int i = 0; i < num; i++)
				{
					this.wps[i] = newWps[i];
				}
				return;
			}
			this.wps = newWps;
		}
		public void AssignDecoder(PathType pathType)
		{
			this.type = pathType;
			if (pathType == PathType.Linear)
			{
				if (Path._linearDecoder == null)
				{
					Path._linearDecoder = new LinearDecoder();
				}
				this._decoder = Path._linearDecoder;
				return;
			}
			if (Path._catmullRomDecoder == null)
			{
				Path._catmullRomDecoder = new CatmullRomDecoder();
			}
			this._decoder = Path._catmullRomDecoder;
		}
		public void Draw()
		{
			Path.Draw(this);
		}
		private static void Draw(Path p)
		{
			if (p.timesTable == null)
			{
				return;
			}
			Color color = p.gizmoColor;
			color.a *= 0.5f;
			Gizmos.color = p.gizmoColor;
			int num = p.wps.Length;
			if (p._changed || (p.type != PathType.Linear && p.nonLinearDrawWps == null))
			{
				p._changed = false;
				if (p.type != PathType.Linear)
				{
					Path.RefreshNonLinearDrawWps(p);
				}
			}
			PathType pathType = p.type;
			if (pathType == PathType.Linear)
			{
				Vector3 to = p.wps[0];
				for (int i = 0; i < num; i++)
				{
					Vector3 vector = p.wps[i];
					Gizmos.DrawLine(vector, to);
					to = vector;
				}
			}
			else
			{
				Vector3 to = p.nonLinearDrawWps[0];
				int num2 = p.nonLinearDrawWps.Length;
				for (int j = 1; j < num2; j++)
				{
					Vector3 vector = p.nonLinearDrawWps[j];
					Gizmos.DrawLine(vector, to);
					to = vector;
				}
			}
			Gizmos.color = color;
			for (int k = 0; k < num; k++)
			{
				Gizmos.DrawSphere(p.wps[k], 0.075f);
			}
			if (p.lookAtPosition.HasValue)
			{
				Vector3 value = p.lookAtPosition.Value;
				Gizmos.DrawLine(p.targetPosition, value);
				Gizmos.DrawWireSphere(value, 0.075f);
			}
		}
	}
}
