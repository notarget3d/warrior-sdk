using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_button : BaseEntityTable
	{
		[Min(0.01f)] public float delay = 0.5f;
		public bool toggle;
		public string sound = "Button.PushClick1";
		public string soundLocked;

		[Tooltip("Object to animate")]
		public Transform animated;
		[Tooltip("Material to use when button is in pressed state")]
		public Material animPressedMaterial;
		[Tooltip("Material to use when button is in locked state")]
		public Material animLockedMaterial;
	}

	[AddComponentMenu("Entities/" + nameof(func_button))]
	internal sealed class FuncButtonComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_button table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
