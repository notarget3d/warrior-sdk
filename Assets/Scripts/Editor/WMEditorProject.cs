#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


[CreateAssetMenu(fileName = "WMEditorProject", menuName = "WMEditorProject")]
public sealed class WMEditorProject : ScriptableObject
{
	public string PackName;
	public SceneAsset[] scenes;
}

#endif