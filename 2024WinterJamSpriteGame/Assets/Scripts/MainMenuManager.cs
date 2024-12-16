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
        SceneManager.LoadScene("MainGame");
    }
}
