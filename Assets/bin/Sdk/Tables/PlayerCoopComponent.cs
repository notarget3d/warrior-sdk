using System;
using UnityEngine;

using UnityEditor;


namespace WMSDK
{
	[Serializable]
	public sealed class info_player_coop : BaseEntityTable
	{
	}

	[AddComponentMenu("Entities/" + nameof(info_player_coop))]
	internal sealed class PlayerCoopComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private info_player_coop table;

		public override BaseEntityTable GetEntitySpawnTable() => table;

		private void OnDrawGizmos()
		{
			Utils.DrawPlayerGizmo(transform.position, Color.cyan);
			Quaternion rot = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
			Gizmos.color = Color.green;
			Gizmos.DrawRay(transform.position + Vector3.up, rot * Vector3.forward);
		}
	}
}
