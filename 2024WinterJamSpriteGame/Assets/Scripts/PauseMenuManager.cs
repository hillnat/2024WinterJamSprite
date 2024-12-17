using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public UnityEvent OnOpenMenu;
    public UnityEvent OnOpenSettings;

    void Awake()
    {
        //OnOpenSettings.Invoke(); //Debug
    }

    public void OpenSettings() => OnOpenSettings.Invoke();
    public void OpenMenu() => OnOpenMenu.Invoke();
    //+Unpause game
    public void BackToMainMenu() => SceneManager.LoadScene("MainMenu");
}
