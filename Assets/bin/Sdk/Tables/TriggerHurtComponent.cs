using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_hurt : BaseEntityTable
	{
		public short damageAmount;
		public int damageType;
	}

	internal sealed class TriggerHurtComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_hurt table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
