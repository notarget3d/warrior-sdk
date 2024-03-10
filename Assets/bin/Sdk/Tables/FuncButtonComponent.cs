using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_button : BaseEntityTable
	{
		[Min(0.01f)] public float delay;
		public bool toggle;
		public string sound;
		public string soundLocked;
	}

	[AddComponentMenu("Entities/func_button")]
	internal sealed class FuncButtonComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_button table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
