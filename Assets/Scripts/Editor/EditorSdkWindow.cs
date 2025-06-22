#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using WMSDK;


public sealed class EditorSdkWindow : EditorWindow
{
	public const string GAME_EXECUTABLE = "WM.exe";
	public const string CONFIG_PATH = "Assets/Scripts/Editor/WMDataSettings.asset";

	private WMEditorSettings m_Settings;
	private Editor m_SettingsEdit;
	private string m_EntListString;

	private bool m_ScenesFold;
	private Vector2 m_ScrollPos;
	private Vector2 m_MainScrollPos;

	private readonly char dsc = Path.DirectorySeparatorChar;


	private void OnEnable()
	{
		InitEntityList();

		m_Settings = AssetDatabase.LoadAssetAtPath<WMEditorSettings>(CONFIG_PATH);

		if (m_Settings == null)
		{
			m_Settings = ScriptableObject.CreateInstance<WMEditorSettings>();
			AssetDatabase.CreateAsset(m_Settings, CONFIG_PATH);
		}

		m_SettingsEdit = Editor.CreateEditor(m_Settings);
	}

	private void OnGUI()
	{
		m_MainScrollPos = GUILayout.BeginScrollView(m_MainScrollPos);

		if (GUILayout.Button("Generate ent IDs", GUILayout.MaxWidth(120f)))
		{
			EntityTableUtils.UpdateAllEntityTables();
			InitEntityList();
		}

		if (GUILayout.Button("Build map", GUILayout.MaxWidth(120f)))
		{
			BuildContent(false);
		}

		if (GUILayout.Button("Build and Run", GUILayout.MaxWidth(120f)))
		{
			BuildContent(true);
		}

		m_ScenesFold = EditorGUILayout.Foldout(m_ScenesFold, "Scene Quick Access");

		if (m_ScenesFold && m_Settings != null)
		{
			for (int i = 0; i < m_Settings.scenes.Length; i++)
			{
				if (m_Settings.scenes[i] == null)
				{
					continue;
				}

				if (GUILayout.Button(m_Settings.scenes[i].name, GUILayout.MaxWidth(240f)))
				{
					LoadScene(m_Settings.scenes[i]);
				}
			}
		}

		if (m_SettingsEdit != null)
		{
			m_SettingsEdit.OnInspectorGUI();
		}

		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos, GUILayout.MinHeight(75.0f));
		GUILayout.Label(m_EntListString, GUILayout.MaxWidth(360f));
		GUILayout.EndScrollView();
		GUILayout.EndScrollView();
	}

	private void LoadScene(SceneAsset scene)
	{
		EditorSceneManager.OpenScene($"Assets/Scenes/{scene.name}.unity", OpenSceneMode.Single);
	}

	private void BuildContent(bool run = false)
	{
		Debug.Log("Building");
		EntityTableUtils.UpdateAllEntityTables();

		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false)
		{
			Debug.Log("Abort");
			return;
		}

		List<string> paths = new List<string>();

		foreach (var s in m_Settings.scenes)
		{
			if (s == null)
			{
				continue;
			}

			paths.Add(AssetDatabase.GetAssetPath(s));
		}

		if (EditorCreateAssets.BuildSceneAssets(paths.ToArray(), m_Settings.PackName) == false)
		{
			return;
		}

		if (run)
		{
			RunMap();
		}
	}

	private void RunMap()
	{
		string targetPath;
		string sourcePath;

		string gameExec = $"{m_Settings.GamePath}{dsc}{GAME_EXECUTABLE}";
		string mapsDir = $"{m_Settings.GamePath}{dsc}maps";

		// Check for executable
		if (!File.Exists(gameExec))
		{
			Debug.LogWarning($"Unable to find game executable at '{gameExec}'");
			return;
		}

		// Create folders to copy
		if (!Directory.Exists(mapsDir))
		{
			Directory.CreateDirectory(mapsDir);
		}

		targetPath = $"{mapsDir}{dsc}{m_Settings.PackName}{EditorCreateAssets.FILE_EXT_MAP}";

		// Copy bundles to game folder
		DirectoryInfo di = Directory.GetParent(Application.dataPath);

		sourcePath = di.FullName + dsc + EditorCreateAssets.BUILD_DIR + dsc +
			m_Settings.PackName + EditorCreateAssets.FILE_EXT_MAP;

		File.Copy(sourcePath, targetPath, true);

		string module = m_Settings.GamePath + dsc + GAME_EXECUTABLE;
		string cmdline = m_Settings.GameRunParams + " +map " + EditorSceneManager.GetActiveScene().name;

		Debug.Log($"Starting game {module} {cmdline}");

		System.Diagnostics.ProcessStartInfo inf = new System.Diagnostics.ProcessStartInfo(module, cmdline);
		inf.WorkingDirectory = m_Settings.GamePath;
		inf.UseShellExecute = true;
		inf.FileName = module;
		inf.Arguments = cmdline;
		System.Diagnostics.Process.Start(inf);
	}

	private void InitEntityList()
	{
		m_EntListString = "";
		BaseEntityTableComponent[] entities =
			FindObjectsByType<BaseEntityTableComponent>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);

		for (int i = 0; i < entities.Length; i++)
		{
			var table = entities[i].GetEntitySpawnTable();
			m_EntListString += $"{i + 1}) Entity: {table.classname} - '{table.targetname}' [{table.hammerId}]\n";
		}
	}

	[MenuItem("Sdk/Map settings")]
	public static void InitWindow()
	{
		EditorSdkWindow window = GetWindow<EditorSdkWindow>();
		window.minSize = new Vector2(120.0f, 320.0f);
	}
}

#endif