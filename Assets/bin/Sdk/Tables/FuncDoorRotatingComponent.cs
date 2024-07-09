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

		public string soundOpen;
		public string soundOpened;
		public string soundClose;
		public string soundClosed;
		public string soundMoving;
	}

	[AddComponentMenu("Entities/" + nameof(func_door_rotating))]
	internal sealed class FuncDoorRotatingComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_door_rotating table;

		public override BaseEntityTable GetEntitySpawnTable() => table;
	}
}
