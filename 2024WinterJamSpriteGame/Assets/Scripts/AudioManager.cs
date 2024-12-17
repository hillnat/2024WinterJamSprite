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
	public void PlaySound(AudioClip clip, float volume, float pitch, float stereoPan, float spatialBlend, float reverb)
	{
		int emitterIndex = GetFreeEmitter();
		if (emitterIndex != -1) {return;}
		AudioSource target = emitters[emitterIndex];
		target.volume = volume;
		target.pitch = pitch;
		target.panStereo = stereoPan;
		target.spatialBlend = spatialBlend;
		target.reverbZoneMix = reverb;
		target.clip = clip;
		target.PlayOneShot(clip);
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
			if (!emitters[i].isPlaying) { return i; }
		}
		Debug.Log("Faield to find emitter");
		return -1;//Failed
	}
}
