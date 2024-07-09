using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class game_end : BaseEntityTable
	{
	}

	[AddComponentMenu("Entities/" + nameof(game_end))]
	internal sealed class GameEndComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private game_end table;

		public override BaseEntityTable GetEntitySpawnTable() => table;


		private void OnDrawGizmos()
		{
			DrawDefaultGizmo(nameof(game_end));
		}

		private void OnDrawGizmosSelected()
		{
			DrawDefaultGizmoSelectedWire();
		}
	}
}
