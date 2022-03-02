using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    private void OnParticleCollision(GameObject obj)
    {
        if (obj.tag == "Enemy")
        {
            EnemyBase enemy = obj.gameObject.GetComponent<EnemyBase>();
            if (enemy)
            {
                enemy._stateMode = EnemyBase.State.Hit;
            }
        }
        else if (obj.tag == "Boss")
        {
            var boss = GameObject.FindGameObjectWithTag("BossDragon").GetComponent<BossController>();
            boss._stateMode = BossController.State.Hit;
        }
    }
}
