using System.Collections;
using System.Collections.Generic;
using TMPro;
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
	//Score is the total number of tabs you have collected, up to 15
	public int score
	{
		get { return _score; }
		set
		{
			_score = value;
			currentMeasure = Mathf.Clamp(_score, 0, allMeasures.Count);
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
	public int _score = 0;
	//Mini score is your progress through the current measure
	public int miniScore
	{
		get { return _miniScore; }
		set { _miniScore = value; UpdateMiniScore(); }
	}
	public int _miniScore = 0;

	public float timer = 0f;//Measure timer
	public float multiplier = 2.66666666667f;
	public bool isPlayingMeasure = false;//Are we playing the measure to the player?
	public bool isListeningToPlayer = false;//Are we waiting for the player to match the rhythm?

	public List<RythmMeasure> allMeasures;
	public int currentMeasure
	{
		get { return _currentMeasure; }
		set { _currentMeasure = value; allMeasures[_currentMeasure].GenerateNoteTimes(); }
	}
	public int _currentMeasure;
	public Animator miniScoreAnimator;
	public Animator yourTurnAnimator;
	public Animator measureBeatAnimator;
	public Animator tryAgainAnimator;
	public TMP_Text noteIconTextWhite;
	public TMP_Text noteIconTextGreen;

	private int lastNoteIndex = int.MinValue;//The index of the previous note we played, in the measures list of notes
	private int lastMiniScoreAtIndex = int.MinValue;

	public Image scoreIcon1;
	public Image scoreIcon2;
	public Image scoreIcon3;
	public Image miniScoreIcon;
	public Canvas rythmCanvas;
	public Image hitIndicator;
	public GameObject starParticles;


	#region Unity Callbacks
	private void Awake()
	{
		Singleton();
		currentMeasure = 0;
		noteIconTextWhite.text = "";
		noteIconTextGreen.text = "";
		score = 0;
		miniScore = 0;
		hitIndicator.enabled = false;
		yourTurnAnimator.gameObject.SetActive(false);
		measureBeatAnimator.gameObject.SetActive(false);
		tryAgainAnimator.gameObject.SetActive(false);
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
			EvaluationResults eR = allMeasures[currentMeasure].EvaluateNoteTimes(timer);//Evaluate measure

			CheckForNoteChange(eR, true);
			hitIndicator.enabled = false;
			if (measureBeatAnimator.gameObject.activeInHierarchy) { measureBeatAnimator.gameObject.SetActive(false); }
			if (tryAgainAnimator.gameObject.activeInHierarchy) { tryAgainAnimator.gameObject.SetActive(false); }

            //SetToneAudioSource(eR);//Set audio source on or off

            if (timer > allMeasures[currentMeasure].measureEndTime) { timer = 0; isListeningToPlayer = true; isPlayingMeasure = false; miniScore = 0; yourTurnAnimator.gameObject.SetActive(true); yourTurnAnimator.SetTrigger("Flash"); }//Handle end of running timer. After this elapses, the call part is over and we move onto the response

		}
		else if (isListeningToPlayer)
		{
			timer += Time.deltaTime * multiplier;//Increment timer
			EvaluationResults eR = allMeasures[currentMeasure].EvaluateNoteTimes(timer);//Evaluate measure


			CheckForNoteChange(eR, false);

			//SetToneAudioSource(eR);//Set audio source on or off
			hitIndicator.enabled = eR.isPlayingWithTolerance;

			//Check if the player clicked at the right time
			if (InputManager.instance.hit)
			{
				if (eR.isPlayingWithTolerance && (lastMiniScoreAtIndex == int.MinValue || lastMiniScoreAtIndex != lastNoteIndex))
				{
					GameObject go = Instantiate(starParticles, Vector3.zero, Quaternion.identity);
					go.transform.SetParent(rythmCanvas.transform);
					go.transform.localPosition = new Vector2(Random.Range(-150f,150f), Random.Range(-150f, 150f));
					miniScore++;
					lastMiniScoreAtIndex = lastNoteIndex;
				}
				else
				{
					miniScoreAnimator.SetTrigger("Shake");
				}
			}

			if(miniScore >= allMeasures[currentMeasure].noteSet.Count)
            {
                measureBeatAnimator.gameObject.SetActive(true);
            }

            if (timer > allMeasures[currentMeasure].measureEndTime + 1.5f)//Timer over for responce
			{
				timer = 0;
				isListeningToPlayer = false;
				isPlayingMeasure = false;
				noteIconTextWhite.text = "";
				noteIconTextGreen.text = "";
				yourTurnAnimator.gameObject.SetActive(false);
				if (miniScore >= allMeasures[currentMeasure].noteSet.Count) {//If score is high enough move on
					score++;
				}
				else
				{
					tryAgainAnimator.gameObject.SetActive(true);
					tryAgainAnimator.SetTrigger("Shake");
				}
			}//Handle end of running timer.
		}
	}
	#endregion
	#region UI Callbacks
	public void UICALLBACK_StartPlayingMeasure()
	{
		if (isListeningToPlayer) { Debug.Log("Tried to start playing the measure while listening to the users input"); return; }
		isPlayingMeasure = true;
		isListeningToPlayer = false;
		timer = 0f;
		noteIconTextWhite.text = "";
		noteIconTextGreen.text = "";
	}
	#endregion

	private void UpdateScoreIcons()
	{
		float scoreScaled = score / 5f; //This math works assuming 15 is max score. Fuck shit balls
		scoreIcon1.fillAmount = Mathf.Clamp01(scoreScaled);
		scoreIcon2.fillAmount = Mathf.Clamp01(scoreScaled - 1f);
		scoreIcon3.fillAmount = Mathf.Clamp01(scoreScaled - 2f);
	}
	private void UpdateMiniScore()
	{
		float miniScoreScaled = (float)miniScore / (float)allMeasures[currentMeasure].noteSet.Count;
		miniScoreIcon.fillAmount = miniScoreScaled;
	}
	private void CheckForNoteChange(EvaluationResults eR, bool useNoteText1)
	{
		if (eR.isPlayingWithoutTolerance && (lastNoteIndex == int.MinValue || lastNoteIndex != eR.index))
		{
			Debug.Log("Note has changed");
			//recentNote = eR.note;
			lastNoteIndex = eR.index;
			if (useNoteText1)
			{
				noteIconTextWhite.text += allMeasures[currentMeasure].noteSet[lastNoteIndex].note.icon;
			}
			else
			{
				noteIconTextGreen.text += allMeasures[currentMeasure].noteSet[lastNoteIndex].note.icon;
			}

			AudioManager.instance.PlaySound(eR.note.audioClip, eR.note.volume, eR.note.pitch, eR.note.stereoPan, eR.note.spatialBlend, eR.note.reverb);
		}
	}
}