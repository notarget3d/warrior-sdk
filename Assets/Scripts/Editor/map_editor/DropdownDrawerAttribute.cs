#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[System.Diagnostics.Conditional("UNITY_EDITOR")]
public class DropdownDrawerAttribute : PropertyAttribute
{
	public int offset;
	public string[] choices;


	public DropdownDrawerAttribute(int _offset, params string[] _choices)
	{
		offset = _offset;
		choices = _choices;
	}

	public DropdownDrawerAttribute(params string[] _choices)
	{
		offset = 0;
		choices = _choices;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(DropdownDrawerAttribute))]
public class DropdownDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		DropdownDrawerAttribute dropAttribute = attribute as DropdownDrawerAttribute;

		EditorGUI.BeginChangeCheck();
		int choiceIndex = EditorGUI.Popup(position, property.intValue - dropAttribute.offset, dropAttribute.choices);
		if (EditorGUI.EndChangeCheck())
		{
			property.intValue = choiceIndex + dropAttribute.offset;
		}
	}
}
#endif