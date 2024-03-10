#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Diagnostics.Conditional("UNITY_EDITOR")]
public class DropdownStringDrawerAttribute : PropertyAttribute
{
	public readonly string[] options;


	public DropdownStringDrawerAttribute(params string[] ch)
	{
		options = ch;
	}

	public DropdownStringDrawerAttribute(EntitySpawnerListType t)
	{
		List<string> list = new List<string>();

		if (CheckFlag(t, EntitySpawnerListType.WEAPONS))
		{
			list.AddRange(EntitySpawnerList.WEAPONS);
		}

		if (CheckFlag(t, EntitySpawnerListType.ITEMS))
		{
			list.AddRange(EntitySpawnerList.ITEMS);
		}

		if (CheckFlag(t, EntitySpawnerListType.NPCS))
		{
			list.AddRange(EntitySpawnerList.NPCS);
		}

		options = list.Distinct().ToArray();
	}

	private static bool CheckFlag(EntitySpawnerListType t, EntitySpawnerListType f)
	{
		return Utils.IsSet((int)t, (int)f);
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DropdownStringDrawerAttribute))]
public class DropdownStringDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		DropdownStringDrawerAttribute dropAttribute = attribute as DropdownStringDrawerAttribute;

		EditorGUI.BeginChangeCheck();

		int choiceIndex = EditorGUI.Popup(position, property.displayName,
			GetCurrentIndex(dropAttribute.options, property.stringValue), dropAttribute.options);

		if (EditorGUI.EndChangeCheck())
		{
			property.stringValue = GetChoice(dropAttribute.options, choiceIndex);
		}
	}

	private string GetChoice(string[] choices, int choise)
	{
		return (choise > 0 && choise < choices.Length) ? choices[choise] : choices[0];
	}

	private int GetCurrentIndex(string[] choices, string choise)
	{
		int idx = System.Array.FindIndex(choices, x => x == choise);

		return idx == -1 ? 0 : idx;
	}
}
#endif