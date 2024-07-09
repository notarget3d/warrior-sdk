using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class npc_zombie : BaseEntityTable
	{
		public npc_zombie()
		{
			health = 600;
		}
	}

	[AddComponentMenu("Entities/npc_zombie")]
	internal sealed class NpcZombieComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private npc_zombie table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			float r = 0.35f;
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position + new Vector3(0.0f, r, 0.0f), r);
		}
	}
}
