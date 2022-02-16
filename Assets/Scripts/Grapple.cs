using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class Grapple : MonoBehaviour
{
    Vector3 _grapplePoint;
    Vector3 _currentGrapplePosition;
    [SerializeField] LayerMask _whatIsGrappleable;
    [SerializeField] Transform _gunTip, _player;
    [SerializeField] float _maxDistance = 20f;
    [SerializeField] float _grapplrPointHeight = 2f;
    [SerializeField] float _grapplrPointDis = 5;
    [SerializeField] float _canceljointDis = 0.5f;
    bool _isCanceljoint = false;
    Collider _grappleHandlePos;
    ConfigurableJoint _joint;
    PlayerController _playercon;
    LineRenderer _lr;

    public ConfigurableJoint Joint { get => _joint; set => _joint = value; }

    void Awake()
    {
        _lr = GetComponent<LineRenderer>();
        _playercon = GetComponent<PlayerController>();
    }

    void Update()
    {
        _grappleHandlePos = _playercon?.GrapplePoints?.Where(g => g.tag == "GrapplePos")
            .OrderBy(g => Vector3.Distance(g.transform.position, transform.position)).ToList().FirstOrDefault();

        if ((Vector3.Distance(_gunTip.position, _grapplePoint) < _canceljointDis && !_isCanceljoint) || (Input.GetKeyUp(KeyCode.F) && _isCanceljoint))
        {
            if (_isCanceljoint)
            {
                _isCanceljoint = false;
            }
            StopGrapple();
        }
        Debug.DrawLine(_gunTip.position, _player.position + (_player.up * _grapplrPointHeight + _player.forward * _grapplrPointDis), Color.red);
    }

    //Called after Update
    void LateUpdate()
    {
        DrawRope();
    }

    /// <summary>
    /// Call whenever we want to start a grapple
    /// </summary>
    public void StartGrapple()
    {
        RaycastHit hit;
        if (_grappleHandlePos)
        {
            _grapplePoint = _grappleHandlePos.transform.position;
            _isCanceljoint = true;
        }
        else if (Physics.Linecast(_gunTip.position, _player.position + (_player.up * _grapplrPointHeight + _player.forward * _grapplrPointDis), out hit))
        {
            _grapplePoint = hit.point;
        }
        else
        {
            _grapplePoint = _player.position + (_player.up * _grapplrPointHeight + _player.forward * _grapplrPointDis);
        }
        _joint = _player.gameObject.AddComponent<ConfigurableJoint>();
        if (_joint && !_grappleHandlePos)
        {
            StartCoroutine(DestroyJoint());
        }

        StartCoroutine(DelayMethod(0.4f, () =>
        {
            if (!_joint) return;
            SoftJointLimitSpring SoftJointLimitSpring = _joint.linearLimitSpring;
            SoftJointLimit SoftJointLimit = _joint.linearLimit;
            JointDrive jointxDrive = _joint.xDrive;
            JointDrive jointyDrive = _joint.xDrive;
            JointDrive jointzDrive = _joint.zDrive;

            _joint.autoConfigureConnectedAnchor = false;
            _joint.anchor = new Vector3(0, 0.8f, 0);
            _joint.connectedAnchor = _grapplePoint;

            _joint.xMotion = ConfigurableJointMotion.Limited;
            _joint.yMotion = ConfigurableJointMotion.Limited;
            _joint.zMotion = ConfigurableJointMotion.Limited;
            if (_grappleHandlePos)
            {
                SoftJointLimitSpring.spring = 30f;
                SoftJointLimit.limit = 1f;
                jointyDrive.maximumForce = 0;
                jointyDrive.positionDamper = 100;
            }
            else
            {
                SoftJointLimitSpring.spring = 5;
                SoftJointLimit.limit = 0.1f;
                jointyDrive.positionDamper = 50;
            }
            jointxDrive.positionSpring = 200;
            jointyDrive.positionSpring = 200;
            jointzDrive.positionSpring = 200;

            _joint.linearLimitSpring = SoftJointLimitSpring;
            _joint.linearLimit = SoftJointLimit;
            _joint.xDrive = jointxDrive;
            _joint.yDrive = jointyDrive;
            _joint.zDrive = jointzDrive;
        }));
        _lr.positionCount = 2;
        _currentGrapplePosition = _gunTip.position;
    }


    /// <summary>
    /// Call whenever we want to stop a grapple
    /// </summary>
    void StopGrapple()
    {
        _lr.positionCount = 0;
        Destroy(_joint);
    }



    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!_joint) return;

        _currentGrapplePosition = Vector3.Lerp(_currentGrapplePosition, _grapplePoint, Time.deltaTime * 4f);
        if (_lr.positionCount != 0)
        {
            _lr.SetPosition(0, _gunTip.position);
            _lr.SetPosition(1, _currentGrapplePosition);
        }
    }

    public bool IsGrappling()
    {
        return _joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return _grapplePoint;
    }
    IEnumerator DestroyJoint()
    {
        if (!_joint)
        {
            yield break;
        }
        yield return new WaitForSeconds(1.0f);
        StopGrapple();
    }
    public IEnumerator DelayMethod(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();
    }
}
