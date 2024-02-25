using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class point_hurt : BaseEntityTable
	{
		[Min(0.0f)] public float radius = 1.0f;
		[Min(0.1f)] public float delay = 1.0f;

		public short damageAmount;

		[EnumMask(typeof(DamageType), 1)]
		public int damageType = 1;
	}

	internal sealed class PointHurtComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private point_hurt table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(point_hurt));
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position, table.radius);

			DrawDefaultGizmoSelectedWire();
		}
	}
}
