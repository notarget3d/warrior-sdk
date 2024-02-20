using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static partial class Utils
{
	public static bool IsSet(int mask, int flag) => (mask & flag) == flag;
	public static bool IsSet(uint mask, uint flag) => (mask & flag) == flag;
	public static bool IsSet(ushort mask, ushort flag) => (mask & flag) == flag;


	public static void DotProduct(Vector3 lhs, Vector3 rhs, out bool forward, out bool right, out float ang)
	{
		ang = Vector3.Dot(lhs, rhs);
		float ang2 = Vector3.Dot(rhs, lhs);

		forward = ang >= 0.0f;
		right = ang2 >= 0.0f;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Vector3 Direction(Vector3 from, Vector3 to) => to - from;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Quaternion RotateTowards(Quaternion from, Quaternion to, float maxDegreesDelta, out float angle)
	{
		angle = Quaternion.Angle(from, to);
		if (angle == 0f)
		{
			return to;
		}

		return Quaternion.SlerpUnclamped(from, to, Mathf.Min(1f, maxDegreesDelta / angle));
	}

	private static readonly Vector4[] s_UnitSphere = MakeUnitSphere(48);
	public static void DrawSphere(Vector4 pos, float radius, Color color, float duration)
	{
		Vector4[] v = s_UnitSphere;
		int len = s_UnitSphere.Length / 3;
		for (int i = 0; i < len; i++)
		{
			var sX = pos + radius * v[0 * len + i];
			var eX = pos + radius * v[0 * len + (i + 1) % len];
			var sY = pos + radius * v[1 * len + i];
			var eY = pos + radius * v[1 * len + (i + 1) % len];
			var sZ = pos + radius * v[2 * len + i];
			var eZ = pos + radius * v[2 * len + (i + 1) % len];
			Debug.DrawLine(sX, eX, color, duration);
			Debug.DrawLine(sY, eY, color, duration);
			Debug.DrawLine(sZ, eZ, color, duration);
		}
	}

	public static void DrawPoint(Vector4 pos, float scale, Color color, float duration)
	{
		var sX = pos + new Vector4(+scale, 0, 0);
		var eX = pos + new Vector4(-scale, 0, 0);
		var sY = pos + new Vector4(0, +scale, 0);
		var eY = pos + new Vector4(0, -scale, 0);
		var sZ = pos + new Vector4(0, 0, +scale);
		var eZ = pos + new Vector4(0, 0, -scale);
		Debug.DrawLine(sX, eX, color, duration);
		Debug.DrawLine(sY, eY, color, duration);
		Debug.DrawLine(sZ, eZ, color, duration);
	}

	public static void DrawPlayerGizmo(Vector3 pos, Color color)
	{
		const float radius = 0.4f;
		const float height = 1.8f;
		Gizmos.color = color;
		Gizmos.DrawWireSphere(pos + new Vector3(0.0f, radius, 0.0f), radius);
		Gizmos.DrawWireSphere(pos + new Vector3(0.0f, height - radius, 0.0f), radius);
	}

	private static Vector4[] MakeUnitSphere(int len)
	{
		Debug.Assert(len > 2);
		var v = new Vector4[len * 3];
		for (int i = 0; i < len; i++)
		{
			var f = i / (float)len;
			float c = Mathf.Cos(f * (float)(Mathf.PI * 2.0));
			float s = Mathf.Sin(f * (float)(Mathf.PI * 2.0));
			v[0 * len + i] = new Vector4(c, s, 0, 1);
			v[1 * len + i] = new Vector4(0, c, s, 1);
			v[2 * len + i] = new Vector4(s, 0, c, 1);
		}
		return v;
	}

	public static Vector3 ClosestPointOnLine(Vector3 lineStart, Vector3 lineEnd, Vector3 point)
	{
		var pointStart = point - lineStart;
		var dir = lineEnd - lineStart;
		var dist = dir.magnitude;
		dir = dir / dist;

		var dot = Vector3.Dot(dir, pointStart);

		if (dot <= 0)
			return lineStart;

		if (dot >= dist)
			return lineEnd;

		var offset = dir * dot;
		var ret = lineStart + offset;

		return ret;
	}

	public static float Remap(float value, float from1, float to1, float from2, float to2)
	{
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public static float Remap01(float value, float min, float max)
	{
		return (value - min) / (max - min);
	}
}
