using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class info_changelevel : BaseEntityTable
	{
		public string landMark;
		public string[] possibleTransitions;
		public string[] openedTransitions;
	}

	[AddComponentMenu("Entities/" + nameof(info_changelevel))]
	internal sealed class InfoChangeLevelComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private info_changelevel table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(info_changelevel));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
