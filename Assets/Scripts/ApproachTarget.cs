using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ApproachTarget : MonoBehaviour
{
    [SerializeField] float _lifetime = 3f;
    [SerializeField] float _movespeed = 3f;
    [SerializeField] float _lookonspeed = 5f;
    [SerializeField] float _moveDistance;
    GameObject[] _targets = default;
    GameObject _neartarget = default;
    Rigidbody _rb = default;

    void Start()
    {
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        _rb = GetComponent<Rigidbody>();
        _neartarget = _targets.OrderBy(t => Vector3.Distance(t.transform.position, this.transform.position)).FirstOrDefault();
        _rb.velocity = this.transform.forward * _movespeed;
        Destroy(gameObject, _lifetime);
    }
    void Update()
    {
        Vector3 targetPos = _neartarget.transform.position;

        targetPos.y = transform.position.y;
        // オブジェクトを変数 targetPos の座標方向に向かせる
        transform.LookAt(targetPos);

        float distance = Vector3.Distance(transform.position, _neartarget.transform.position);
        // オブジェクトとターゲットオブジェクトの距離判定
        // 変数 distance（ターゲットオブジェクトとオブジェクトの距離）が変数 moveDistance の値より小さければ
        if (distance <= _moveDistance)
        {
            // 変数 moveSpeed を乗算した速度でオブジェクトを前方向に移動する
            _rb.MovePosition(transform.position + this.transform.forward * _lookonspeed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            Destroy(this.gameObject);
    }
}
