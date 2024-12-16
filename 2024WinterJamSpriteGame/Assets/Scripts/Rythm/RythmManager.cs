using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RythmManager : MonoBehaviour
{
    public float timer = 0f;
    public bool isPlaying=false;
    private AudioSource toneAudioSource;
    public RythmMeasure currentMeasure
    {
        get { return _currentMeasure; }
        set { _currentMeasure = value; _currentMeasure.CalibrateNoteTimes(); }
    }
    public RythmMeasure _currentMeasure;

    public Image hitFeedbackImage;
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
        if (isPlaying)
        {
            timer += Time.deltaTime;//Increment timer
            bool isPlayingNote = currentMeasure.Evaluate(timer);//Evaluate measure
            SetToneAudioSource(isPlayingNote);//Set audio source on or off

            if (InputManager.instance.hit && isPlayingNote) { StartCoroutine(FlashHitFeedback()); }

            if (timer > currentMeasure.measureEndTime) { isPlaying = false; }//Handle end of timer
        }
    }
    #endregion
    private void SetToneAudioSource(bool state)
    {
        if (state && !toneAudioSource.isPlaying) { toneAudioSource.Play(); }
        else if(!state && toneAudioSource.isPlaying) { toneAudioSource.Stop(); }
    }
    #region UI Callbacks
    public void UICALLBACK_StartPlaying()
    {
        isPlaying = true;
        timer = 0f;
    }
    #endregion

    IEnumerator FlashHitFeedback()
    {
        hitFeedbackImage.color = new Vector4(1f, 1f, 1f, 0f);
        yield return new WaitForSeconds(0.1f);
        hitFeedbackImage.color = new Vector4(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        hitFeedbackImage.color = new Vector4(1f, 1f, 1f, 0f);
    }
}
