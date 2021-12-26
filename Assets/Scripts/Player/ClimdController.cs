using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClimdController : MonoBehaviour
{
    //float _ph, _pv;
    //public bool _isclimd = default;
    //public bool _isHandleSarch = default;
    //[SerializeField] float _raydis = 0.5f;
    //Vector3 raydir = default;


    //[SerializeField] GameObject _handleSachcollider = default;
    //[SerializeField] GameObject _handleSachcollider2 = default;
    //[SerializeField] GameObject _handleSachcollider3 = default;
    //[SerializeField] GameObject _climdUpRay = default;
    //[SerializeField] GameObject _climdDownRay = default;
    //[SerializeField] private GameObject LHand;
    //[SerializeField] private GameObject RHand;
    //public bool _isLover = default;
    //public bool _isRover = default;
    //[SerializeField] Vector3 curOriginGrabOffset = new Vector3(0, 1.2f, 0);
    //[SerializeField] Vector3 _wallSarchRayOffset = new Vector3(0, 0, 0);
    //[SerializeField] Vector3 _SarchRayOffset = new Vector3(0, 0, 0);
    //[SerializeField] Vector3 _SarchRayOffset2 = new Vector3(0, 0, 0);
    //PlayerController _playercontrol;
    //ClimdIK _climdIK;
    //ControllerSystem _input;
    //Rigidbody _rb = default;
    //Animator _anim = default;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    _playercontrol = GetComponent<PlayerController>();
    //    _rb = GetComponent<Rigidbody>();
    //    _climdIK = GetComponent<ClimdIK>();
    //    _anim = GetComponent<Animator>();
    //    _input = GetComponent<ControllerSystem>();
    //    _ph = _playercontrol.h;
    //    _pv = _playercontrol.v;
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    bool up = _playercontrol._input.jump;
    //    if (up)
    //    {
    //        Debug.Log("aaa");
    //    }
    //    Climb();
    //}
    //void Climb()
    //{
    //    Vector3 origin = _climdUpRay.transform.position;// 原点
    //    Vector3 origin2 = LHand.transform.position;
    //    Vector3 origin3 = RHand.transform.position;
    //    Vector3 origin4 = transform.position + _wallSarchRayOffset;
    //    Vector3 origin5 = _climdDownRay.transform.position;
    //    origin2.y = transform.position.y + curOriginGrabOffset.y;
    //    origin3.y = origin2.y;
    //    raydir = transform.forward + new Vector3(0,20, 0); // X軸方向を表すベクトル
    //    Vector3 raydir2 = transform.forward;

    //    Ray ray = new Ray(origin, raydir); // Rayを生成
    //    Ray ray2 = new Ray(origin2, raydir2);
    //    Ray ray3 = new Ray(origin3, raydir2);
    //    Ray ray4 = new Ray(origin4, raydir2);
    //    Ray ray5 = new Ray(origin5, raydir2 + new Vector3(0, -90, 0));
    //    Ray ray6 = new Ray(origin2, raydir2 + _SarchRayOffset);
    //    Ray ray7 = new Ray(origin3, raydir2 + _SarchRayOffset2);
    //    Debug.DrawRay(ray.origin, ray.direction * _raydis, Color.blue); // 長さ３０、赤色で可視化
    //    Debug.DrawRay(ray4.origin, ray4.direction * _raydis, Color.blue);
    //    if (_isclimd)
    //    {
    //        Debug.DrawRay(ray2.origin, ray2.direction * _raydis, Color.blue);
    //        Debug.DrawRay(ray3.origin, ray3.direction * _raydis, Color.blue);
    //        Debug.DrawRay(ray5.origin, ray5.direction * _raydis, Color.blue);
    //        Debug.DrawRay(ray6.origin, ray6.direction * _raydis, Color.blue);
    //        Debug.DrawRay(ray7.origin, ray7.direction * _raydis, Color.blue);
    //    }

    //    RaycastHit hit;
    //    RaycastHit hit2;
    //    // レイヤーの管理番号を取得
    //    int layerNo = LayerMask.NameToLayer("Handle");
    //    // マスクへの変換（ビットシフト）
    //    int layerMask = 1 << layerNo;
    //    // レイヤーの管理番号を取得
    //    int layerNo2 = LayerMask.NameToLayer("Ground");
    //    // マスクへの変換（ビットシフト）
    //    int layerMask2 = 1 << layerNo2;
    //    ;
    //    if (Physics.Raycast(ray, out hit, _raydis, layerMask))
    //    {
    //        if (hit.collider.tag == "Handle" && !_isclimd)
    //        {
    //            if (_playercontrol._isjump)
    //            {
    //                Debug.Log("つかめる");
    //                _isclimd = true;
    //            }
    //        }
    //        else if (_isclimd)
    //        {
    //            if (_playercontrol._isjump && _pv > 0)
    //                _anim.SetBool("NextUpClimb", true);
    //            else
    //                _anim.SetBool("NextUpClimb", false);
    //        }
    //    }
    //    if (Physics.Raycast(ray5, out hit, _raydis, layerMask))
    //    {
    //        if (_isclimd)
    //        {
    //            if (_playercontrol._isjump && _pv < 0)
    //            {
    //                _anim.SetBool("NextDownClimb", true);
    //            }
    //            else
    //            {
    //                _anim.SetBool("NextDownClimb", false);
    //            }
    //        }
    //    }
    //    if (_isclimd)
    //    {

    //        if (Physics.Raycast(ray2, out hit, _raydis))
    //        {
    //            if (hit.collider.tag != "Handle")
    //            {
    //                _isLover = true;
    //                if (Physics.Raycast(ray6, out hit, _raydis, layerMask))
    //                {
    //                    if (hit.collider.tag == "Handle" && _ph < 0 && _playercontrol._isjump)
    //                    {
    //                        _anim.CrossFade("Braced Hang Hop Left", 0.2f);
    //                    }
    //                }

    //            }
    //            else
    //                _isLover = false;
    //        }
    //        if (Physics.Raycast(ray3, out hit, _raydis))
    //        {
    //            if (hit.collider.tag != "Handle")
    //            {
    //                _isRover = true;
    //                if (Physics.Raycast(ray7, out hit, _raydis, layerMask))
    //                {
    //                    if (hit.collider.tag == "Handle" && _ph > 0 && _playercontrol._isjump)
    //                    {
    //                        _anim.CrossFade("Braced Hang Hop Right", 0.2f);
    //                    }
    //                }
    //            }
    //            else
    //                _isRover = false;
    //        }
    //        if (!Physics.Raycast(ray4, out hit, _raydis, layerMask2))
    //        {
    //            if (_playercontrol._isjump && _pv > 0)
    //            {
    //                if (_isclimd)
    //                {
    //                    _isclimd = false;
    //                    _playercontrol._stopmovedir = true;
    //                }
    //            }
    //        }
    //        if (_ph == 0)
    //        {
    //            if (Physics.Raycast(ray2, out hit, _raydis, layerMask) && Physics.Raycast(ray3, out hit2, _raydis, layerMask) && !_isHandleSarch)
    //            {
    //                _climdIK._leftHandTarget.position = hit.point;
    //                _climdIK._rightHandTarget.position = hit2.point;
    //                _climdIK.ChangeWeight(1f, 1);
    //            }
    //        }
    //        else
    //        {
    //            _climdIK.ChangeWeight(0, 1);
    //        }
    //    }
    //    else
    //    {
    //        _climdIK.ChangeWeight(0, 1);
    //    }
    //}
    //public void GrabLedge(Vector3 handPos, Transform yrot)
    //{
    //    if (_isclimd)
    //    {
    //        _rb.isKinematic = true;
    //        transform.rotation = yrot.rotation;
    //        this.transform.DOMove(handPos, 1f);
    //    }
    //}
    //void HandleSarchSwitch(int UpDown)
    //{
    //    _isHandleSarch = !_isHandleSarch;
    //    _climdIK.ChangeWeight(0, 1);
    //    switch (UpDown)
    //    {
    //        case 1:
    //            _handleSachcollider.SetActive(true);
    //            _handleSachcollider2.SetActive(false);
    //            _handleSachcollider3.SetActive(false);
    //            break;
    //        case 2:
    //            _handleSachcollider.SetActive(false);
    //            _handleSachcollider2.SetActive(true);
    //            break;
    //        case 3:
    //            _handleSachcollider.SetActive(false);
    //            _handleSachcollider3.SetActive(true);
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
