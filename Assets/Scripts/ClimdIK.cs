using System.Collections;
using UnityEngine;

public class ClimdIK : MonoBehaviour
{
    /// <summary>「押す」時の右手の IK ターゲット</summary>
    [SerializeField] public Transform _rightHandTarget = default;
    /// <summary>「押す」時の左手の IK ターゲット</summary>
    [SerializeField] public Transform _leftHandTarget = default;
    /// <summary>右手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0;
    /// <summary>左手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _leftPositionWeight = 0;
    Animator _anim = default;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void OnAnimatorIK(int layerIndex)
    {
        // 右手に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, _rightHandTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, _rightHandTarget.rotation);
        // 左手に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, _leftPositionWeight);
        _anim.SetIKPosition(AvatarIKGoal.LeftHand, _leftHandTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.LeftHand, _leftHandTarget.rotation);
    }

    /// <summary>
    /// 指定した値にウェイトを変更する
    /// </summary>
    /// <param name="targetWeight"></param>
    //public void ChangeWeight(float targetrightpos, float targetleftpos)
    //{
    //    _rightPositionWeight = targetrightpos;
    //    _leftPositionWeight = targetleftpos;
    //}

    /// <summary>
    /// 指定した値にウェイトを step ずつ変更する
    /// </summary>
    /// <param name="targetWeight"></param>
    /// <param name="step"></param>
    public void ChangeWeight(float targetWeight, float step)
    {
        StartCoroutine(ChangeWeightRoutine(targetWeight, step));
    }

    /// <summary>
    /// 指定した値にウェイトを step ずつ変更する
    /// </summary>
    /// <param name="targetWeight"></param>
    /// <param name="step"></param>
    /// <returns></returns>
    IEnumerator ChangeWeightRoutine(float targetWeight, float step)
    {
        if (_rightPositionWeight < targetWeight)
        {
            while (_rightPositionWeight < targetWeight)
            {
                _rightPositionWeight += step;
                _leftPositionWeight = _rightPositionWeight;
                yield return null;
            }
        }
        else
        {
            while (_rightPositionWeight > targetWeight)
            {
                _rightPositionWeight -= step;
                _leftPositionWeight = _rightPositionWeight;
                yield return null;
            }
        }
    }
    
}
