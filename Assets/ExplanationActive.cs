using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationActive : MonoBehaviour
{
    Animator _anim;
    private void Start()
    {
        _anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            _anim.CrossFade("Explanation", 0f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _anim.CrossFade("ExplanationClose", 0f);
        }
    }
}
