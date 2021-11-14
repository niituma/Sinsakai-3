using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetLookOn : MonoBehaviour
{
    [SerializeField] int _targetindex;
    GameObject _player = default;
    [SerializeField] Collider _nowtarget = null;
    [SerializeField] GameObject playercon;
    [SerializeField] float distance = 5f;
    PlayerMove targets;
    [SerializeField] List<Collider> targetList = new List<Collider>();
    public bool isneartarget = default;
    public bool targeton = true;
    public bool targetchange = default;

    void Start()
    {
        _player = GameObject.Find("Player");
        targets = playercon.GetComponent<PlayerMove>();
    }

    void Update()
    {
        targetList = targets._currentenemy?.Where(t => t.tag == "Enemy").Distinct().ToList();
        targetList = targetList?.OrderBy(t => Vector3.Distance(t.transform.position, _player.transform.position)).Distinct().ToList();
        if (targeton)
        {
            _nowtarget = targetList.FirstOrDefault();
            _targetindex = 0;
        }
        
        if (_nowtarget)
        {
            isneartarget = true;
            transform.position = _nowtarget.transform.position + new Vector3(0, 1.3f, 0);
        }
        else
        {
            isneartarget = false;
        }

        if (Vector3.Distance(transform.position, _player.transform.position) >= distance)
        {
            isneartarget = false;
            _targetindex = 0;
        }
    }
    public void ChangeTarget()
    {
        if (targetList.Count > 1)
        {
            _targetindex++;
            _nowtarget = targetList[_targetindex % targetList.Count];
        }
    }
}
