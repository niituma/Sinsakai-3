using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject _enemyObj = default;
    [SerializeField] float _generateTime = 3f;
    [SerializeField] int _maxEnemy = 5;
    [SerializeField] float _radius = 3;
    [SerializeField] Transform central;
    float _timer = 0;

    // Update is called once per frame
    void Update()
    {
        int _childCount = this.transform.childCount;

        if (_childCount < _maxEnemy)
        {
            _timer += Time.deltaTime;

            if (_generateTime < _timer)
            {
                var enemy = Instantiate(_enemyObj, GeneratePoint(), GenerateRotation());
                enemy.transform.parent = this.transform;
                _timer = 0;
            }
        }
    }
    Vector3 GeneratePoint()
    {
        //目標地点のX軸、Z軸をランダムで決める
        float posX = Random.Range(-1 * _radius, _radius);
        float posZ = Random.Range(-1 * _radius, _radius);

        //CentralPointの位置にPosXとPosZを足す
        Vector3 pos = central.position;
        pos.x += posX;
        pos.z += posZ;
        return pos;
    }
    Quaternion GenerateRotation()
    {
        float rotY = Random.Range(0, 350);
        Quaternion enemyrot = Quaternion.Euler(0, rotY, 0);
        return enemyrot;
    }
}
