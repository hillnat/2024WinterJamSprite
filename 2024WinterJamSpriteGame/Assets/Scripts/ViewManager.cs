using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    public Transform dialogueRoot;
    public Transform mainGameRoot;

    #region Singleton
    public static ViewManager instance;
    private void Singleton()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }
    #endregion
    private void Awake()
    {
        Singleton();
        SwitchToMainGame();
    }
    public void SwitchToDialogue()
    {
        dialogueRoot.gameObject.SetActive(true);
        mainGameRoot.gameObject.SetActive(false);
    }
    public void SwitchToMainGame()
    {
        dialogueRoot.gameObject.SetActive(false);
        mainGameRoot.gameObject.SetActive(true);
    }
}
