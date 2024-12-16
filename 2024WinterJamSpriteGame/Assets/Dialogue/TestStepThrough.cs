using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TestStepThrough : MonoBehaviour
{
    public DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        if(dialogueManager != null)
            dialogueManager.ReadNextLine();
    }

    void OnContinue(InputValue value)
    {
        print("Continue");
        if(dialogueManager != null)
            dialogueManager.ReadNextLine();
    }
}
