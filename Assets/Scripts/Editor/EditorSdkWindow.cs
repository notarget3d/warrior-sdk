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

	private bool m_ManualTextureBuild => m_Settings.UseSharedTextures;
	private bool m_ScenesFold;
	private Vector2 m_ScrollPos;
	private Vector2 m_MainScrollPos;


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
			EditorApplication.delayCall += () => BuildContent(false, m_ManualTextureBuild);
		}

		if (GUILayout.Button("Build and Run", GUILayout.MaxWidth(120f)))
		{
			EditorApplication.delayCall += () => BuildContent(true, m_ManualTextureBuild);
		}

		if (GUILayout.Button("Build sound assets", GUILayout.MaxWidth(120f)))
		{
			EditorApplication.delayCall += () => BuildSoundAssets();
		}

		m_Settings.UseSharedTextures = GUILayout.Toggle(m_ManualTextureBuild,
			"Shared texture manifest (set if you use multiple projects)");

		if (m_ManualTextureBuild)
		{
			if (GUILayout.Button("Build textures", GUILayout.MaxWidth(120f)))
			{
				EditorApplication.delayCall += () => BuildTextureAssets();
			}
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

	private void BuildTextureAssets()
	{
		if (string.IsNullOrEmpty(m_Settings.SharedTexturesName))
		{
			Debug.Log("Provide shared textures package name");
			return;
		}

		AssetBuildScript.BuildSceneTexturesAssets(m_Settings.SharedTexturesName,
			AssetBuildScript.SCENE_TEXTURES_MANIFEST_SHARED_PATH, false);
	}

	private void BuildSoundAssets()
	{
		if (m_Settings == null || string.IsNullOrEmpty(m_Settings.SoundsPath) == true)
		{
			return;
		}

		string fullPath = Path.Combine(Application.dataPath, m_Settings.SoundsPath);

		if (Directory.Exists(fullPath) == false)
		{
			Debug.Log($"Path: {m_Settings.SoundsPath} not found");
			return;
		}

		string soundPackName = m_Settings.PackName + "_sound";

		WMSoundSystem.instance.UnloadAudioBundles();
		EditorCreateAssets.BuildSoundAssetCustom("Assets/" + m_Settings.SoundsPath, soundPackName, true);

		soundPackName = soundPackName + EditorCreateAssets.FILE_EXT_SOUND;

		// Copy bundles to game folder
		DirectoryInfo di = Directory.GetParent(Application.dataPath);

		string sourcePath = Path.Combine(di.FullName, EditorCreateAssets.BUILD_DIR, soundPackName);
		string gameSoundPath = Path.Combine(m_Settings.GamePath, "sound", soundPackName);

		if (!File.Exists(gameSoundPath) || !File.Exists(sourcePath))
		{
			return;
		}

		File.Copy(sourcePath, gameSoundPath, true);
	}

	private bool BuildContent(bool run, bool manualTextures)
	{
		Debug.Log("Building");

		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false)
		{
			Debug.Log("Abort");
			return false;
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

		WMSoundSystem.instance.UnloadAudioBundles();

		if (!manualTextures)
		{
			if (File.Exists(AssetBuildScript.SCENE_TEXTURES_MANIFEST_PATH))
			{
				File.Delete(AssetBuildScript.SCENE_TEXTURES_MANIFEST_PATH);
			}
		}

		if (AssetBuildScript.BuildSceneAssets(paths.ToArray(), m_Settings.PackName) == false)
		{
			return false;
		}

		if (!manualTextures)
		{
			AssetBuildScript.BuildSceneTexturesAssets(m_Settings.PackName,
				AssetBuildScript.SCENE_TEXTURES_MANIFEST_PATH, true);
		}

		if (run)
		{
			RunMap(manualTextures);
		}

		return true;
	}

	private void RunMap(bool manualTextures)
	{
		string mapFileName = $"{m_Settings.PackName}{AssetBuildScript.FILE_EXT_MAP}";
		CopyRuntimeAsset(mapFileName, "maps/" + mapFileName);

		if (manualTextures == false)
		{
			string texturesFileName = $"{m_Settings.PackName}{AssetBuildScript.FILE_PACK_EXT}";
			CopyRuntimeAsset(texturesFileName, "textures/" + texturesFileName);
		}

		string module = PathCombine(m_Settings.GamePath, GAME_EXECUTABLE);
		string cmdline = m_Settings.GameRunParams + " +map " + EditorSceneManager.GetActiveScene().name;

		Debug.Log($"Starting game {module} {cmdline}");

		System.Diagnostics.ProcessStartInfo inf = new System.Diagnostics.ProcessStartInfo(module, cmdline);
		inf.WorkingDirectory = m_Settings.GamePath;
		inf.UseShellExecute = true;
		inf.FileName = module;
		inf.Arguments = cmdline;
		System.Diagnostics.Process.Start(inf);
	}

	private void CopyRuntimeAsset(string src, string dst)
	{
		DirectoryInfo di = Directory.GetParent(Application.dataPath);
		string gameExec = PathCombine(m_Settings.GamePath, GAME_EXECUTABLE);
		string srcBasePath = PathCombine(di.FullName, AssetBuildScript.BUILD_DIR);

		string sourcePath = PathCombine(srcBasePath, src);
		string destinationPath = PathCombine(m_Settings.GamePath, dst);

		// Check for executable
		if (!File.Exists(gameExec))
		{
			Debug.LogWarning($"Unable to find game executable at '{gameExec}'");
			return;
		}

		File.Copy(sourcePath, destinationPath, true);
	}

	private static string PathCombine(params string[] paths)
	{
		return Path.Combine(paths).Replace('\\', '/');
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