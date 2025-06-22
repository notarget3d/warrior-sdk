using System;
using UnityEngine;
using UnityEditor;


namespace WMSDK
{
	[Serializable]
	public sealed class info_player_deathmatch : BaseEntityTable
	{
	}

	[AddComponentMenu("Entities/" + nameof(info_player_deathmatch))]
	internal sealed class PlayerDeathmatchComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private info_player_deathmatch table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			Utils.DrawPlayerGizmo(transform.position, Color.orangeRed);
			Quaternion rot = Quaternion.Euler(0.0f, transform.eulerAngles.y, 0.0f);
			DrawArrow(transform.position + Vector3.up, rot, Color.darkRed, 0.7f);
		}
	}
}
