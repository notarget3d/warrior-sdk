using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_speedrun : BaseEntityTable
	{
		public string finishTrigger;
		public int score;
	}

	internal sealed class TriggerSpeedrunComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_speedrun table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
