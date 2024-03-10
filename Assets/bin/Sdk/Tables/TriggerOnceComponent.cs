using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class trigger_once : BaseEntityTable
	{
		public bool killOnFire;
	}

	[AddComponentMenu("Entities/trigger_once")]
	internal sealed class TriggerOnceComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private trigger_once table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
