using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace WMSDK
{
    public static class EntityTableUtils
    {
#if UNITY_EDITOR
		[MenuItem("Sdk/update all entity tables")]
		public static void UpdateAllEntityTables()
		{
			var tables = GameObject.FindObjectsByType<BaseEntityTableComponent>(FindObjectsInactive.Include,
				FindObjectsSortMode.InstanceID);

			ushort editorId = 1;

			foreach (var t in tables)
			{
				t.GetEntitySpawnTable().hammerId = editorId;
				t.PreBuild();

				editorId++;
			}
		}
#endif
	}
}
