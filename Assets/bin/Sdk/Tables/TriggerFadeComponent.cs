using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_fade : BaseEntityTable
	{
		public GameObject propFadeRoot;
	}

	[AddComponentMenu("Entities/" + nameof(trigger_fade))]
	internal sealed class TriggerFadeComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_fade table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
