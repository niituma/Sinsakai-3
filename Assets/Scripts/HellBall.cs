using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HellBall : MonoBehaviour
{
    [SerializeField] GameObject _touchEff = default;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHP>().Heel();
            var obj = Instantiate(_touchEff, this.transform.position, this.transform.rotation);
            obj.transform.parent = this.transform.parent;
            Destroy(gameObject);
        }
    }
}
