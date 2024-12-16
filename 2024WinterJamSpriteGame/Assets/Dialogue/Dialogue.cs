using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dialogue")]
public class Dialogue : ScriptableObject
{
    public float typingSpeed = 0.05f;
    public float punctuationSpeed = 0.2f;
    public AudioClip sound = null;
    public string[] text = new string[1];
}