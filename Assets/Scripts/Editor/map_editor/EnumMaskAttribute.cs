#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using UnityEngine;


[System.Diagnostics.Conditional("UNITY_EDITOR")]
public class EnumMaskAttribute : PropertyAttribute
{
	public Type EnumType;
	public Enum Enum;


	public EnumMaskAttribute(Type enumType, int defaultValueIndex = 0)
	{
		this.EnumType = enumType;
		this.Enum = (Enum)Enum.GetValues(enumType).GetValue(defaultValueIndex);
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(EnumMaskAttribute))]
public class EnumMaskDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EnumMaskAttribute enumMaskAttribute = attribute as EnumMaskAttribute;

		EditorGUI.BeginChangeCheck();
		//enumMaskAttribute.Enum = EditorGUI.EnumFlagsField(position, label, enumMaskAttribute.Enum);
		Enum e = (Enum)Enum.ToObject(enumMaskAttribute.EnumType, property.intValue);
		enumMaskAttribute.Enum = EditorGUI.EnumFlagsField(position, label, e);

		if (EditorGUI.EndChangeCheck())
		{
			property.intValue = Convert.ToInt32(enumMaskAttribute.Enum);
		}
	}
}
#endif