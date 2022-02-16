using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressAnyKey : MonoBehaviour
{
    [SerializeField] UnityEvent _events;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            _events.Invoke();
        }
    }
}
