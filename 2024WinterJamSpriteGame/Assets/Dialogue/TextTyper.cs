using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class TextTyper : MonoBehaviour
{
    [Header("Values & References")]
    public TextMeshProUGUI textDisplay;
    public Dialogue dialogue;
    public AudioSource audioSource;

    [HideInInspector]
    public bool isTyping;
    private string targetText;
    private int currentCharacterIndex;
    private Coroutine typingCoroutine;

    void Awake()
    {
        if(audioSource == null){
            audioSource = GetComponent<AudioSource>();
        }
    }
    public void SetupRefs(){
        audioSource.clip = dialogue.sound;
    }
    public void DisplayText(string text)
    {
        // Stop any existing coroutine to prevent overlap
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        targetText = text;
        currentCharacterIndex = 0;
        typingCoroutine = StartCoroutine(TypeText());
    }


    private IEnumerator TypeText()
    {
        textDisplay.text = ""; // Clear existing text
        isTyping = true;

        while (currentCharacterIndex < targetText.Length)
        {
            textDisplay.text += targetText[currentCharacterIndex];
            currentCharacterIndex++;
            if(dialogue.sound != null){
                //Play sound, cut previous
                audioSource.Stop();
                audioSource.Play();
            }
            if(isPunctuation(targetText[currentCharacterIndex - 1])){
                yield return new WaitForSeconds(dialogue.punctuationSpeed);
            }
            yield return new WaitForSeconds(dialogue.typingSpeed);
        }
        if(dialogue.sound != null){
                audioSource.Stop();
        }
        isTyping = false;
    }

    public void Skip(){
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textDisplay.text = targetText;
        isTyping = false;
    }

    public void TypeFromArray(int index){
        if(dialogue == null || dialogue.text == null || dialogue.text.Length < index + 1){
            return;
        }
        DisplayText(dialogue.text[index]);
    }

    bool isPunctuation(char c){
        switch (c){
            case ',':
            case '.':
            case '!':
            case ':':
            case ';':
            case '?':
                return true;
            default:
                return false;
        }
    }
}

