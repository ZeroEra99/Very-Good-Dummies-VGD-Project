using UnityEngine.Audio;
using UnityEngine;

public enum SoundType { Theme, GlobalSound, LocalSound};

[System.Serializable]
public class Sound {

	public string name;
	public SoundType type;
    public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

    [HideInInspector]
    public AudioMixerGroup mixerGroup;
    [HideInInspector]
    public AudioSource source;

}
