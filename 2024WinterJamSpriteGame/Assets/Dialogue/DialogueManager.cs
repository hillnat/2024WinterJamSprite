using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("Settings")]
    public bool shouldReadFirstLine = true;
    public WaitType waitType = WaitType.Skip;

    [Header("Object References")]
    public TextTyper textTyper;
    public Dialogue[] dialogueTrees = new Dialogue[1];
    /// <summary> For individual textboxes </summary>
    int stepThroughIndex = 0;
    public UnityEvent OnEndGame;

    void OnEnable()
    {
        int ind = GameManager.dialogueTreeIndex;
        if(dialogueTrees != null && dialogueTrees.Length > ind) { LoadDialogueTree(ind); }
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
        if(index == 5){
            OnEndGame.Invoke();
            return;
        }

        textTyper.dialogue = dialogueTrees[index];
        stepThroughIndex = 0;
        textTyper.SetupRefs();
        
        if(shouldReadFirstLine) { ReadNextLine(); }
    }

    public void ReadNextLine(){
        if(textTyper == null || textTyper.dialogue == null) { return; }
        if(stepThroughIndex > textTyper.dialogue.text.Length){
            //Tell game manager to load next dialogue tree
            GameManager.IncrementDialogueTreeIndex();
            LoadDialogueTree(GameManager.dialogueTreeIndex);
            return;
        }

        //Apply text
        textTyper.TypeFromArray(stepThroughIndex);
        stepThroughIndex++;
    }

    public void TryReadNextLine(){
        if(waitType == WaitType.Wait && textTyper.isTyping) { return; }
        if(waitType == WaitType.Skip && textTyper.isTyping) { 
            textTyper.Skip(); 
            return;
        }
        ReadNextLine();
    }
}

public enum WaitType{
    None, Skip, Wait
}