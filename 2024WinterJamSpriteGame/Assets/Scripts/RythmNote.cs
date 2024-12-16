using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "RythmNote", menuName = "Rythm Note", order = 100)]

public class RythmNote : ScriptableObject
{
    public float preDelay;
    public float duration;
    public float postDelay;
}
