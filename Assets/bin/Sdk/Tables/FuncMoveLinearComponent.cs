using System;
using UnityEngine;


namespace WMSDK
{
	[Serializable]
	public sealed class func_movelinear : BaseEntityTable
	{
		[Min(0.0001f)] public float speed = 2.0f;
		[Min(0)] public short blockDamage;

		public Transform[] floors;

		public bool blockClientCamera;

		public string soundStart;
		public string soundStop;
		public string soundMoving;
	}

	[AddComponentMenu("Entities/func_movelinear")]
	internal sealed class FuncMoveLinearComponent : BaseEntityTableComponent
	{
		[SerializeField]
		private func_movelinear table;

		public override BaseEntityTable GetEntitySpawnTable() => table;

#if UNITY_EDITOR
		private void OnDrawGizmos()
		{
			Transform[] floors = table.floors;

			if (floors != null && floors.Length > 1)
			{
				Color color;
				bool selected = false;

				for (int i = 0; i < UnityEditor.Selection.gameObjects.Length; i++)
				{
					GameObject obj = UnityEditor.Selection.gameObjects[i];

					if (obj == gameObject)
					{
						selected = true;
						break;
					}

					for (int f = 0; f < floors.Length; f++)
					{
						if (floors[f].gameObject == obj)
						{
							selected = true;
							break;
						}
					}

					if (selected)
					{
						break;
					}
				}

				color = selected ? Color.yellow : new Color(0.5f, 0.5f, 0.5f, 0.4f);

				Gizmos.color = color;

				for (int i = 0; i < floors.Length - 1; i++)
				{
					Gizmos.DrawLine(floors[i].position, floors[i + 1].position);
				}

				for (int i = 0; i < floors.Length; i++)
				{
					Gizmos.DrawWireSphere(floors[i].position, 0.08f);
				}
			}
		}
#endif
	}
}
