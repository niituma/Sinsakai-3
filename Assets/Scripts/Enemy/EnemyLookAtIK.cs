using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyLookAtIK : EnemyBase
{
    /// <summary>どれくらい見るか</summary>
    [SerializeField, Range(0f, 1f)] float _weight = 0;
    /// <summary>身体をどれくらい向けるか</summary>
    [SerializeField, Range(0f, 1f)] float _bodyWeight = 0;
    /// <summary>頭をどれくらい向けるか</summary>
    [SerializeField, Range(0f, 1f)] float _headWeight = 0;
    /// <summary>目をどれくらい向けるか</summary>
    [SerializeField, Range(0f, 1f)] float _eyesWeight = 0;
    /// <summary>関節の動きをどれくらい制限するか</summary>
    [SerializeField, Range(0f, 1f)] float _clampWeight = 0;
    Animator _anim = default;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // LookAt の重みとターゲットを指定する
        if (player)
        {
            _anim.SetLookAtWeight(_weight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight);
            _anim.SetLookAtPosition(player.transform.position + new Vector3(0, 1, 0));
        }
    }
}
