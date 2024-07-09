using System;
using UnityEngine;

using UnityEditor;


namespace WMSDK
{
	[Serializable]
	public sealed class point_text : BaseEntityTable
	{
		[TextArea(3, 5)]
		public string message;
		public float distance = 15.0f;
		[Tooltip("Spawn rotating question icon on the place")]
		public bool questionIcon = true;
		[Tooltip("Text offset from origin point on Y axis")]
		public float offset = 1.0f;
	}

	[AddComponentMenu("Entities/" + nameof(point_text))]
	internal sealed class PointTextComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private point_text table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			Vector3 p = transform.position;
			const float r = 0.3f;

			Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(p, r);
			Gizmos.color = new Color(0f, 0f, 0f, 0.01f);
			Gizmos.DrawSphere(p, r);

			p.y += table.offset;
			DrawText(table.message, Color.white, p);
		}
	}
}
