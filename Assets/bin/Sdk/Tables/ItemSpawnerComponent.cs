using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class point_item_spawner : BaseEntityTable
	{
		[Min(0.1f)] public float respawnTime = 20.0f;

		[DropdownStringDrawer(EntitySpawnerListType.WEAPONS | EntitySpawnerListType.ITEMS)]
		public string spawnEntity;
	}

	[AddComponentMenu("Entities/" + nameof(point_item_spawner))]
	internal sealed class ItemSpawnerComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private point_item_spawner table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo();
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelected();
		}
	}
}
