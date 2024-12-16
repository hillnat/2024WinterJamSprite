using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TestStepThrough : MonoBehaviour
{
    public UnityEvent onContinue;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnContinue(InputValue value)
    {
        if(onContinue != null)
            onContinue.Invoke();
    }
}
