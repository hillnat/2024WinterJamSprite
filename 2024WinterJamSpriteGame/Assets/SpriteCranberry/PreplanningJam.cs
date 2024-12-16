using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PreplanningJam : MonoBehaviour
{
    [Header("Values & References")]
    public TextMeshProUGUI textDisplay;
    public float typingSpeed = 0.05f;
    public float punctuationSpeed = 0.2f;
    public string[] dialogue = new string[1];

    private string targetText;
    private int currentCharacterIndex;
    private Coroutine typingCoroutine;


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

        while (currentCharacterIndex < targetText.Length)
        {
            textDisplay.text += targetText[currentCharacterIndex];
            currentCharacterIndex++;
            if(isPunctuation(targetText[currentCharacterIndex - 1])){
                yield return new WaitForSeconds(punctuationSpeed);
            }
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    public void Start()
    {
        //DisplayText("This is an example sentence displayed over time.");

        TypeFromArray(0);
    }

    public void TypeFromArray(int index){
        if(dialogue.Length < index + 1){
            return;
        }
        DisplayText(dialogue[index]);
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

