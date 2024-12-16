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
            EvaluationResults eR = currentMeasure.Evaluate(timer);//Evaluate measure
            SetToneAudioSource(eR);//Set audio source on or off

            if (InputManager.instance.hit && eR.success) { StartCoroutine(FlashHitFeedback()); }

            if (timer > currentMeasure.measureEndTime) { isPlaying = false; }//Handle end of timer
        }
    }
    #endregion
    private void SetToneAudioSource(EvaluationResults eR)
    {
        if (eR.success && !toneAudioSource.isPlaying) {
            toneAudioSource.clip = eR.note.getRandomClip;
            //Set other audio stuff. Im aware this will get called each frame there is a note playing. However adding the state checks would probably be even less performant.
            toneAudioSource.volume = eR.note.volume;
            toneAudioSource.pitch = eR.note.pitch;
            toneAudioSource.panStereo = eR.note.stereoPan;
            toneAudioSource.spatialBlend = eR.note.spatialBlend;
            toneAudioSource.reverbZoneMix = eR.note.reverb;
            toneAudioSource.Play();
            
        }
        else if(!eR.success && toneAudioSource.isPlaying) { toneAudioSource.Stop(); }
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
        hitFeedbackImage.color = new Vector4(0f, 1f, 0f, 0f);
        yield return new WaitForSeconds(0.1f);
        hitFeedbackImage.color = new Vector4(0f, 1f, 0f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        hitFeedbackImage.color = new Vector4(0f, 1f, 0f, 0f);
    }
}
