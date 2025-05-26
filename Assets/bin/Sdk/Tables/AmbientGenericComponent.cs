using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class ambient_generic : BaseEntityTable
	{
		[Min(0.0f)] public float distMin = 8.0f;
		[Min(0.0f)] public float distMax = 24.0f;
		[Range(0.0f, 1.0f)] public float volume = 1.0f;
		[SoundBrowserDrawer] public string sound;

		[DropdownStringDrawer("Master", "Music", "Ambient", "Voice")]
		public string channel;
	}

	[AddComponentMenu("Entities/" + nameof(ambient_generic))]
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
			if (table == null)
			{
				return;
			}

			table.distMax = Mathf.Clamp(table.distMax, table.distMin, float.MaxValue);
		}
	}
}
