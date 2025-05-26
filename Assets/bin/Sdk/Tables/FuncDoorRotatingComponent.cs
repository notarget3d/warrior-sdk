using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_door_rotating : BaseEntityTable
	{
		[Min(0.0f)] public float speed;
		[Min(0)] public short blockDamage;

		public Vector3 rotAxis;
		public float openAngle;

		public string linkedDoor;

		[SoundBrowserDrawer] public string soundOpen;
		[SoundBrowserDrawer] public string soundOpened;
		[SoundBrowserDrawer] public string soundClose;
		[SoundBrowserDrawer] public string soundClosed;
		[SoundBrowserDrawer] public string soundMoving;
	}

	[AddComponentMenu("Entities/" + nameof(func_door_rotating))]
	internal sealed class FuncDoorRotatingComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_door_rotating table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
