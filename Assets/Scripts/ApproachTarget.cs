using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class ApproachTarget : MonoBehaviour
{
    [SerializeField] float _lifetime = 3f;
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _upspeed = 3f;
    [SerializeField] float _lookonSpeed = 5f;
    [SerializeField] float _moveDistance;
    [SerializeField] Vector3 _gettargetsRangeCenter = default;
    /// <summary>敵のターゲットロックできる範囲の半径</summary>
    [SerializeField] float _targetsRangeRadius = 1f;
    [SerializeField] GameObject _hitEff = default;
    [SerializeField] GameObject _createIce = default;
    List<Collider> _enemyList = new List<Collider>();
    GameObject[] _targets = default;
    Rigidbody _rb = default;

    public List<Collider> EnemyList { get => _enemyList; set => _enemyList = value; }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        _rb.velocity = this.transform.forward * _moveSpeed;
        Destroy(gameObject, _lifetime);
    }
    void Update()
    {
        _rb.AddForce(Vector3.up * _upspeed);

        EnemyList = Physics.OverlapSphere(GetTargetsRangeCenter(), _targetsRangeRadius).Where(t => t.tag == "Enemy").ToList();
        if (EnemyList != null)
        {
            foreach (var c in EnemyList)
            {
                if (c.gameObject.tag == "Enemy")
                {
                    c.GetComponent<Rigidbody>().isKinematic = true;
                    if (c.gameObject.GetComponent<NavMeshAgent>())
                        c.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                    c.gameObject.GetComponent<Rigidbody>().useGravity = true;
                    c.transform.position = Vector3.MoveTowards(c.transform.position, transform.position, _lookonSpeed * Time.deltaTime);
                }
            }
        }

        //GameObject _neartarget = _targets?.OrderBy(t => Vector3.Distance(t.transform.position, this.transform.position)).FirstOrDefault();

        //if (_neartarget != null)
        //{
        //    Vector3 targetPos = _neartarget.transform.position;

        //    targetPos.y = transform.position.y;
        //    // オブジェクトを変数 targetPos の座標方向に向かせる
        //    transform.LookAt(targetPos);

        //    float distance = Vector3.Distance(transform.position, _neartarget.transform.position);
        //    // オブジェクトとターゲットオブジェクトの距離判定
        //    // 変数 distance（ターゲットオブジェクトとオブジェクトの距離）が変数 moveDistance の値より小さければ
        //    if (distance <= _moveDistance)
        //    {
        //        // 変数 lookonSpeed を乗算した速度でオブジェクトを移動させる
        //        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, _lookonspeed * Time.deltaTime);
        //    }
        //}

    }
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetTargetsRangeCenter(), _targetsRangeRadius);
    }
    Vector3 GetTargetsRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _gettargetsRangeCenter.z
            + this.transform.up * _gettargetsRangeCenter.y
            + this.transform.right * _gettargetsRangeCenter.x;
        return center;
    }
    private void OnDestroy()
    {
        Instantiate(_createIce, transform.position, Quaternion.identity);
        FindObjectOfType<EnemyIskinematicOff>().ListTarget(EnemyList);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy" && other.tag != "Player")
        {
            Instantiate(_hitEff, transform.position, Quaternion.identity);
            Instantiate(_createIce, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
