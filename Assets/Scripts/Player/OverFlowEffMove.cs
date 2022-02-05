using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverFlowEffMove : MonoBehaviour
{
    GameObject _player = default;
    float _speed = 100f;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(_player)
        transform.position = Vector3.MoveTowards(this.transform.position, _player.transform.position, _speed * Time.deltaTime);
    }
}
