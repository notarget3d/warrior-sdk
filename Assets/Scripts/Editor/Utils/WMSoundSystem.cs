#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public sealed class WMSoundSystem
{
	private const string SOUND_PACK_EXT = ".wsd";
	public static WMSoundSystem instance { get; private set; }

	public bool isLoaded { get; private set; }
	private readonly List<AssetBundle> m_LoadedBoundles = new List<AssetBundle>();
	private readonly Dictionary<string, AudioClip> m_Clips = new Dictionary<string, AudioClip>();
	private readonly Dictionary<string, WMSound> m_Sounds = new Dictionary<string, WMSound>();


	public Dictionary<string, WMSound> GetSounds()
	{
		return m_Sounds;
	}

	public string[] GetSoundsRaw()
	{
		List<string> ret = new List<string>(200);

		for (int i = 0; i < m_LoadedBoundles.Count; i++)
		{
			ret.AddRange(m_LoadedBoundles[i].GetAllAssetNames());
		}

		return ret.ToArray();
	}

	public void LoadAudioBundles(string path)
	{
		string[] files = Directory.GetFiles(path);

		for (int i = 0; i < files.Length; i++)
		{
			string file = files[i];
			if (Path.GetExtension(file) == SOUND_PACK_EXT)
			{
				AssetBundle bundle = AssetBundle.LoadFromFile(file);

				if (bundle != null)
				{
					m_LoadedBoundles.Add(bundle);
				}
			}
		}

		isLoaded = m_LoadedBoundles.Count > 0;

		GC.Collect();
	}

	public void UnloadAudioBundles()
	{
		for (int i = 0; i < m_LoadedBoundles.Count; i++)
		{
			m_LoadedBoundles[i].Unload(true);
		}

		m_LoadedBoundles.Clear();
		m_Clips.Clear();
		m_Sounds.Clear();
		isLoaded = false;
	}

	public void LoadSoundScript(string scriptName)
	{
		if (!File.Exists(scriptName))
		{
			Debug.LogError($"Error! Failed to load sound script '{scriptName}' File not found.");
			return;
		}

		KeyValueNode root = KeyValueParser.ParseFile(scriptName);

		foreach (var entry in root.values)
		{
			string key = entry.key;
			string channel = "";
			List<string> wave = new List<string>();
			int soundLevel = 75;
			float pmin = 1.0f, vmin = 1.0f, pmax = 1.0f, vmax = 1.0f;

			foreach (var param in entry.values)
			{
				string p = param.key;

				switch (p)
				{
					case "channel":
						channel = param.value;
						break;
					case "soundlevel":
						soundLevel = ParseSoundLevel(param.value);
						break;
					case "pitch":
						float min, max;
						ParseSoundParam(param.value, out min, out max);

						pmin = min * 0.01f;
						pmax = max * 0.01f;
						break;
					case "volume":
						ParseSoundParam(param.value, out vmin, out vmax);
						break;
					case "wave":
						wave.Add(param.value);
						break;
					case "rndwave":
						ParseRandomWave(wave, param.values);
						break;
				}
			}

			AudioClip[] clips = new AudioClip[wave.Count];

			for (int i = 0; i < wave.Count; i++)
			{
				clips[i] = GetAudioClip(wave[i]);
			}

			if (!m_Sounds.ContainsKey(key))
			{
				m_Sounds.Add(key, new WMSound(pmin, pmax, vmin, vmax, clips));
			}
			else
			{
				m_Sounds[key] = new WMSound(pmin, pmax, vmin, vmax, clips);
			}
		}
	}

	public bool TryGetSound(string name, out WMSound sound)
	{
		if (m_Sounds.TryGetValue(name, out sound))
		{
			return true;
		}

		AudioClip clip = GetAudioClip(name);

		sound = new WMSound(clip);
		return true;
	}

	public WMSound GetSound(string name)
	{
		if (m_Sounds.TryGetValue(name, out WMSound sound))
		{
			return sound;
		}

		AudioClip clip = GetAudioClip(name);

		return new WMSound(clip);
	}

	public AudioClip GetAudioClip(string name)
	{
		AudioClip clip;

		if (m_Clips.TryGetValue(name, out clip))
		{
			return clip;
		}

		for (int i = 0; i < m_LoadedBoundles.Count; i++)
		{
			if (m_LoadedBoundles[i].Contains(name))
			{
				clip = m_LoadedBoundles[i].LoadAsset<AudioClip>(name);

				if (clip != null)
				{
					m_Clips.Add(name, clip);
					return clip;
				}
			}
		}

		Debug.LogWarning($"C_SoundSystem::GetAudioClip() clip: '{name}' not found");
		return null;
	}

	private static void ParseRandomWave(List<string> wave, List<KeyValueNode> param)
	{
		foreach (var p in param)
		{
			if (p.key == "wave")
			{
				wave.Add(p.value);
			}
			else
			{
				Debug.LogWarning($"C_SoundSystem::ParseRandomWave() Unknown key '{p.key}'");
			}
		}
	}

	private static int ParseSoundLevel(string level)
	{
		if (level.Contains("SNDLVL"))
		{
			return level switch
			{
				"SNDLVL_NONE" => 0,
				"SNDLVL_25dB" => 25,
				"SNDLVL_30dB" => 30,
				"SNDLVL_35dB" => 35,
				"SNDLVL_40dB" => 40,
				"SNDLVL_45dB" => 45,
				"SNDLVL_50dB" => 50,
				"SNDLVL_55dB" => 55,
				"SNDLVL_IDLE" => 60,
				"SNDLVL_TALKING" => 60,
				"SNDLVL_60dB" => 60,
				"SNDLVL_65dB" => 65,
				"SNDLVL_STATIC" => 66,
				"SNDLVL_70dB" => 70,
				"SNDLVL_NORM" => 75,
				"SNDLVL_75dB" => 75,
				"SNDLVL_80dB" => 80,
				"SNDLVL_85dB" => 85,
				"SNDLVL_90dB" => 90,
				"SNDLVL_95dB" => 95,
				"SNDLVL_100dB" => 100,
				"SNDLVL_105dB" => 105,
				"SNDLVL_120dB" => 120,
				"SNDLVL_130dB" => 130,
				"SNDLVL_GUNFIRE" => 140,
				"SNDLVL_140dB" => 140,
				"SNDLVL_150dB" => 150,
				_ => 0
			};
		}
		else
		{
			return int.Parse(level);
		}
	}

	private static void ParseSoundParam(ReadOnlySpan<char> vol, out float min, out float max)
	{
		if (vol.CompareTo("PITCH_NORM", StringComparison.InvariantCultureIgnoreCase) == 0)
		{
			min = max = 100.0f;
			return;
		}

		int idx = vol.IndexOf(',');

		if (idx != -1)
		{
			min = Utils.StringToFloat(vol.Slice(0, idx));
			max = Utils.StringToFloat(vol.Slice(idx + 1));
		}
		else
		{
			min = Utils.StringToFloat(vol);
			max = min;
		}
	}

	static WMSoundSystem()
	{
		instance = new WMSoundSystem();
	}

	private WMSoundSystem()
	{
		//m_Sounds.Add("", WMSound.Null);
	}
}

#endif