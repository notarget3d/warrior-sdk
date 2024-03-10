#if UNITY_EDITOR
using UnityEditor;
using WMSDK;
#endif

using System;
using UnityEngine;


[System.Diagnostics.Conditional("UNITY_EDITOR")]
public class SpawnFlagsDrawerAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SpawnFlagsDrawerAttribute))]
public partial class EditorGenericSpawnFlagsDrawer : PropertyDrawer
{
	private static Type GetFlagsList(UnityEngine.Object obj) => obj switch
	{
		FuncBreakableComponent => typeof(func_breakable),
		ItemSpawnerComponent => typeof(point_item_spawner),
		AmbientGenericComponent => typeof(ambient_generic),
		NpcMakerComponent => typeof(npc_maker),
		_ => typeof(generic_entity)
	};

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginChangeCheck();
		Enum e = (Enum)Enum.ToObject(GetFlagsList(property.serializedObject.targetObject), property.intValue);
		Enum i = EditorGUI.EnumFlagsField(position, label, e);

		if (EditorGUI.EndChangeCheck())
		{
			property.intValue = Convert.ToInt32(i);
		}
	}
}

#endif