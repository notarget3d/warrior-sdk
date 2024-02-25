using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class point_item_spawner : BaseEntityTable
	{
		public float respawnTime;
		public string spawnEntity;
	}

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
