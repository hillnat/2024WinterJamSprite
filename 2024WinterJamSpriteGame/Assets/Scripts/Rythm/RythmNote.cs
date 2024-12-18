using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RythmNote", menuName = "Rythm Note", order = 100)]

public class RythmNote : ScriptableObject
{
    [Header("Note Char")]
    public char icon;
    [Header("Audio Emitter Settings")]
    public AudioClip audioClip;
    public float volume = 1f;
    public float pitch = 1f;
    public float stereoPan = 0f;
    public float spatialBlend = 0f;
    public float reverb = 1f;
}
