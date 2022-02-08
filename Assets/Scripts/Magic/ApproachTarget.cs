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
    [SerializeField] GameObject _createIce = default;
    float _timer = 0;
    List<Collider> _enemyList = new List<Collider>();
    GameObject _world = default;
    GameObject[] _targets = default;
    Rigidbody _rb = default;

    public List<Collider> EnemyList { get => _enemyList; set => _enemyList = value; }

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _world = GameObject.FindGameObjectWithTag("World");
        _targets = GameObject.FindGameObjectsWithTag("Enemy");
        _rb.velocity = this.transform.forward * _moveSpeed;
    }
    void Update()
    {
        _rb.AddForce(Vector3.up * _upspeed);

        _timer += Time.deltaTime;
        if (_timer > _lifetime)
        {
            Destroy(gameObject);
        }

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
        var obj = Instantiate(_createIce, transform.position, Quaternion.identity);
        obj.transform.parent = _world.transform;
        FindObjectOfType<EnemyIskinematicOff>().ListTarget(EnemyList);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Enemy" && other.tag != "Player")
        {
            Destroy(this.gameObject);
        }
    }
}
