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
    Rigidbody _rb = default;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        _rb.velocity = this.transform.forward * _movespeed;
        Destroy(gameObject, _lifetime);
    }
    void Update()
    {
        GameObject _neartarget = _targets?.OrderBy(t => Vector3.Distance(t.transform.position, this.transform.position)).FirstOrDefault();

        if (_neartarget != null)
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
                // 変数 lookonSpeed を乗算した速度でオブジェクトを移動させる
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, _lookonspeed * Time.deltaTime);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
            Destroy(this.gameObject);
    }
}
