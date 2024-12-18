using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiAnimation : MonoBehaviour
{
    public List<Sprite> frames;
    public float frameDelay=0.1f;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        RunAnim();
    }
    private void Update()
    {
        //if (InputManager.instance.hit) { RunAnim(); }
    }
    public void RunAnim()
    {
        StartCoroutine(anim());
    }

    IEnumerator anim()
    {
        image.enabled = true;
        for (int i=0; i<frames.Count; i++)
        {
            image.sprite = frames[i];
            yield return new WaitForSeconds(frameDelay);
        }
        image.sprite = frames[0];
        image.enabled = false;
        Destroy(this.gameObject);
    }
}
