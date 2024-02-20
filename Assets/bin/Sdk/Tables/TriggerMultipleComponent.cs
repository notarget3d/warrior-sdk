using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_multiple : BaseEntityTable
	{
		public float wait = 1.0f;
	}

	internal sealed class TriggerMultipleComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_multiple table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
