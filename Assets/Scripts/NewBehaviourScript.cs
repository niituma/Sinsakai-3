using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] GameObject _gard = default;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitPos = other.bounds.ClosestPoint(this.transform.position);
        if (other.tag == "EnemyHit")
        {
            Instantiate(_gard, hitPos, Quaternion.identity);
        }
    }
}
