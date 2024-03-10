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