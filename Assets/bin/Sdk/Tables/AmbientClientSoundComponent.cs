using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class ambient_clientsound : BaseEntityTable
	{
		[DropdownStringDrawer("Master", "Music", "Ambient", "Voice")]
		public string channel;
	}

	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu("Entities/" + nameof(ambient_clientsound))]
	internal sealed class AmbientClientSoundComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private ambient_clientsound table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			Gizmos.color = GIZMO_COLOR_DEFAULT;
			Gizmos.DrawWireCube(transform.position, GIZMO_SIZE);
			Gizmos.color = Color.clear;
			Gizmos.DrawCube(transform.position, GIZMO_SIZE);
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
