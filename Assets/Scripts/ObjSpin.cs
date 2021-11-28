using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjSpin : MonoBehaviour
{
    [SerializeField] float _spinSpeed = 3f;
    [SerializeField] Vector3 _spindir = default;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(_spindir,_spinSpeed * Time.deltaTime);
    }
}
