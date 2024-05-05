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
	private static Type GetFlagsList(UnityEngine.Object obj)
	{
		var fieldFlags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
		var info = obj.GetType().GetField("table", fieldFlags);

		if (info != null)
		{
			var thisType = typeof(EditorGenericSpawnFlagsDrawer);

			var decl = thisType.GetMembers(System.Reflection.BindingFlags.Public |
				System.Reflection.BindingFlags.DeclaredOnly);

			for (int i = 0; i < decl.Length; i++)
			{
				var declEnum = decl[i];

				if (declEnum is Type e && e.IsEnum)
				{
					if (e.Name == info.FieldType.Name)
					{
						return e;
					}
				}
			}
		}

		return typeof(generic_entity);
	}

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