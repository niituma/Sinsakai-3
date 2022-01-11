using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHit : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
    }
    private void OnParticleCollision(GameObject obj)
    {
        if(obj.tag == "Enemy")
        {
            EnemyR enemy = obj.gameObject.GetComponent<EnemyR>();
            if (enemy)
            {
                enemy.mode = EnemyBase.Action.Hit;
                enemy._ishit = true;
            }
        }
    }
}
