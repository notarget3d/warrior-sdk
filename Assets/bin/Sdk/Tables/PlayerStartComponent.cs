using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class info_player_start : BaseEntityTable
	{
	}

	internal sealed class PlayerStartComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private info_player_start table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
