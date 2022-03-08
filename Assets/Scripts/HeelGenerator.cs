using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeelGenerator : MonoBehaviour
{
    float _timer = 0;
    [SerializeField] float _generateTime = 10;
    [SerializeField] GameObject _generatePoint = default;
    int _heelCount = 0;
    [SerializeField] GameObject _heelBall;
    // Update is called once per frame
    void Update()
    {
        _heelCount = this.transform.childCount;
        if (_heelCount == 1)
        {
            _timer += Time.deltaTime;
            if (_timer >= _generateTime)
            {
                var _obj = Instantiate(_heelBall, _generatePoint.transform.position, Quaternion.identity);
                _obj.transform.parent = this.transform;
                _timer = 0;
            }
        }
    }
}
