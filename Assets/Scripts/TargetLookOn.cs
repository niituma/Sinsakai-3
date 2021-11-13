using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetLookOn : MonoBehaviour
{
    [SerializeField] int _targetindex;
    GameObject _player = default;
    Collider _nowtarget;
    [SerializeField] GameObject playercon;
    [SerializeField] float distance = 5f;
    PlayerMove targets;
    List<Collider> targetList = new List<Collider>();
    public bool targeton;
    public bool isneartarget = true;

    void Start()
    {
        _player = GameObject.Find("Player");
        targets = playercon.GetComponent<PlayerMove>();
    }

    void Update()
    {
        targetList = targets._currentenemy?.Where(t => t.tag == "Enemy").Distinct().ToList();
        if (isneartarget)
        {
            targetList = targetList?.OrderBy(t => Vector3.Distance(t.transform.position, _player.transform.position)).Distinct().ToList();
            _nowtarget = targetList[_targetindex % targetList.Count];//targetList.FirstOrDefault();
            _targetindex = 0;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            _targetindex++;
            if (targetList.Count > 0)
                _nowtarget = targetList[_targetindex % targetList.Count];

        }
        if (_nowtarget)
        {
            targeton = true;
            transform.position = _nowtarget.transform.position + new Vector3(0, 1.3f, 0);
        }
        else
        {
            targeton = false;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) >= distance)
        {
            targeton = false;
            _targetindex = 0;
        }
    }
}
