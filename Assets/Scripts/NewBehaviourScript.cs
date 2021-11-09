using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject _camera = default;
    [SerializeField] GameObject lookTarget = default;
    void Start()
    {
        //_camera = Camera.main.gameObject;
    }

    void Update()
    {
        
            var direction = lookTarget.transform.position - _camera.transform.position;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction, Vector3.up);
            _camera.transform.rotation = Quaternion.Lerp(_camera.transform.rotation, lookRotation, 0.1f);
    }
}
