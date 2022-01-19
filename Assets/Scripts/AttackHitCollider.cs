using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitCollider : MonoBehaviour
{
    [SerializeField] float _attackRangeRadius = 1f;
    [SerializeField] Vector3 _attackRangeCenter = default;
    public enum Action
    {
        NomalAttack,
        BigAttack
    }
    public Action _hitmode;
    // Start is called before the first frame update
    void Start()
    {
        var hit = Physics.OverlapSphere(GetAttackRangeCenter(), _attackRangeRadius);
        foreach (var c in hit)
        {
            EnemyBase enemy = c.gameObject.GetComponent<EnemyBase>();
            EnemyHPBar Ehp = c.gameObject.GetComponent<EnemyHPBar>();

            if (enemy)
            {
                if (_hitmode == Action.BigAttack)
                {
                    enemy.mode = EnemyBase.Action.BHit;
                    Ehp.Damage(40, 50);
                }
                else
                    enemy.mode = EnemyBase.Action.Hit;

            }
        }
    }
    void OnDrawGizmosSelected()
    {
        // 攻撃範囲を赤い線でシーンビューに表示する
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetAttackRangeCenter(), _attackRangeRadius);

    }
    Vector3 GetAttackRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackRangeCenter.z
            + this.transform.up * _attackRangeCenter.y
            + this.transform.right * _attackRangeCenter.x;
        return center;
    }
}
