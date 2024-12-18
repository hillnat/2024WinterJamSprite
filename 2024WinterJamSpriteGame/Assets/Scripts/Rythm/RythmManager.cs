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
		set {
			_score = value;
			UpdateScoreIcons();
			GameManager.tabs = score;
			switch (score)
			{
				case 5:
					ViewManager.instance.SwitchToDialogue();
					break;
                case 10:
                    ViewManager.instance.SwitchToDialogue();
                    break;
                case 15:
                    ViewManager.instance.SwitchToDialogue();
                    break;
				default:
					break;
            }
		}
	}
	public int _score=0;

    public int miniScore
    {
        get { return _miniScore; }
        set { _miniScore = value; UpdateMiniScore(); }
    }
    public int _miniScore = 0;
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

	public List<RythmMeasure> allMeasures;
	public int currentMeasure
	{
		get { return _currentMeasure; }
		set { _currentMeasure = value; allMeasures[_currentMeasure].CalibrateMeasure();}
	}
	public int _currentMeasure;

	public UiAnimation hitFeedback;
	public TMP_Text noteIconText;

	//private RythmNote recentNote;
	private int priorNoteIndex=int.MinValue;

	public Image scoreIcon1;
	public Image scoreIcon2;
	public Image scoreIcon3;
	public Image miniScoreIcon;

	public Image test;
	#region Unity Callbacks
	private void Awake()
	{
		Singleton();
		currentMeasure = 0;
        noteIconText.text = "";
        score = 0;
		miniScore = 0;
    }
	private void Start()
	{
		
	}
	private void Update()
	{
		if (isPlayingMeasure)
		{
			if (miniScore != 0) { miniScore = 0; }//Reset score but not every frame because the UI update call is tied into the set call

			timer += Time.deltaTime * multiplier;//Increment timer
			EvaluationResults eR = allMeasures[currentMeasure].Evaluate(timer);//Evaluate measure

            if (eR.note != null && (priorNoteIndex == int.MinValue || priorNoteIndex != eR.index))
            {
				Debug.Log("Note has changed");
				//recentNote = eR.note;
				priorNoteIndex = eR.index;
                noteIconText.text += allMeasures[currentMeasure].noteSet[priorNoteIndex].icon;

                AudioManager.instance.PlaySound(eR.note.audioClip, eR.note.volume, eR.note.pitch, eR.note.stereoPan, eR.note.spatialBlend, eR.note.reverb);
            }
            test.enabled = eR.isPlaying;

            //SetToneAudioSource(eR);//Set audio source on or off

            if (timer > allMeasures[currentMeasure].measureEndTime) { timer = 0; isListeningToPlayer = true; isPlayingMeasure = false; noteIconText.text = ""; miniScore = 0; }//Handle end of running timer. After this elapses, the call part is over and we move onto the response

        }
        else if (isListeningToPlayer)
		{
            timer += Time.deltaTime * multiplier;//Increment timer
            EvaluationResults eR = allMeasures[currentMeasure].Evaluate(timer);//Evaluate measure


            if (eR.note != null && (priorNoteIndex == int.MinValue || priorNoteIndex != eR.index))
            {
                Debug.Log("Note has changed");
                //recentNote = eR.note;
                priorNoteIndex = eR.index;
                noteIconText.text += allMeasures[currentMeasure].noteSet[priorNoteIndex].icon;

                AudioManager.instance.PlaySound(eR.note.audioClip, eR.note.volume, eR.note.pitch, eR.note.stereoPan, eR.note.spatialBlend, eR.note.reverb);
            }
			//SetToneAudioSource(eR);//Set audio source on or off
			test.enabled = eR.isPlaying;

            if (InputManager.instance.hit && eR.isPlaying) {
				hitFeedback.RunAnim();
				miniScore++;
			}

            if (timer > allMeasures[currentMeasure].measureEndTime) {
				timer = 0;
				isListeningToPlayer = false;
				isPlayingMeasure = false;
				noteIconText.text = "";
				if (miniScore >= allMeasures[currentMeasure].noteSet.Count) { score++; currentMeasure++; }
			}//Handle end of running timer.
        }
    }
    #endregion
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
		float scoreScaled = score / 5f; //This math works assuming 15 is max score. Fuck shit balls
        scoreIcon1.fillAmount = Mathf.Clamp01(scoreScaled);
        scoreIcon2.fillAmount = Mathf.Clamp01(scoreScaled-1f);
        scoreIcon3.fillAmount = Mathf.Clamp01(scoreScaled-2f);
    }
    private void UpdateMiniScore()
    {
        float miniScoreScaled = (float)miniScore / (float)allMeasures[currentMeasure].noteSet.Count;
		miniScoreIcon.fillAmount = miniScoreScaled;
    }
}
