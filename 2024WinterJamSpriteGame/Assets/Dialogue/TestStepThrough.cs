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
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    void CheckInput(){ //insertInput
        Input.GetKeyDown(KeyCode.Space);
    }

    void OnContinue(InputValue value)
    {
        if(dialogueManager != null)
            dialogueManager.ReadNextLine();
    }
}
