using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_door : BaseEntityTable
	{
		[Min(0.001f)] public float speed = 4.0f;
		public short blockDamage;

		[Tooltip("Should door be opened from start")]
		public bool startOpen;
		public bool blockClientCamera;
		[Tooltip("Door move direction")]
		public Vector3 moveDir = Vector3.forward;

		[Tooltip("Negative values will make the door move that many more than its length")]
		public float lip;
		[Tooltip("Time until the door returns to the closed position")]
		[Min(0.001f)] public float wait = float.PositiveInfinity;
		[Tooltip("This door will open with linked door")]
		public string linkedDoor;

		[SoundBrowserDrawer] public string soundLocked;
		[SoundBrowserDrawer] public string soundUnlocked;
		[SoundBrowserDrawer] public string soundOpen;
		[SoundBrowserDrawer] public string soundOpened;
		[SoundBrowserDrawer] public string soundClose;
		[SoundBrowserDrawer] public string soundClosed;
		[SoundBrowserDrawer] public string soundMoving;
	}

	[AddComponentMenu("Entities/" + nameof(func_door))]
	internal sealed class FuncDoorComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_door table;

		public override BaseEntityTable GetEntitySpawnTable() => table;

#if UNITY_EDITOR
		private Bounds m_Bounds = new Bounds();


		private void OnDrawGizmos()
		{
			if (IsSelectedOrAny(GetChilds()) == false)
			{
				return;
			}

			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(m_Bounds.center, m_Bounds.size);

			DrawArrow(m_Bounds.center, Quaternion.LookRotation(table.moveDir), Color.cyan, 1.0f);
		}

		private void OnValidate()
		{
			if (table is null)
			{
				return;
			}

			m_Bounds = GetObjectCollidersBounds(gameObject);

			if (table.moveDir.magnitude <= float.Epsilon)
			{
				table.moveDir = Vector3.forward;
			}
		}
#endif
	}
}
