using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RythmManager : MonoBehaviour
{
	public int score
	{
		get { return _score; }
		set { _score = value; UpdateScoreIcons(); }
	}
	public int _score=0;
	public int miniScore=0;
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
	public bool isPlayingMeasure = false;
	public bool isListeningToPlayer = false;
	private AudioSource toneAudioSource;
	public RythmMeasure currentMeasure
	{
		get { return _currentMeasure; }
		set { _currentMeasure = value; _currentMeasure.CalibrateMeasure();}
	}
	public RythmMeasure _currentMeasure;

	public ParticleSystem hitFeedback;
	public TMP_Text noteIconText;

	private RythmNote recentNote;

	public Image tabIcon1;
	public Image tabIcon2;
	public Image tabIcon3;
	#region Unity Callbacks
	private void Awake()
	{
		Singleton();
		toneAudioSource = GetComponent<AudioSource>();
		toneAudioSource.loop = true;
		toneAudioSource.Stop();
        if (currentMeasure != null) { currentMeasure.CalibrateMeasure(); }
        noteIconText.text = "";
        score = 0;
    }
	private void Start()
	{
		
	}
	private void Update()
	{
		if (isPlayingMeasure)
		{
			timer += Time.deltaTime * multiplier;//Increment timer
			EvaluationResults eR = currentMeasure.Evaluate(timer);//Evaluate measure

            SetNoteIcons(eR);
            SetToneAudioSource(eR);//Set audio source on or off

            if (timer > currentMeasure.measureEndTime) { timer = 0; isListeningToPlayer = true; isPlayingMeasure = false; noteIconText.text = ""; miniScore = 0; }//Handle end of running timer. After this elapses, the call part is over and we move onto the response

        }
        else if (isListeningToPlayer)
		{
            timer += Time.deltaTime * multiplier;//Increment timer
            EvaluationResults eR = currentMeasure.Evaluate(timer);//Evaluate measure

			SetNoteIcons(eR);
            SetToneAudioSource(eR);//Set audio source on or off


            if (InputManager.instance.hit && eR.isPlaying) {
				hitFeedback.Play();
				miniScore++;
			}

            if (timer > currentMeasure.measureEndTime) {
				timer = 0;
				isListeningToPlayer = false;
				isPlayingMeasure = false;
				noteIconText.text = "";
				if (miniScore >= currentMeasure.noteSet.Count) { score++; }
			}//Handle end of running timer.
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
	public void SetNoteIcons(EvaluationResults eR)
	{
        if (eR.note != null && recentNote != eR.note)
        {
            recentNote = eR.note;
            noteIconText.text += recentNote.icon;
        }
    }
	#region UI Callbacks
	public void UICALLBACK_StartPlayingMeasure()
	{
		if (isListeningToPlayer) { Debug.Log("Tried to start palying the measure while listening to the users input"); return; }
		isPlayingMeasure = true;
		timer = 0f;
	}
    #endregion

	private void UpdateScoreIcons()
	{
		float scoreScaled = score / 5f; //If 15, now we have 3
        tabIcon1.fillAmount = Mathf.Clamp01(scoreScaled);
        tabIcon2.fillAmount = Mathf.Clamp01(scoreScaled-1f);
        tabIcon3.fillAmount = Mathf.Clamp01(scoreScaled-2f);
    }
}
