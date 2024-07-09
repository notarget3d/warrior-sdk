using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_conveyor : BaseEntityTable
	{
		[Min(0.1f)]
		public float speed = 1.0f;
		public Vector3 dir = Vector3.forward;

		public int animMaterialId = -1;
		public float animUvScale = 1.0f;
	}

	[AddComponentMenu("Entities/" + nameof(func_conveyor))]
	internal sealed class FuncConveyorComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_conveyor table;

		public override BaseEntityTable GetEntitySpawnTable() => table;

#if UNITY_EDITOR


		private void OnDrawGizmos()
		{
			void DrawArrowGizmo(Vector3 offset)
			{
				DrawArrow(offset, Quaternion.LookRotation(table.dir), Color.yellow, 0.75f);
			}

			if (IsSelectedOrAny(GetChilds()) == false)
			{
				return;
			}

			Vector3 arrowPos = Vector3.up;

			if (Utils.IsSet(table.spawnFlags, (int)EditorGenericSpawnFlagsDrawer.func_conveyor.WorldSpace))
			{
				DrawArrowGizmo(transform.position + arrowPos);
			}
			else
			{
				Gizmos.matrix = transform.localToWorldMatrix;
				DrawArrowGizmo(arrowPos);
			}
		}

		private void OnValidate()
		{
			if (table is null)
			{
				return;
			}

			if (table.dir.magnitude <= float.Epsilon)
			{
				table.dir = Vector3.forward;
			}
		}
#endif
	}
}
