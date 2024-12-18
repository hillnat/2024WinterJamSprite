using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndSequence : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] GameObject theCamera;
    [SerializeField] Vector3 firstCameraPos;
    [SerializeField] Vector3 secondCameraPos;
    [Header("Kayak")]
    [SerializeField] GameObject kayak;
    [SerializeField] Vector3 firstKayakPos;
    [SerializeField] Vector3 firstKayakRotation;
    [SerializeField] Vector3 secondKayakPos;
    [SerializeField] Vector3 secondKayakRotation;

    [Header("Splash")]
    [SerializeField] GameObject Guy;
    [SerializeField] Image Darkness;
    [SerializeField] GameObject cutsceneCanvas;
    [SerializeField] AudioSource splasher;


    public UnityEvent OnFree;

    // Start is called before the first frame update
    void Start()
    {
        theCamera.transform.position = firstCameraPos;
        kayak.transform.position = firstKayakPos;
        kayak.transform.eulerAngles = firstKayakRotation;
        Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, 0f);

        StartCoroutine(Splash());
    }

    public void EndGame(){
        //Reset
        GameManager.tabs = 0;
        GameManager.dialogueTreeIndex = 0;
        if(RythmManager.instance){ RythmManager.instance.score = 0; }
        //Escape
        SceneManager.LoadScene("MainMenu");
    }

    public void Freedom()
    {
        OnFree.Invoke();
    
        theCamera.transform.position = secondCameraPos;
        kayak.transform.position = secondKayakPos;
        kayak.transform.eulerAngles = secondKayakRotation;
    }

    private IEnumerator Splash(){
        yield return new WaitForSeconds(2);
        //Guy
        float fadeDuration = 2f; // Adjust as needed
        float elapsedTime = 0f;
    
        //Fade In
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.Lerp(0, 1f, elapsedTime / fadeDuration));
            yield return null;
        }

        yield return new WaitForSeconds(1);

        //Splash
        splasher.Play();
    
        //Guy gone
        Guy.SetActive(false);

        yield return new WaitForSeconds(1);
    
        // Fade Out
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            Darkness.color = new Color(Darkness.color.r, Darkness.color.g, Darkness.color.b, Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration));
            yield return null;
        }

        cutsceneCanvas.SetActive(false);
        Freedom();
    }
}
