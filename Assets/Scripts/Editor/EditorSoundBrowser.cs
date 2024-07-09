#if UNITY_EDITOR

using System.IO;
using UnityEngine;
using UnityEditor;


public sealed class EditorSoundBrowser : EditorWindow
{
	private readonly struct SoundEntry
	{
		public readonly string name;
		public readonly WMSound sound;


		public SoundEntry(string n, in WMSound snd)
		{
			name = n;
			sound = snd;
		}
	}

	private static AudioSource m_AudioSource;

	private static AssetBundle m_SoundBundle;
	private WMEditorSettings m_Settings;
	private Vector2 m_ScrollPos;

	private int m_MinEnties = 45;
	private SoundEntry[] m_Entries = new SoundEntry[0];


	private void OnEnable()
	{
		if (m_Settings == null)
		{
			m_Settings = AssetDatabase.LoadAssetAtPath<WMEditorSettings>(EditorSdkWindow.CONFIG_PATH);
			Refresh();
		}
	}

	private void UnloadAll()
	{
		AssetBundle.UnloadAllAssetBundles(true);
	}

	private void Refresh()
	{
		UnloadAll();

		if (m_SoundBundle == null)
		{
			string path = Path.Combine(m_Settings.GamePath, "sound", "sounds.wsd");
			m_SoundBundle = AssetBundle.LoadFromFile(path);
		}

		if (m_AudioSource == null)
		{
			GameObject snd = new GameObject("1");
			snd.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy;

			m_AudioSource = snd.AddComponent<AudioSource>();
		}

		string scriptsPath = Path.Combine(m_Settings.GamePath, "scripts");

		WMSoundSystem.instance.SetBundle(m_SoundBundle);
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "default_sounds.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "game_player_sounds.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "item_sounds.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "misc_phys.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "npc_zombie.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "weapon_sounds.txt"));

		var dict = WMSoundSystem.instance.GetSounds();
		m_Entries = new SoundEntry[dict.Count];
		int i = 0;

		foreach (var kv in dict)
		{
			m_Entries[i] = new SoundEntry(kv.Key, kv.Value);
			i++;
		}
	}

	private void OnGUI()
	{
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);

		GUILayout.Space(16.0f);
		if (GUILayout.Button("Refresh", GUILayout.MaxWidth(360f)))
		{
			Refresh();
		}

		if (m_Entries.Length > 0)
		{
			GUILayout.Label("Max enties:", GUILayout.MaxWidth(340f));
			GUILayout.Space(4.0f);
			m_MinEnties = (int)GUILayout.HorizontalSlider(m_MinEnties, 0, m_Entries.Length, GUILayout.MaxWidth(340f));
			GUILayout.Space(16.0f);

			if (GUILayout.Button("Stop", GUILayout.MaxWidth(320f)))
			{
				m_AudioSource.Stop();
			}

			GUILayout.Label("Sounds:", GUILayout.MaxWidth(360f));
			GUILayout.Space(16.0f);
		}

		for (int i = 0; i < Mathf.Min(m_MinEnties, m_Entries.Length); i++)
		{
			string soundName = m_Entries[i].name;

			if (GUILayout.Button(soundName, GUILayout.MaxWidth(320f)))
			{
				m_Entries[i].sound.Play(m_AudioSource);
				EditorGUIUtility.systemCopyBuffer = soundName;
			}
		}

		GUILayout.EndScrollView();
	}

	[MenuItem("Sdk/Sound browser")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(EditorSoundBrowser));
	}
}

#endif