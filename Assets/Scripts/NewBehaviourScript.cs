using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    GameObject[] _targets = default;
    GameObject _player = default;
    void Start()
    {
        _player = GameObject.Find("Player");
    }

    void Update()
    {
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject _neartarget = _targets?.OrderBy(t => Vector3.Distance(t.transform.position, _player.transform.position)).FirstOrDefault();
        transform.position = _neartarget.transform.position + new Vector3(0,1.3f,0);
    }
}
