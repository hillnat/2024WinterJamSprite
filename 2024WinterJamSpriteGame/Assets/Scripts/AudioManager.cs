using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

	#region Singleton
	public static AudioManager instance;
	private void Singleton()
	{
		if (instance != null)
		{
			Destroy(instance);
		}

		instance = this;
	}
	#endregion
	public GameObject emitterPrefab;
	public List<AudioSource> emitters;
	private void Awake()
	{
		Singleton();
		InitPool();
	}
    public void UICALLBACK_PlaySoundui(AudioClip clip)
    {
        int emitterIndex = GetFreeEmitter();
        if (emitterIndex == -1) { return; }
        AudioSource target = emitters[emitterIndex];
        if (target.isPlaying) { target.Stop(); }

        target.volume = 1;
        target.pitch = 1;
        target.panStereo = 0;
        target.spatialBlend = 0;
        target.reverbZoneMix = 0;
        target.clip = clip;
        Debug.Log($"Playing sound {clip.name}");
        target.Play();
    }
    public void PlaySound(AudioClip clip, float volume, float pitch, float stereoPan, float spatialBlend, float reverb)
	{
		int emitterIndex = GetFreeEmitter();
		if (emitterIndex == -1) { return;}
		AudioSource target = emitters[emitterIndex];
        if (target.isPlaying) { target.Stop(); }

        target.volume = volume;
		target.pitch = pitch;
		target.panStereo = stereoPan;
		target.spatialBlend = spatialBlend;
		target.reverbZoneMix = reverb;
		target.clip = clip;
		Debug.Log($"Playing sound {clip.name}");
		target.Play();
	}
	private void InitPool()
	{
		for(int i=0; i<emitters.Count; i++)
		{
			Destroy(emitters[i].gameObject);
		}
		emitters.Clear();
		for (int i = 0; i < 10; i++)
		{
			AudioSource aS = Instantiate(emitterPrefab, Vector3.zero, Quaternion.identity, null).GetComponent<AudioSource>();
			aS.playOnAwake = false;
			emitters.Add(aS);
		}
	}
	private int GetFreeEmitter()
	{
		for(int i=0; i<emitters.Count; i++)
		{
			if (!emitters[i].isPlaying) { Debug.Log($"Found Audio Emitter at index {i}"); return i; }
		}
		Debug.Log("Faield to find emitter");
		return -1;//Failed
	}
}
