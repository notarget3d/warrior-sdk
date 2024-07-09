using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class info_killcounter : BaseEntityTable
	{
		public uint additionalEnemiesInKillCounter;
	}

	[AddComponentMenu("Entities/" + nameof(info_killcounter))]
	internal sealed class InfoKillCounterComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private info_killcounter table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo();
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
