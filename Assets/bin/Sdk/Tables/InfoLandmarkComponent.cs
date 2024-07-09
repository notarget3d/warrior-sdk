using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class info_landmark : BaseEntityTable
	{
		public string[] spawnPoints;
	}

	[AddComponentMenu("Entities/" + nameof(info_landmark))]
	internal sealed class InfoLandmarkComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private info_landmark table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(info_landmark));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
