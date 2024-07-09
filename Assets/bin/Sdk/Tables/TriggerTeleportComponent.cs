using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_teleport : BaseEntityTable
	{
		public Transform[] targets;
		public int currentTarget;
		public bool isVelocityReset;
		public bool isVelocitySetDir;
		public bool isTargetResetAfterUsed;
		public string sound;
		public string soundTarget;
	}

	[AddComponentMenu("Entities/" + nameof(trigger_teleport))]
	internal sealed class TriggerTeleportComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_teleport table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
