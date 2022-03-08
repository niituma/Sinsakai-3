using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTeleport : MonoBehaviour
{
    [SerializeField] Transform _teleportPos = default;
    [SerializeField] GameObject _bossroombarrier = default;
    [SerializeField] GameObject _boss = default;
    GameObject _player;
    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        if (!_boss)
        {
            _bossroombarrier.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.position = _teleportPos.position;
        }
    }
     public void Teleport()
    {
        _player.transform.position = this.transform.position;
    }
}
