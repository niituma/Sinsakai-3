using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attackmagic_1Move : MonoBehaviour
{
    [SerializeField] float _lifetime = 3f;
    [SerializeField] float _movespeed = 3f;
    Rigidbody _rb = default;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.velocity = this.transform.forward * _movespeed;
        Destroy(gameObject, _lifetime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        Destroy(this.gameObject);
    }
}
