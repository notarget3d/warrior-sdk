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
        public string[] outputs = new string[0];
    }

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
	}
}