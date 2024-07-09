using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_breakable : BaseEntityTable
	{
		public int gibs;
		public int damageFilter;
		public float respawn = -1.0f;
		public string soundBreak;
		public GameObject customGibs;
	}

	[AddComponentMenu("Entities/" + nameof(func_breakable))]
	internal sealed class FuncBreakableComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_breakable table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
