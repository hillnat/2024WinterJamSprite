using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Singleton
    public static InputManager instance;
    private void Singleton()
    {
        if (instance != null)
        {
            Destroy(instance);
        }

        instance = this;
    }
    #endregion
    #region States
    public bool hit = false;
    #endregion
    #region Unity Callbacks
    private void Awake()
    {
        Singleton();
    }
    private void LateUpdate()
    {
        if (hit) { hit = false; }
    }
    #endregion
    #region Input System Callbacks
    public void OnHit()
    {
        hit = true;
    }
    public void OnCheats()
    {
        RythmManager.instance.miniScore++;
    }
    #endregion
}
