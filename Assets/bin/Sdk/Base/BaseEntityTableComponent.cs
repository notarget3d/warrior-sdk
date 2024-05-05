using System;
using UnityEngine;


namespace WMSDK
{
    [Serializable]
    public abstract class BaseEntityTable
    {
		[HideInInspector] public Vector3 position;
		[HideInInspector] public Vector3 angles;
        [ReadOnly] public string classname;
		[HideInInspector] public string targetname;
		[Min(0)] public short maxHealth;
		[Min(0)] public short health;
		[HideInInspector] public ushort hammerId;
		[HideInInspector] public ushort parentId;
		[SpawnFlagsDrawer] public int spawnFlags;

		[HideInInspector] public GameObject model;

        public GameObject CreateObject() => GameObject.Instantiate(model);

		[Tooltip("Syntax:  'InputName TargetName,OutputName,Param,Delay'")]
		[EntityIODrawer]
        public string[] outputs = new string[0];
    }

	[DisallowMultipleComponent]
	public abstract class BaseEntityTableComponent : MonoBehaviour
    {
        public abstract BaseEntityTable GetEntitySpawnTable();

#if UNITY_EDITOR
		public virtual void PreBuild()
		{
			var t = GetEntitySpawnTable();
			t.classname = t.GetType().Name;
			t.model = gameObject;
			t.targetname = gameObject.name;
			t.position = gameObject.transform.position;
			t.angles = gameObject.transform.eulerAngles;

			SetDirty();
		}

		public void SetDirty()
		{
			UnityEditor.EditorUtility.SetDirty(gameObject);
			UnityEditor.EditorUtility.SetDirty(this);
		}

		public Transform[] GetChilds() => gameObject.GetComponentsInChildren<Transform>();

		public bool IsSelectedOrAny(Transform[] transforms)
		{
			for (int i = 0; i < UnityEditor.Selection.gameObjects.Length; i++)
			{
				GameObject obj = UnityEditor.Selection.gameObjects[i];

				if (obj == gameObject)
				{
					return true;
				}

				for (int f = 0; f < transforms.Length; f++)
				{
					if (transforms[f].gameObject == obj)
					{
						return true;
					}
				}
			}

			return false;
		}

		public static Bounds GetColliderBoundingBox(Collider collider)
		{
			if (collider is BoxCollider box)
			{
				return new Bounds(box.center, box.size);
			}
			else if (collider is SphereCollider sphere)
			{
				float r2 = sphere.radius * 2.0f;
				return new Bounds(sphere.center, new Vector3(r2, r2, r2));
			}
			else if (collider is CapsuleCollider capsule)
			{
				float r2 = capsule.radius * 2.0f;
				float h = capsule.height;
				int dir = capsule.direction;

				Vector3 size = Vector3.zero;
				ReadOnlySpan<Vector3> dirs = stackalloc Vector3[3];

				for (int i = 0; i < 3; i++)
				{
					if (i == dir)
						size += dirs[i] * h;
					else
						size += dirs[i] * r2;
				}

				return new Bounds(capsule.center, size);
			}
			else if (collider is MeshCollider mesh)
			{
				return mesh.sharedMesh.bounds;
			}

			return new Bounds(Vector3.zero, Vector3.one * 0.01f);
		}

		public static Bounds GetObjectCollidersBounds(GameObject obj)
		{
			Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

			Collider[] parentColliders = obj.GetComponents<Collider>();

			for (int i = 0; i < parentColliders.Length; i++)
			{
				bounds.Encapsulate(GetColliderBoundingBox(parentColliders[i]));
			}

			for (int i = 0; i < obj.transform.childCount; i++)
			{
				Transform tc = obj.transform.GetChild(i);
				Collider[] childs = tc.GetComponents<Collider>();

				for (int f = 0; f < childs.Length; f++)
				{
					Bounds b = GetColliderBoundingBox(childs[f]);
					b.center += tc.localPosition;
					bounds.Encapsulate(b);
				}
			}

			return bounds;
		}
#endif

		private static readonly Vector3 GIZMO_SIZE = new Vector3(0.4f, 0.4f, 0.4f);
		private static readonly Color GIZMO_COLOR_SELECTED = new Color(0.8f, 0.01f, 0.01f, 1.0f);
		private static readonly Color GIZMO_COLOR_DEFAULT = new Color(0.65f, 0.65f, 0.65f, 1.0f);

		protected void DrawDefaultGizmo()
		{
			Gizmos.color = GIZMO_COLOR_DEFAULT;
			Gizmos.DrawCube(transform.position, GIZMO_SIZE);
		}

		protected void DrawDefaultGizmoSelected()
		{
			Gizmos.color = GIZMO_COLOR_SELECTED;
			Gizmos.DrawCube(transform.position, GIZMO_SIZE);
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(transform.position, GIZMO_SIZE);
		}

		protected void DrawDefaultGizmo(string icon)
		{
			Gizmos.DrawIcon(transform.position, icon);
		}

		protected void DrawDefaultGizmoSelectedWire()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireCube(transform.position, GIZMO_SIZE);
		}
	}
}