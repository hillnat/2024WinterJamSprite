using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            timer += Time.deltaTime;
            bool isPlayingNote = currentMeasure.Evaluate(timer);
            SetToneAudioSource(isPlayingNote);
            if (timer > currentMeasure.measureEndTime) { isPlaying = false; }
        }
    }
    private void SetToneAudioSource(bool state)
    {
        if (state && !toneAudioSource.isPlaying) { toneAudioSource.Play(); }
        else if(!state && toneAudioSource.isPlaying) { toneAudioSource.Stop(); }
    }

    public void UICALLBACK_StartPlaying()
    {
        isPlaying = true;
        timer = 0f;
    }
}
