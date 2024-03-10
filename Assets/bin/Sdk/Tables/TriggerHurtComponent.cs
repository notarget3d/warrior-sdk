using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_hurt : BaseEntityTable
	{
		public short damageAmount;
		[EnumMask(typeof(DamageType), 1)]
		public int damageType = 1;
	}

	[AddComponentMenu("Entities/trigger_hurt")]
	internal sealed class TriggerHurtComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_hurt table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
