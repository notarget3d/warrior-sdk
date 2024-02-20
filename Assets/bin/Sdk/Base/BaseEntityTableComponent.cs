using System;
using UnityEngine;


namespace WMSDK
{
    [Flags]
    public enum SpawnFlags : uint
    {
        START_DISABLED = 1u << 0,
        FLAG_01 = 1u << 1, FLAG_02 = 1u << 2, FLAG_03 = 1u << 3, FLAG_04 = 1u << 4, FLAG_05 = 1u << 5, FLAG_06 = 1u << 6,
        FLAG_07 = 1u << 7, FLAG_08 = 1u << 8, FLAG_09 = 1u << 9, FLAG_10 = 1u << 10, FLAG_11 = 1u << 11, FLAG_12 = 1u << 12,
        FLAG_13 = 1u << 13, FLAG_14 = 1u << 14, FLAG_15 = 1u << 15, FLAG_16 = 1u << 16, FLAG_17 = 1u << 17, FLAG_18 = 1u << 18,
        FLAG_19 = 1u << 19, FLAG_20 = 1u << 20, FLAG_21 = 1u << 21, FLAG_22 = 1u << 22, FLAG_23 = 1u << 23, FLAG_24 = 1u << 24,
        FLAG_25 = 1u << 25, FLAG_26 = 1u << 26, FLAG_27 = 1u << 27, FLAG_28 = 1u << 28, FLAG_29 = 1u << 29, FLAG_30 = 1u << 30,
        FLAG_31 = 1u << 31
    }

    [Serializable]
    public abstract class BaseEntityTable
    {
        public Vector3 position;
        public Vector3 angles;
        public string classname;
        public string targetname;
        public short maxHealth;
        public short health;
        public ushort hammerId;
        public ushort parentId;
		public SpawnFlags spawnFlags;

        public GameObject model;

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