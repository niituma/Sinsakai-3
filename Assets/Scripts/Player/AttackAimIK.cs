using System.Collections;
using UnityEngine;
using DG.Tweening;

public class AttackAimIK : MonoBehaviour
{
    [SerializeField] public Transform _aimRightHandTarget = default;
    [SerializeField] Transform _rockAimBody = default;
    /// <summary>右手の Position に対するウェイト</summary>
    [SerializeField, Range(0f, 1f)] float _rightPositionWeight = 0;
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
    bool _isAimChange = true;
    Animator _anim = default;

    public bool IsAimChange { get => _isAimChange; set => _isAimChange = value; }

    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        DOTween.SetTweensCapacity(tweenersCapacity: 800, sequencesCapacity: 200);
    }
    void OnAnimatorIK(int layerIndex)
    {
        // 右手に対して IK を設定する
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, _rightPositionWeight);
        _anim.SetIKPosition(AvatarIKGoal.RightHand, _aimRightHandTarget.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, _aimRightHandTarget.rotation);
        _anim.SetLookAtWeight(_weight, _bodyWeight, _headWeight, _eyesWeight, _clampWeight);
        _anim.SetLookAtPosition(_rockAimBody.transform.position + new Vector3(0, 1, 0));
    }
    public void chageAim(float targetWeight, float step)
    {

        DOTween.To(() => _bodyWeight, num => _bodyWeight = num, targetWeight, step);
        DOTween.To(() => _headWeight, num => _headWeight = num, targetWeight, step);
        if (targetWeight > 0)
        {
            DOTween.To(() => _weight, num => _weight = num, 0.3f, step);
        }
        else
        {
            DOTween.To(() => _weight, num => _weight = num, 0f, step);
        }
        DOTween.To(() => _rightPositionWeight, num => _rightPositionWeight = num, targetWeight, step)
            .OnComplete(() =>
            {
                if (targetWeight > 0)
                {
                    IsAimChange = false;
                }
                else
                {
                    IsAimChange = true;
                }
            });
    }
}
