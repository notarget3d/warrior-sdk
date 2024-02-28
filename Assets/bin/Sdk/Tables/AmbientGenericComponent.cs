using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class ambient_generic : BaseEntityTable
	{
		[Min(0.0f)] public float distMin;
		[Min(0.0f)] public float distMax;
		[Range(0.0f, 1.0f)] public float volume;
		public string sound;
		public string channel;
	}

	internal sealed class AmbientGenericComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private ambient_generic table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(ambient_generic));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, table.distMin);
			Gizmos.DrawWireSphere(transform.position, table.distMax);
		}

		private void OnValidate()
		{
			table.distMax = Mathf.Clamp(table.distMax, table.distMin, float.MaxValue);
		}
	}
}
