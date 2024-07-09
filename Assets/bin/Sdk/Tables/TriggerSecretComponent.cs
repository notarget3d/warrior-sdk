using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_secret : BaseEntityTable
	{
		public bool killOnFire;
	}

	[AddComponentMenu("Entities/" + nameof(trigger_secret))]
	internal sealed class TriggerSecretComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_secret table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
