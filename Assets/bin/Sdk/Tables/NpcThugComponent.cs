using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class npc_thug : BaseNpcSoldierTable
	{
		public npc_thug()
		{
			health = 910;
		}
	}

	[AddComponentMenu("Entities/npc_thug")]
	internal sealed class NpcThugComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private npc_thug table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			float r = 0.35f;
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(transform.position + new Vector3(0.0f, r, 0.0f), r);
		}
	}
}
