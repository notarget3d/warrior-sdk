using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_soundscape : BaseEntityTable
	{
        public string soundscapeReference;
	}

	[AddComponentMenu("Entities/" + nameof(trigger_soundscape))]
	internal sealed class TriggerSoundscapeComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_soundscape table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
