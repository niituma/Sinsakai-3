using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetLookOn : MonoBehaviour
{
    [SerializeField] int _targetindex;
    GameObject _player = default;
    [SerializeField] Collider _nowtarget = null;
    [SerializeField] float _distance = 5f;
    PlayerController _targets;
    [SerializeField] List<Collider> _targetList = new List<Collider>();
    public bool _isneartarget = default;
    public bool _targeton = true;
    public bool _targetchange = default;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _targets = _player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (_targets)
        {
            _targetList = _targets._currentenemy ?? _targets?._currentenemy.Where(t => t.tag == "Enemy")
                .OrderBy(t => Vector3.Distance(t.transform.position, _player.transform.position)).ToList();
            if (_targeton)
            {
                _nowtarget = _targetList.FirstOrDefault();
                _targetindex = 0;
            }
        }

        if (_nowtarget)
        {
            _isneartarget = true;
            transform.position = _nowtarget.transform.position + new Vector3(0, 1.3f, 0);
        }
        else
        {
            _isneartarget = false;
        }
        if (_targets && _player)
        {
            if (Vector3.Distance(transform.position, _player.transform.position) >= _distance)
            {
                _isneartarget = false;
                _targetindex = 0;
            }
        }
    }
    public void ChangeTarget()
    {
        if (_targetList.Count > 1)
        {
            _targetindex++;
            _nowtarget = _targetList[_targetindex % _targetList.Count];
        }
    }
}
