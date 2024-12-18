using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void UICALLBACK_Quit()
    {
        Application.Quit();
    }
    public void UICALLBACK_Play()
    {
        //Reset if possible
        GameManager.tabs = 0;
        GameManager.dialogueTreeIndex = 0;
        if(RythmManager.instance){ RythmManager.instance.score = 0; }
        //Play
        SceneManager.LoadScene("MainGame");
    }
}
