using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class npc_maker : BaseEntityTable
	{
		[Min(0.1f)] public float respawnTime;
		[Min(0.0f)] public float spawnMinDist;
		[Min(0.0f)] public float spawnMaxDist;

		[Min(0)] public int spawnCount;

		[DropdownStringDrawer(EntitySpawnerListType.NPCS)]
		public string spawnEntity;
		[DropdownStringDrawer(EntitySpawnerListType.WEAPONS)]
		public string spawnWeapon;
	}

	[AddComponentMenu("Entities/npc_maker")]
	internal sealed class NpcMakerComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private npc_maker table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(npc_maker));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, table.spawnMinDist);
			Gizmos.DrawWireSphere(transform.position, table.spawnMaxDist);
		}

		private void OnValidate()
		{
			if (table == null)
			{
				return;
			}

			table.spawnMaxDist = Mathf.Clamp(table.spawnMaxDist, table.spawnMinDist, float.MaxValue);
		}
	}
}
