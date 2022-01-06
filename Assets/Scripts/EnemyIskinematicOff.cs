using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIskinematicOff : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
