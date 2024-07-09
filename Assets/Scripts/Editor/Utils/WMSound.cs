using UnityEngine;


public readonly struct WMSound
{
	private readonly float m_Pitch;
	private readonly float m_Pitch_max;
	private readonly float m_Volume;
	private readonly float m_Volume_max;

	private readonly AudioClip[] m_Audio;

	public float pitch => Random.Range(m_Pitch, m_Pitch_max);
	public float volume => Random.Range(m_Volume, m_Volume_max);
	public AudioClip clip => m_Audio[Random.Range(0, m_Audio.Length)];


	public void Play(AudioSource src)
	{
		src.clip = clip;
		src.volume = volume;
		src.pitch = pitch;
		src.Play();
	}

	public void Play(AudioSource src, float _volume)
	{
		src.clip = clip;
		src.volume = _volume;
		src.pitch = pitch;
		src.Play();
	}

	public WMSound(float pitch, float volume, AudioClip[] clips)
	{
		m_Pitch = pitch;
		m_Pitch_max = pitch;
		m_Volume = volume;
		m_Volume_max = volume;
		m_Audio = clips;
	}

	public WMSound(float pitch, float pitchMax, float volume, float volumeMax, AudioClip[] clips)
	{
		m_Pitch = pitch;
		m_Pitch_max = pitchMax;
		m_Volume = volume;
		m_Volume_max = volumeMax;
		m_Audio = clips;
	}

	public WMSound(AudioClip clips)
	{
		m_Pitch = 1.0f;
		m_Pitch_max = 1.0f;
		m_Volume = 1.0f;
		m_Volume_max = 1.0f;
		m_Audio = new AudioClip[] { clips };
	}

	public readonly static WMSound Null = new WMSound(1f, 1f, new AudioClip[1] { null });

	public override string ToString()
	{
		var sb = new System.Text.StringBuilder(128);

		sb.Append("volume: <");
		sb.Append(m_Volume);
		sb.Append(' ');
		sb.Append(m_Volume_max);
		sb.Append("> pitch: <");
		sb.Append((m_Pitch + m_Pitch_max) * 0.5f);
		sb.Append("> clips: ");
		sb.Append(m_Audio.Length);
		return sb.ToString();
	}
}
