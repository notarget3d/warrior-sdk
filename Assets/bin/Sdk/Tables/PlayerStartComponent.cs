using System;
using UnityEngine;
using UnityEditor;


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
			Quaternion rot = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
			Gizmos.color = Color.cyan;
			Gizmos.DrawRay(transform.position + Vector3.up, rot * Vector3.forward);
		}
	}
}
