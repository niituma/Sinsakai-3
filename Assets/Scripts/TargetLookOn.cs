using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetLookOn : MonoBehaviour
{
    Collider[] _targets = default;
    GameObject _player = default;
    Collider _neartarget;
    [SerializeField] GameObject playercon;
    [SerializeField] float distance = 5f;
    PlayerMove target;
    public bool targeton;
    public bool isneartarget = true;

    void Start()
    {
        _player = GameObject.Find("Player");
        target = playercon.GetComponent<PlayerMove>();
    }

    void Update()
    {
        _targets = target._currentenemy;
        if (isneartarget)
        {
            _neartarget = _targets?.Where(t => t.tag == "Enemy").OrderBy(t => Vector3.Distance(t.transform.position, _player.transform.position)).FirstOrDefault();
        }
        if (_neartarget)
        {
            targeton = true;
            transform.position = _neartarget.transform.position + new Vector3(0, 1.3f, 0);
        }
        
        if(Vector3.Distance(transform.position, _player.transform.position) >= distance)
        {
            targeton = false;
        }
    }
}
