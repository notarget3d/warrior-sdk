using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class info_player_start : BaseEntityTable
	{
	}

	[AddComponentMenu("Entities/info_player_start")]
	internal sealed class PlayerStartComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private info_player_start table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			Utils.DrawPlayerGizmo(transform.position, Color.green);
		}
	}
}
