using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1.0f)] float _timeScale = 1;
    private void Update()
    {
        Time.timeScale = _timeScale;
    }
}
