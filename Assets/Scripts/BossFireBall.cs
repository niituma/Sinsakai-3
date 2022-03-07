using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBall : MonoBehaviour
{
    GameObject _player;
    [SerializeField] float _speed = 3f;
    float _trakingTimer = 1.5f;
    float time = 0.0f;
    [SerializeField]GameObject _eff = default;
    Vector3 _offset = new Vector3(0,1.3f,0);
    Rigidbody _rb;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time <= _trakingTimer)
        {
            this.transform.LookAt(_player.transform);
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position + _offset, _speed * Time.deltaTime);
            return;
        }

        _rb.velocity = transform.forward * _speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Postprocessing"&& other.tag != "Boss")
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        var obj = Instantiate(_eff, this.transform.position, this.transform.rotation);
        obj.transform.parent = this.transform.parent;
    }
}
