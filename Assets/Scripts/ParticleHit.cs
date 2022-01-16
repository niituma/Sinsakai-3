﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    private void OnParticleCollision(GameObject obj)
    {
        if(obj.tag == "Enemy")
        {
            EnemyBase enemy = obj.gameObject.GetComponent<EnemyBase>();
            if (enemy)
            {
                enemy.mode = EnemyBase.Action.Hit;
            }
        }
    }
}