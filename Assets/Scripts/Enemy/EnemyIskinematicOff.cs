using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class EnemyIskinematicOff : MonoBehaviour
{
    List<Collider> _targetList = new List<Collider>();
    [SerializeField]AudioClip _breakSound = default;
    AudioSource _audio;
    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        Invoke("BreakSound", 2f);
    }

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
    void BreakSound()
    {
        _audio.PlayOneShot(_breakSound);
    }
    public void ListTarget(List<Collider> enemys)
    {
        _targetList = enemys;
    }
}
