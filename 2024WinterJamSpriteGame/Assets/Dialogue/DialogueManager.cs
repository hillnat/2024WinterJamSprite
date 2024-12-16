using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextTyper textTyper;
    public Dialogue[] dialogueTrees = new Dialogue[1];
    /// <summary> For individual textboxes </summary>
    int stepThroughIndex = 0;

    void Start()
    {
        if(dialogueTrees != null && dialogueTrees.Length > 0) { LoadDialogueTree(0); }
    }

    void LoadDialogueTree(int index){
        if(textTyper == null) {
            Debug.LogWarning("No Text Typer");
            return;
        }
        if(dialogueTrees.Length < index + 1) {
            Debug.LogWarning("Not Enough Dialogue Trees");
            return; 
        }

        textTyper.dialogue = dialogueTrees[index];
        stepThroughIndex = 0;
    }

    public void ReadNextLine(){
        if(textTyper == null) { return; }
        
        textTyper.TypeFromArray(stepThroughIndex);
        stepThroughIndex++;
    }
}
