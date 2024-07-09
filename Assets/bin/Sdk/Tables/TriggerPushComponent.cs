using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_push : BaseEntityTable
	{
		public Vector3 force;
	}

	[AddComponentMenu("Entities/" + nameof(trigger_push))]
	internal sealed class TriggerPushComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_push table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
