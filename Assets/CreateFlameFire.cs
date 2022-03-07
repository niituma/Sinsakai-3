using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateFlameFire : MonoBehaviour
{
    GameObject _player;
    [SerializeField] GameObject _flamefire;
    [SerializeField] float _speed = 50;
    float _time = 0;
    float _creatlimittime = 3f;
    float _resettimer;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _resettimer = _creatlimittime;
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        if (_time >= _creatlimittime)
        {
            var obj = Instantiate(_flamefire, this.transform.position, Quaternion.Euler(-90, 0, 0));
            obj.transform.parent = this.transform.root;
            if (_creatlimittime >= 9f)
            {
                _time = 0;
                _creatlimittime = _resettimer;
                this.gameObject.SetActive(false);
                return;
            }
            _creatlimittime += _resettimer;
        }
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
    }
}
