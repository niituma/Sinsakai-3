using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    Collider _collder;
    SphereCollider _sphereCollider;
    private void Start()
    {
        _collder = GetComponent<Collider>();
        _sphereCollider = GetComponent<SphereCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (_collder is SphereCollider)
            {
                ((SphereCollider)_collder).enabled = false;
            }

            _sphereCollider.enabled = false;
        }
    }
}
