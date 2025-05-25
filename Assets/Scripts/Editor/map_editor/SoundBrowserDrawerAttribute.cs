#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


[System.Diagnostics.Conditional("UNITY_EDITOR")]
public class SoundBrowserDrawerAttribute : PropertyAttribute
{
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SoundBrowserDrawerAttribute))]
public class SoundBrowserDrawer : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
        SoundBrowserDrawerAttribute dropAttribute = attribute as SoundBrowserDrawerAttribute;

		EditorGUI.BeginChangeCheck();

        float labelWidth = EditorGUIUtility.labelWidth;
        float valueWidth = position.width - labelWidth;
        float width = labelWidth / 3.0f;
        valueWidth = valueWidth - width;

        Rect buttonRect = new Rect(position.x + labelWidth + valueWidth, position.y, width, position.height);
        Rect propertyRect = new Rect(position.x, position.y, position.width - width, position.height);

        EditorGUI.PropertyField(propertyRect, property, label, true);
        if (GUI.Button(buttonRect, "Browse"))
        {
            Debug.Log("Open sound browser");
            EditorSoundBrowser window = EditorWindow.GetWindow(typeof(EditorSoundBrowser)) as EditorSoundBrowser;
            window.SetEditorProperty(property);
        }

        EditorGUI.EndChangeCheck();
	}
}
#endif