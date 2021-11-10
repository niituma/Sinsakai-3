using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetLookOn : MonoBehaviour
{
    GameObject[] _targets = default;
    GameObject _player = default;
    GameObject _neartarget;


    void Start()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        _neartarget = _targets?.Where(t => Vector3.Distance(t.transform.position, _player.transform.position) <= 3f).OrderBy(t => Vector3.Distance(t.transform.position, _player.transform.position)).FirstOrDefault();
        if (_neartarget)
        {
            transform.position = _neartarget.transform.position + new Vector3(0, 1.3f, 0);
        }
    }
}
