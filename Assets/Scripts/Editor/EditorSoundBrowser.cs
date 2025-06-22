#if UNITY_EDITOR

using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;


public sealed class EditorSoundBrowser : EditorWindow
{
	private abstract class SoundEntryBase
	{
		public abstract string name { get; }
		public abstract void Preview(AudioSource source);
	}

	private sealed class SoundEntryScript : SoundEntryBase
	{
		private readonly string m_name;
		private readonly WMSound m_sound;

		public override string name => m_name;


		public SoundEntryScript(string n, in WMSound snd)
		{
			m_name = n;
			m_sound = snd;
		}

		public override void Preview(AudioSource source)
		{
			m_sound.Play(source);
		}
	}

	private sealed class SoundEntryRaw : SoundEntryBase
	{
		private readonly string m_name;
		public override string name => m_name;


		public SoundEntryRaw(string n)
		{
			m_name = n;
		}

		public override void Preview(AudioSource source)
		{
			m_AudioSource.clip = WMSoundSystem.instance.GetAudioClip(m_name);
			m_AudioSource.volume = 1.0f;
			m_AudioSource.pitch = 1.0f;
			m_AudioSource.Play();
		}
	}

	private static AudioSource m_AudioSource;
	private WMEditorSettings m_Settings;
	private Vector2 m_ScrollPos;

	private int m_MinEnties = 45;
	private int m_PageNum = 0;
	private int m_PageMax = 0;
	private bool m_CopyBuffer;
	private bool m_RawView;
	private SoundEntryScript[] m_Entries = new SoundEntryScript[0];
	private SoundEntryRaw[] m_EntriesRaw = new SoundEntryRaw[0];
	private SerializedProperty m_Property = null;


	private void OnEnable()
	{
		if (m_Settings == null)
		{
			m_Settings = AssetDatabase.LoadAssetAtPath<WMEditorSettings>(EditorSdkWindow.CONFIG_PATH);

			if (WMSoundSystem.instance.isLoaded == false)
			{
				Refresh();
			}

			InitSoundEntries();
		}
	}

	private void OnDisable()
	{
		SetEditorProperty(null);
	}

	public void SetEditorProperty(SerializedProperty property)
	{
		m_Property = property;
	}

	private void UnloadAll()
	{
		WMSoundSystem.instance.UnloadAudioBundles();
	}

	private void Refresh()
	{
		UnloadAll();

		string path = Path.Combine(m_Settings.GamePath, "sound");
		WMSoundSystem.instance.LoadAudioBundles(path);

		if (m_AudioSource == null)
		{
			GameObject snd = new GameObject("1");
			snd.hideFlags = HideFlags.DontSaveInBuild | HideFlags.DontSaveInEditor | HideFlags.HideInHierarchy;

			m_AudioSource = snd.AddComponent<AudioSource>();
		}

		string scriptsPath = Path.Combine(m_Settings.GamePath, "scripts");

		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "default_sounds.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "game_player_sounds.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "item_sounds.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "misc_phys.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "npc_zombie.txt"));
		WMSoundSystem.instance.LoadSoundScript(Path.Combine(scriptsPath, "weapon_sounds.txt"));
	}

	private void InitSoundEntries()
	{
		var raw = WMSoundSystem.instance.GetSoundsRaw();
		var dict = WMSoundSystem.instance.GetSounds();
		m_Entries = new SoundEntryScript[dict.Count];
		m_EntriesRaw = new SoundEntryRaw[raw.Length];
		int i = 0;
		m_PageNum = 0;

		foreach (var kv in dict)
		{
			m_Entries[i] = new SoundEntryScript(kv.Key, kv.Value);
			i++;
		}

		for (int f = 0; f < raw.Length; f++)
		{
			m_EntriesRaw[f] = new SoundEntryRaw(raw[f]);
		}
	}

	private void OnGUI()
	{
		const float MAX_WIDTH = 320.0f;
		const float ARROW_BUTTONS_WIDTH = 64.0f;
		m_ScrollPos = GUILayout.BeginScrollView(m_ScrollPos);

		GUILayout.Space(16.0f);
		if (GUILayout.Button("Refresh", GUILayout.MaxWidth(MAX_WIDTH)))
		{
			Refresh();
			InitSoundEntries();
		}

		SoundEntryBase[] entriesBase;

		entriesBase = m_RawView == false ? m_Entries : m_EntriesRaw;

		if (entriesBase.Length > 0)
		{
			GUILayout.BeginHorizontal(GUILayout.MaxWidth(MAX_WIDTH));
			GUILayout.Label("Max entries:", GUILayout.MaxWidth(MAX_WIDTH / 4f));
			GUILayout.Space(4.0f);
			m_MinEnties = (int)GUILayout.HorizontalSlider(m_MinEnties, 1, entriesBase.Length, GUILayout.MaxWidth(MAX_WIDTH));
			GUILayout.EndHorizontal();

			if (entriesBase.Length > m_MinEnties)
			{
				m_PageMax = entriesBase.Length / m_MinEnties;
			}
			else
			{
				m_PageMax = 0;
			}

			if (m_PageNum > m_PageMax)
			{
				m_PageNum = m_PageMax;
			}

			m_CopyBuffer = GUILayout.Toggle(m_CopyBuffer, " Copy to system buffer", GUILayout.MaxWidth(MAX_WIDTH));
			m_RawView = GUILayout.Toggle(m_RawView, " Raw view", GUILayout.MaxWidth(MAX_WIDTH));

			GUILayout.Space(16.0f);

			if (GUILayout.Button("Stop", GUILayout.MaxWidth(MAX_WIDTH)))
			{
				m_AudioSource.Stop();
			}

			GUILayout.Label($"Total entries count: {m_Entries.Length}", GUILayout.MaxWidth(MAX_WIDTH));
			GUILayout.Space(16.0f);
		}
		else
		{
			m_MinEnties = 45;
			m_PageMax = 0;
		}

		SoundEntryBase[] entries;

		if (m_MinEnties >= entriesBase.Length)
		{
			entries = entriesBase;
		}
		else
		{
			entries = entriesBase.Skip(m_MinEnties * m_PageNum).Take(m_MinEnties).ToArray();

			GUILayout.BeginHorizontal(GUILayout.MaxWidth(MAX_WIDTH));

			if (GUILayout.Button("<", GUILayout.MaxWidth(ARROW_BUTTONS_WIDTH / 2)))
			{
				m_PageNum--;
				m_PageNum = Mathf.Max(m_PageNum, 0);
			}

			GUILayout.Label($"Page: {m_PageNum + 1}/{m_PageMax + 1} Shown: {entries.Length}",
				GUILayout.MaxWidth(MAX_WIDTH - ARROW_BUTTONS_WIDTH));

			if (GUILayout.Button(">", GUILayout.MaxWidth(ARROW_BUTTONS_WIDTH / 2)))
			{
				m_PageNum++;
				m_PageNum = Mathf.Min(m_PageNum, m_PageMax);
			}

			GUILayout.EndHorizontal();
		}

		GUILayout.Space(8.0f);

		for (int i = 0; i < entries.Length; i++)
		{
			string soundName = entries[i].name;

			if (GUILayout.Button(soundName, GUILayout.MaxWidth(MAX_WIDTH)))
			{
				entries[i].Preview(m_AudioSource);

				if (m_CopyBuffer == true)
				{
					EditorGUIUtility.systemCopyBuffer = soundName;
				}

				if (m_Property != null)
				{
					try
					{
						m_Property.stringValue = soundName;
						m_Property.serializedObject.ApplyModifiedProperties();
					}
					catch
					{
						// Incase of inspector closed or object is deselected
						m_Property = null;
					}
				}
			}
		}

		GUILayout.EndScrollView();
	}

	[UnityEditor.Callbacks.DidReloadScripts]
	private static void OnScriptsReloaded()
	{
		AssetBundle.UnloadAllAssetBundles(true);
		WMSoundSystem.instance.UnloadAudioBundles();
	}

	[MenuItem("Sdk/Sound browser")]
	public static void ShowWindow()
	{
		EditorSoundBrowser window = GetWindow<EditorSoundBrowser>(typeof(EditorSoundBrowser));
		window.SetEditorProperty(null);
	}
}

#endif