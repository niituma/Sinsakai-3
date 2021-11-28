using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGade : MonoBehaviour
{
    [SerializeField] GameObject[] _stones = default;
    int _count = 0;

    // Update is called once per frame
    void Update()
    {
        if (_count >= 3)
        {
            Destroy(gameObject);
        }
    }
    public void OpenCount()
    {
        _count++;
    }
}
