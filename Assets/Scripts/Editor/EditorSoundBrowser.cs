#if UNITY_EDITOR

using System.IO;
using System.Linq;
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
	private int m_PageNum = 0;
	private int m_PageMax = 0;
    private bool m_CopyBuffer;
	private SoundEntry[] m_Entries = new SoundEntry[0];
    private SerializedProperty m_Property;


    private void OnEnable()
	{
		if (m_Settings == null)
		{
			m_Settings = AssetDatabase.LoadAssetAtPath<WMEditorSettings>(EditorSdkWindow.CONFIG_PATH);
			Refresh();
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
        m_PageNum = 0;

        foreach (var kv in dict)
		{
			m_Entries[i] = new SoundEntry(kv.Key, kv.Value);
			i++;
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
		}

		if (m_Entries.Length > 0)
		{
            GUILayout.BeginHorizontal(GUILayout.MaxWidth(MAX_WIDTH));
            GUILayout.Label("Max entries:", GUILayout.MaxWidth(MAX_WIDTH / 4f));
			GUILayout.Space(4.0f);
			m_MinEnties = (int)GUILayout.HorizontalSlider(m_MinEnties, 1, m_Entries.Length, GUILayout.MaxWidth(MAX_WIDTH));
            GUILayout.EndHorizontal();

            if (m_Entries.Length > m_MinEnties)
            {
                m_PageMax = m_Entries.Length / m_MinEnties;
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

        SoundEntry[] entries;

        if (m_MinEnties >= m_Entries.Length)
        {
            entries = m_Entries;
        }
        else
        {
            entries = m_Entries.Skip(m_MinEnties * m_PageNum).Take(m_MinEnties).ToArray();

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
                entries[i].sound.Play(m_AudioSource);

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

	[MenuItem("Sdk/Sound browser")]
	public static void ShowWindow()
	{
        EditorSoundBrowser window = EditorWindow.GetWindow(typeof(EditorSoundBrowser)) as EditorSoundBrowser;
        window.SetEditorProperty(null);
	}
}

#endif