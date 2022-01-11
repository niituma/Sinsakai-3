using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AttackAimIK : MonoBehaviour
{
    [SerializeField] public Transform _aimRightHandTarget = default;
    /// <summary>右手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0;
    bool _isAimChange = true;
    Animator _anim = default;

    public bool IsAimChange { get => _isAimChange; set => _isAimChange = value; }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }
    void OnAnimatorIK(int layerIndex)
    {
        // 右手に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, _aimRightHandTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, _aimRightHandTarget.rotation);
    }
    public void chageAim(float targetWeight, float step)
    {

        DOTween.To(() => _rightPositionWeight, num => _rightPositionWeight = num, targetWeight, step)
            .OnComplete(() =>
            {
                if (targetWeight > 0)
                    IsAimChange = false;
                else
                    IsAimChange = true;
            });
    }
}
