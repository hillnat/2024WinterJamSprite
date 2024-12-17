using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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

    public UnityEvent OnFree;

    // Start is called before the first frame update
    void Start()
    {
        theCamera.transform.position = firstCameraPos;
        kayak.transform.position = firstKayakPos;
        kayak.transform.eulerAngles = firstKayakRotation;
    }

    public void EndGame() => SceneManager.LoadScene("MainMenu");

    public void Freedom()
    {
        OnFree.Invoke();
    
        theCamera.transform.position = secondCameraPos;
        kayak.transform.position = secondKayakPos;
        kayak.transform.eulerAngles = secondKayakRotation;
    }
}
