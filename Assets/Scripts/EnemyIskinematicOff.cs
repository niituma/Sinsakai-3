using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class EnemyIskinematicOff : MonoBehaviour
{
    List<Collider> _targetList = new List<Collider>();

    private void OnDestroy()
    {
        _targetList?.Where(t => t != null).ToList().ForEach(t =>
        {
            t.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            //if (t.gameObject.GetComponent<NavMeshAgent>())
            //{
            //    t.gameObject.GetComponent<Rigidbody>().useGravity = false;
            //    t.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            //}
        });
    }
    public void ListTarget(List<Collider> enemys)
    {
        _targetList = enemys;
    }
}
