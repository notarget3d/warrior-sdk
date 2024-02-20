using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_button : BaseEntityTable
	{
		public float delay;
		public bool toggle;
		public string sound;
		public string soundLocked;
	}

	internal sealed class FuncButtonComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_button table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
