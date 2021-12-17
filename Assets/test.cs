using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOMove(new Vector3(0, 9, 0), 0.5f).SetRelative(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
