using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyBase : MonoBehaviour
{
    [SerializeField] Vector3 _sarchRangeCenter = default;
    [SerializeField] float _turnSpeed = 8.0f;
    /// <summary>Player検知範囲の半径</summary>
    [SerializeField] float _sarchsRangeRadius = 1f;
    [SerializeField] float _movingdis = 1f;
    [SerializeField] Collider player;
    Vector3 direction = new Vector3(0f, 0f, 10f);
    GameObject _player = default;
    float _speed = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Sarch();
    }
    void OnDrawGizmosSelected()
    {
        // 攻撃範囲を赤い線でシーンビューに表示する
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(PlayerSarchRangeCenter(), _sarchsRangeRadius);
    }
    Vector3 PlayerSarchRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _sarchRangeCenter.z
            + this.transform.up * _sarchRangeCenter.y
            + this.transform.right * _sarchRangeCenter.x;
        return center;
    }
    void Sarch()
    {
        player = Physics.OverlapSphere(PlayerSarchRangeCenter(), _sarchsRangeRadius).Where(p => p.tag == "Player").FirstOrDefault();
        if (player)
        {
            var dir = player.transform.position - transform.position;
            dir.y = 0;

            var lookRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
            if(Vector3.Distance(transform.position, player.transform.position) >= _movingdis)
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, _speed * Time.deltaTime);
        }
    }
}
