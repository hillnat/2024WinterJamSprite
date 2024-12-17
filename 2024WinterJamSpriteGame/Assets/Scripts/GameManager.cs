using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    //Global variables
    public static float masterVolume = 1.0f;
    public static float musicVolume = 0.33f;
    public static float sfxVolume = 0.5f;

    public static int dialogueTreeIndex = 0;
    public static int tabs = 0;

    public static void IncrementDialogueTreeIndex(){
        if(dialogueTreeIndex == 0){
            dialogueTreeIndex = 1;
        }
        if(dialogueTreeIndex == 1){
            //check for tabs
            if(tabs >= 5){
                //Go to next dialogue
                dialogueTreeIndex = 2;
            }
        }
        if(dialogueTreeIndex == 2){
            //check for tabs
            if(tabs >= 10){
                //Go to next dialogue
                dialogueTreeIndex = 3;
            }
        }
        if(dialogueTreeIndex == 3){
            //check for tabs
            if(tabs >= 15){
                //Go to next dialogue
                dialogueTreeIndex = 4;
            }
        }
        if(dialogueTreeIndex == 4){
            dialogueTreeIndex = 5;
            //Initiate escape or something
        }
    }
}
