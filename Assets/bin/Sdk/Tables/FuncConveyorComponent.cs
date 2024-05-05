using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_conveyor : BaseEntityTable
	{
		public float speed;
		public Vector3 dir = Vector3.forward;

		public int animMaterialId = -1;
		public float animUvScale = 1.0f;
	}

	internal sealed class FuncConveyorComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_conveyor table;

		public override BaseEntityTable GetEntitySpawnTable() => table;

#if UNITY_EDITOR


		private void OnDrawGizmos()
		{
			void DrawArrow(Vector3 offset)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawLine(offset, offset + table.dir);
			}

			if (IsSelectedOrAny(GetChilds()) == false)
			{
				return;
			}

			Vector3 arrowPos = Vector3.up;

			if (Utils.IsSet(table.spawnFlags, (int)EditorGenericSpawnFlagsDrawer.func_conveyor.WorldSpace))
			{
				DrawArrow(transform.position + arrowPos);
			}
			else
			{
				Gizmos.matrix = transform.localToWorldMatrix;
				DrawArrow(arrowPos);
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
