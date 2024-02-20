#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;


namespace WMSDK
{
	[CreateAssetMenu(fileName = "WMDataSettings", menuName = "WMDataSettings")]
	public sealed class WMEditorSettings : ScriptableObject
	{
		public WMEditorProject currentProject;

		public string PackName => currentProject.PackName;
		public string GamePath;
		public string GameRunParams = "-console -screen-fullscreen 0 -screen-width 800 -screen-height 600 +sv_cheats 1";

		public SceneAsset[] scenes => currentProject.scenes;
	}
}

#endif