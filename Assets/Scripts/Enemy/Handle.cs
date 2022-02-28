using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    [SerializeField] Transform _handpos;
    [SerializeField] float _yoffset = 6.5f;
    Vector3 newhandpos;
    GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        newhandpos = new Vector3(_handpos.position.x, _handpos.position.y - _yoffset, _handpos.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ledge Chercker")
        {
            var player = other.GetComponentInParent<PlayerController>();
            player.GrabLedge(newhandpos,this.transform);
        }
    }
    public void ChanegeNextHandle(float offset)
    {
        newhandpos.y += offset;
        var player = _player.GetComponent<PlayerController>();
        player.GrabLedge(newhandpos, this.transform);
    }


}
