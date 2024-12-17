using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RythmManager : MonoBehaviour
{
    #region Singleton
    public static RythmManager instance;
    private void Singleton()
    {

        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }
    #endregion
    public float timer = 0f;
	public float multiplier = 2.66666666667f; //90 bpm = 2.6666
	//If mult = 1, it is 240 bpm. There are 4 beats per second
	public bool isRunning=false;
	private AudioSource toneAudioSource;
	public RythmMeasure currentMeasure
	{
		get { return _currentMeasure; }
		set { _currentMeasure = value; _currentMeasure.CalibrateNoteTimes(); }
	}
	public RythmMeasure _currentMeasure;

	public ParticleSystem hitFeedback;
	#region Unity Callbacks
	private void Awake()
	{
		toneAudioSource = GetComponent<AudioSource>();
		toneAudioSource.loop = true;
		toneAudioSource.Stop();
	}
	private void Start()
	{
		if (currentMeasure != null) { currentMeasure.CalibrateNoteTimes(); }
	}
	private void Update()
	{
		if (isRunning)
		{
			timer += Time.deltaTime * multiplier;//Increment timer
			EvaluationResults eR = currentMeasure.Evaluate(timer);//Evaluate measure
			SetToneAudioSource(eR);//Set audio source on or off

			if (InputManager.instance.hit && eR.isPlaying) { hitFeedback.Play(); }

			if (timer > currentMeasure.measureEndTime) { isRunning = false; Debug.Log($"Ended after {timer} seconds"); }//Handle end of running timer
		}
	}
	#endregion
	private void SetToneAudioSource(EvaluationResults eR)
	{
		if (eR.isPlaying && !toneAudioSource.isPlaying) {
			//Set other audio stuff. Im aware this will get called each frame there is a note playing. However adding the state checks would probably be even less performant.
			toneAudioSource.volume = eR.note.volume;
			toneAudioSource.pitch = eR.note.pitch;
			toneAudioSource.panStereo = eR.note.stereoPan;
			toneAudioSource.spatialBlend = eR.note.spatialBlend;
			toneAudioSource.reverbZoneMix = eR.note.reverb;
			if (eR.note.audioClip != null) { toneAudioSource.clip = eR.note.audioClip; toneAudioSource.Play(); }
		}
		else if(!eR.isPlaying && toneAudioSource.isPlaying) { toneAudioSource.Stop(); }
	}
	#region UI Callbacks
	public void UICALLBACK_StartPlaying()
	{
		isRunning = true;
		timer = 0f;
	}
	#endregion
}
