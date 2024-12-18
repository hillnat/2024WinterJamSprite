using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public UnityEvent OnOpenMenu;
    public UnityEvent OnOpenSettings;

    void OnEnable()
    {
        OnOpenMenu.Invoke();
    }

    public void OpenSettings() => OnOpenSettings.Invoke();
    public void OpenMenu() => OnOpenMenu.Invoke();
    //+Unpause game
    public void BackToMainMenu(){
        //Reset
        //TODO: consider not doing it here, nor in main menu, only if tabs are 15/ game is over
        GameManager.tabs = 0;
        GameManager.dialogueTreeIndex = 0;
        if(RythmManager.instance){ RythmManager.instance.score = 0; }
        //Escape
        SceneManager.LoadScene("MainMenu");
    }
}
