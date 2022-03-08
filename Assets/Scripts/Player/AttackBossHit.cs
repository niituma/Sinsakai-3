using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBossHit : MonoBehaviour
{
    public enum Action
    {
        NomalAttack,
        BigAttack
    }
    public Action _hitmode;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss")
        {
            var boss = GameObject.FindGameObjectWithTag("BossDragon").GetComponent<EnemyHPBar>();
            if (_hitmode == Action.BigAttack)
            {
                boss.Damage(3, 5);
            }
            else
            {
                boss.Damage(8, 11);
            }

        }
    }
}
