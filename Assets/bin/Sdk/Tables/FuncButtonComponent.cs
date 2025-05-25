using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_button : BaseEntityTable
	{
        [Tooltip("Amount of time, in seconds, after the button has been pressed before it returns to the starting position.\n" +
            "Once it has returned, it can be used again. If the value is set to 'Infinity', the button never returns.")]
        [Min(0.01f)] public float delay = 0.5f;
		public bool toggle;
        [SoundBrowserDrawer] public string sound = "Button.PushClick1";
		[SoundBrowserDrawer] public string soundLocked;

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
