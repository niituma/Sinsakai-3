using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    /// <summary>rayなどを表示/非表示にする</summary>
    [SerializeField] bool _debug = default;
    [Header("Player")]
    [SerializeField] float _movePower = 3;
    [SerializeField] float _dashmovePower = 6;
    [SerializeField] float _skillDashPower = 6;
    [SerializeField] float _skillDashLimitTime = 5f;
    [SerializeField] float _jumpPower = 3;
    [SerializeField] float _gravityPower = 0.3f;
    [SerializeField] float _turnSpeed = 8.0f;
    [SerializeField] float _isGroundedLength = 1.1f;
    [SerializeField] GameObject _rockAimEff = default;
    [SerializeField] GameObject _shockWave = default;
    [SerializeField] GameObject _speedRightFoot = default;
    [SerializeField] GameObject _speedLeftFoot = default;
    bool _rockGunOn = default;
    bool _isrockAimEff = false;
    bool _isattacklockdir = default;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    Vector3 _dir;
    bool _isSkillDash = default;
    float _skillDashTime = 0;
    float h, v;
    public float _avdTime;
    float _animationspeed;
    float _attackanimationspeedx;
    float _attackanimationspeedy;
    int _magicModeIndex = 0;
    bool _stopmovedir = default;
    public bool _ishit = default;
    bool _isjump = default;

    [Header("索敵、攻撃範囲")]
    [SerializeField] Vector3 _gettargetsRangeCenter = default;
    /// <summary>敵のターゲットロックできる範囲の半径</summary>
    [SerializeField] float _targetsRangeRadius = 1f;

    [Header("ターゲットロック")]
    [SerializeField] CinemachineVirtualCamera _mousecamera;
    [SerializeField] CinemachineVirtualCamera _targetcamera;
    [SerializeField] GameObject _crosshaircanvas;
    [SerializeField] float _changeVerticalAxisValue = 5f;
    GameObject _crosshair;
    CinemachinePOV aim;
    CinemachineFramingTransposer zoom;
    CinemachineFramingTransposer zoom2;
    bool _oncamerachangedir = default;
    public bool _lookon = default;
    [SerializeField] public List<Collider> _currentenemy = new List<Collider>();

    [Header("ClimdController")]
    [SerializeField] GameObject _handleSachcollider = default;
    [SerializeField] GameObject _handleSachcollider2 = default;
    [SerializeField] GameObject _handleSachcollider3 = default;
    bool _isclimd = default;
    bool _isHandleSarch = default;
    bool _isAttackIK = default;
    [SerializeField] float _raydis = 0.5f;
    Vector3 raydir = default;
    [SerializeField] GameObject _climdUpRay = default;
    [SerializeField] GameObject _climdDownRay = default;
    [SerializeField] private GameObject LHand;
    [SerializeField] private GameObject RHand;
    bool _isLover = default;
    bool _isRover = default;
    [SerializeField] Vector3 curOriginGrabOffset = new Vector3(0, 1.2f, 0);
    [SerializeField] Vector3 _wallSarchRayOffset = new Vector3(0, 1.6f, 0);
    [SerializeField] Vector3 _SarchRayOffset = new Vector3(4f, 0, 0);
    [SerializeField] Vector3 _SarchRayOffset2 = new Vector3(-1.2f, 0, 0);


    ControllerSystem _input;
    TargetLookOn target;
    PlayerMagic _magic;
    PlayerHP _hp;
    ClimdIK _climdIK;
    AttackAimIK _aimIK;
    Rigidbody _rb = default;
    Animator _anim = default;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _climdIK = GetComponent<ClimdIK>();
        _aimIK = GetComponent<AttackAimIK>();
        _magic = GetComponent<PlayerMagic>();
        _anim = GetComponent<Animator>();
        _hp = GetComponent<PlayerHP>();
        _input = GetComponent<ControllerSystem>();
        zoom = _mousecamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        zoom2 = _targetcamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        aim = _mousecamera.GetCinemachineComponent<CinemachinePOV>();
        _crosshair = GameObject.Find("CrossHair");
        target = _crosshaircanvas.GetComponent<TargetLookOn>();
    }

    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");
        _dir = new Vector3(h, 0, v);
        _dir = Camera.main.transform.TransformDirection(_dir);
        // カメラは斜め下に向いているので、Y 軸の値を 0 にして「XZ 平面上のベクトル」にする
        _dir.y = 0;
        // キャラクターを「現在の（XZ 平面上の）進行方向」に向ける
        if (_input.move != Vector2.zero && !_stopmovedir && !_isclimd && !_rockGunOn)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
        }

        if (_lookon && _input.move == Vector2.zero && !_isclimd && !_stopmovedir || _isattacklockdir)
        {
            var direction = _crosshaircanvas.transform.position - transform.position;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
        }

        Vector3 velo = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.x);
        velo.y = _rb.velocity.y;
        _rb.velocity = velo;
        if (Input.GetKeyDown(KeyCode.C) && _isclimd)
        {
            _anim.CrossFade("Braced Jump From Wall", 0.2f);
            _stopmovedir = true;
            _isclimd = false;
        }
        else if (Input.GetKeyDown(KeyCode.C) && !_isclimd && _input.move != Vector2.zero && !_isSkillDash && !_rockGunOn)
        {
            _isSkillDash = true;
            Instantiate(_shockWave, transform.position + new Vector3(0, 0.8f, 0), Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            _magicModeIndex++;
            _magic.MagicMode = (PlayerMagic.Action)(_magicModeIndex % System.Enum.GetNames(typeof(PlayerMagic.Action)).Length);
            if (_magicModeIndex > 2)
                _magicModeIndex = 0;
        }
        if (_isSkillDash)
        {
            _speedRightFoot.SetActive(true);
            _speedLeftFoot.SetActive(true);
            DOTween.To(() => zoom.m_CameraDistance, num => zoom.m_CameraDistance = num, 3.5f, 0.5f);
            _skillDashTime += Time.deltaTime;
            if (_skillDashTime >= _skillDashLimitTime || _input.move == Vector2.zero || _rockGunOn || _stopmovedir)
            {
                _isSkillDash = false;
                _speedRightFoot.SetActive(false);
                _speedLeftFoot.SetActive(false);
                DOTween.To(() => zoom.m_CameraDistance, num => zoom.m_CameraDistance = num, 2.5f, 0.5f);
                _skillDashTime = 0;
            }
        }
        _isjump = _input.jump;
        RockAttack();
        TargetLookOn();
        Climb();
        Jump();
        Targets();
    }
    private void LateUpdate()
    {

        _anim.SetBool("Climb", _isclimd);
        _anim.SetBool("Grounded", IsGrounded());
        if (!_isclimd)
            _anim.SetBool("Jump", _isjump);
        _anim.SetBool("Combo", _magic.Iscombo);
        _anim.SetBool("Hit", _ishit);
        _anim.SetBool("Avoidance", _input.avd);
        if (_ishit)
        {
            _hp.Damage();
            _ishit = false;
        }

        if (_input.avd)
        {
            _input.avd = false;
        }
    }

    void FixedUpdate()
    {
        Move();
    }
    void Move()
    {
        float _targetSpeed;

        if (_isclimd)
        {
            _targetSpeed = h;
            if (_isLover)
            {
                if (h < 0)
                    _targetSpeed = 0;
            }
            else if (_isRover)
            {
                if (h > 0)
                    _targetSpeed = 0;
            }
        }
        else
        {
            if (_isSkillDash)
            {
                _targetSpeed = _skillDashPower;
            }
            else if (_rockGunOn)
            {
                _targetSpeed = _movePower;
            }
            else
                _targetSpeed = _input.sprint ? _dashmovePower : _movePower;

        }

        // 「力を加える」処理は力学的処理なので FixedUpdate で行うこと

        if (_input.move == Vector2.zero)
        {
            _targetSpeed = 0.0f;
        }

        if (!_stopmovedir)
            _animationspeed = Mathf.Lerp(_animationspeed, _targetSpeed, Time.deltaTime * 10f);
        else
            _animationspeed = 0;

        if (!_isclimd)
        {
            if (!_stopmovedir)
                _rb.AddForce(_dir.normalized * _targetSpeed, ForceMode.Force);

            _anim.SetFloat("Speed", _animationspeed);
        }
        else
        {
            _anim.SetFloat("ClimbMoveSpeed", _animationspeed);
        }
        _attackanimationspeedx = Mathf.Lerp(_attackanimationspeedx, h, Time.deltaTime * 10f);
        _attackanimationspeedy = Mathf.Lerp(_attackanimationspeedy, v, Time.deltaTime * 10f);
        _anim.SetFloat("X", _attackanimationspeedx);
        _anim.SetFloat("Y", _attackanimationspeedy);

    }

    void OnDrawGizmosSelected()
    {
        if (_debug)
        {
            // 攻撃範囲を赤い線でシーンビューに表示する
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(GetTargetsRangeCenter(), _targetsRangeRadius);
        }
    }
    Vector3 GetTargetsRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _gettargetsRangeCenter.z
            + this.transform.up * _gettargetsRangeCenter.y
            + this.transform.right * _gettargetsRangeCenter.x;
        return center;
    }
    void RockAttack()
    {
        if (_magic.MagicMode == PlayerMagic.Action.Earth)
        {
            if (_rockGunOn)
            {
                transform.rotation = Quaternion.Euler(0, Camera.main.transform.transform.localEulerAngles.y, 0);
            }
            _rockGunOn = _input.aim;

            if (_rockGunOn && _aimIK.IsAimChange)
            {
                _aimIK.chageAim(1f, 0.5f);
                DOTween.To(() => zoom.m_CameraDistance, num => zoom.m_CameraDistance = num, 1.5f, 0.5f);
                DOTween.To(() => zoom2.m_CameraDistance, num => zoom2.m_CameraDistance = num, 1.5f, 0.5f);
            }
            else if (!_rockGunOn && !_aimIK.IsAimChange)
            {
                _aimIK.chageAim(0f, 0.5f);
                DOTween.To(() => zoom.m_CameraDistance, num => zoom.m_CameraDistance = num, 2.5f, 0.5f);
                DOTween.To(() => zoom2.m_CameraDistance, num => zoom2.m_CameraDistance = num, 2f, 0.5f);
            }

            if (_rockGunOn)
            {
                if (!_isrockAimEff)
                {
                    _rockAimEff.SetActive(true);
                    _isrockAimEff = true;
                }
                if (_magic.Ammo > 0 && _input.shoot)
                {
                    StartCoroutine(_magic.ShootTimer());
                }
            }
            else
            {
                if (_isrockAimEff)
                {
                    _rockAimEff.SetActive(false);
                    _isrockAimEff = false;
                }
            }
            _anim.SetBool("RockGun", _input.aim);
        }
    }

    void Targets()
    {
        _currentenemy = FilterTargetObject(Physics.OverlapSphere(GetTargetsRangeCenter(), _targetsRangeRadius).ToList());
    }
    protected List<Collider> FilterTargetObject(List<Collider> detection)
    {
        return detection.Where(h => h.tag == "Enemy").Where(h =>
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(h.transform.position);
            return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > -0.25 && screenPoint.y < 0.9;
        }).ToList();
    }
    void Climb()
    {
        Vector3 origin = _climdUpRay.transform.position;// 原点
        Vector3 origin2 = LHand.transform.position;
        Vector3 origin3 = RHand.transform.position;
        Vector3 origin4 = transform.position + _wallSarchRayOffset;
        Vector3 origin5 = _climdDownRay.transform.position;
        origin2.y = transform.position.y + curOriginGrabOffset.y;
        origin3.y = origin2.y;
        raydir = transform.forward + new Vector3(0, 10, 0); // X軸方向を表すベクトル
        Vector3 raydir2 = transform.forward;

        Ray ray = new Ray(origin, raydir); // Rayを生成
        Ray ray2 = new Ray(origin2, raydir2);
        Ray ray3 = new Ray(origin3, raydir2);
        Ray ray4 = new Ray(origin4, raydir2);
        Ray ray5 = new Ray(origin5, raydir2 + new Vector3(0, -90, 0));
        Ray ray6 = new Ray(origin2, raydir2 + _SarchRayOffset);
        Ray ray7 = new Ray(origin3, raydir2 + _SarchRayOffset2);
        Debug.DrawRay(ray.origin, ray.direction * _raydis, Color.blue); // 長さ３０、赤色で可視化
        Debug.DrawRay(ray4.origin, ray4.direction * _raydis, Color.blue);
        if (_isclimd)
        {
            Debug.DrawRay(ray2.origin, ray2.direction * _raydis, Color.blue);
            Debug.DrawRay(ray3.origin, ray3.direction * _raydis, Color.blue);
            Debug.DrawRay(ray5.origin, ray5.direction * _raydis, Color.blue);
            Debug.DrawRay(ray6.origin, ray6.direction * _raydis, Color.blue);
            Debug.DrawRay(ray7.origin, ray7.direction * _raydis, Color.blue);
            if (_isAttackIK)
            {
                GetComponent<AttackAimIK>().enabled = false;
                _isAttackIK = false;
            }
        }
        else
        {
            if (!_isAttackIK)
            {
                GetComponent<AttackAimIK>().enabled = true;
                _isAttackIK = true;
            }
        }

        RaycastHit hit;
        RaycastHit hit2;
        // レイヤーの管理番号を取得
        int layerNo = LayerMask.NameToLayer("Handle");
        // マスクへの変換（ビットシフト）
        int layerMask = 1 << layerNo;
        // レイヤーの管理番号を取得
        int layerNo2 = LayerMask.NameToLayer("Ground");
        // マスクへの変換（ビットシフト）
        int layerMask2 = 1 << layerNo2;
        ;
        if (Physics.Raycast(ray, out hit, _raydis, layerMask))
        {
            if (hit.collider.tag == "Handle" && !_isclimd)
            {
                if (_input.jump)
                {
                    _isclimd = true;
                }
            }
            else if (_isclimd)
            {
                if (_input.jump && v > 0)
                    _anim.SetBool("NextUpClimb", true);
                else
                    _anim.SetBool("NextUpClimb", false);
            }
        }
        if (Physics.Raycast(ray5, out hit, _raydis, layerMask))
        {
            if (_isclimd)
            {
                if (_input.jump && v < 0)
                {
                    _anim.SetBool("NextDownClimb", true);
                }
                else
                {
                    _anim.SetBool("NextDownClimb", false);
                }
            }
        }
        if (_isclimd)
        {

            if (Physics.Raycast(ray2, out hit, _raydis))
            {
                if (hit.collider.tag != "Handle")
                {
                    _isLover = true;
                    if (Physics.Raycast(ray6, out hit, _raydis, layerMask))
                    {
                        if (hit.collider.tag == "Handle" && h < 0 && _input.jump)
                        {
                            _anim.CrossFade("Braced Hang Hop Left", 0.2f);
                        }
                    }

                }
                else
                    _isLover = false;
            }
            if (Physics.Raycast(ray3, out hit, _raydis))
            {
                if (hit.collider.tag != "Handle")
                {
                    _isRover = true;
                    if (Physics.Raycast(ray7, out hit, _raydis, layerMask))
                    {
                        if (hit.collider.tag == "Handle" && h > 0 && _input.jump)
                        {
                            _anim.CrossFade("Braced Hang Hop Right", 0.2f);
                        }
                    }
                }
                else
                    _isRover = false;
            }
            if (!Physics.Raycast(ray4, out hit, _raydis, layerMask2))
            {
                if (_input.jump && v > 0)
                {
                    if (_isclimd)
                    {
                        _isclimd = false;
                        _stopmovedir = true;
                    }
                }
            }
            if (h == 0)
            {
                if (Physics.Raycast(ray2, out hit, _raydis, layerMask) && Physics.Raycast(ray3, out hit2, _raydis, layerMask) && !_isHandleSarch)
                {
                    _climdIK._leftHandTarget.position = hit.point;
                    _climdIK._rightHandTarget.position = hit2.point;
                    _climdIK.ChangeWeight(1f, 1);
                }
            }
            else
            {
                _climdIK.ChangeWeight(0, 1);
            }
        }
        else
        {
            _climdIK.ChangeWeight(0, 1);
        }
    }
    public void GrabLedge(Vector3 handPos, Transform yrot)
    {
        if (_isclimd)
        {
            _rb.isKinematic = true;
            transform.rotation = yrot.rotation;
            this.transform.DOMove(handPos, 1f);
        }
    }
    void HandleSarchSwitch(int UpDown)
    {
        _isHandleSarch = !_isHandleSarch;
        _climdIK.ChangeWeight(0, 1);
        switch (UpDown)
        {
            case 1:
                _handleSachcollider.SetActive(true);
                _handleSachcollider2.SetActive(false);
                _handleSachcollider3.SetActive(false);
                break;
            case 2:
                _handleSachcollider.SetActive(false);
                _handleSachcollider2.SetActive(true);
                break;
            case 3:
                _handleSachcollider.SetActive(false);
                _handleSachcollider3.SetActive(true);
                break;
            default:
                break;
        }
    }

    void TargetLookOn()
    {
        if (target.isneartarget)
        {
            if (_input.lockon)
            {
                _lookon = !_lookon;
                if (_lookon)
                {
                    target.targeton = false;
                    _targetcamera.Priority = 100;
                    _oncamerachangedir = true;
                }
                else
                {
                    _targetcamera.Priority = 9;
                    target.targeton = true;
                    aim.m_HorizontalAxis.Value = _targetcamera.transform.localEulerAngles.y;
                    aim.m_VerticalAxis.Value = _changeVerticalAxisValue;
                }
                _input.lockon = false;
            }
        }
        else
        {
            _input.lockon = false;
            if (_lookon)
            {
                _targetcamera.Priority = 9;
                _lookon = false;

                if (_oncamerachangedir)
                {
                    aim.m_HorizontalAxis.Value = _targetcamera.transform.localEulerAngles.y;
                    aim.m_VerticalAxis.Value = _changeVerticalAxisValue;
                    _oncamerachangedir = false;
                }
            }
            target.targeton = true;
        }


        if (_input.change)
        {
            target.ChangeTarget();
            _input.change = false;
        }
        _crosshair.SetActive(_lookon);
    }
    void Jump()
    {
        if (!_isclimd)
        {
            Vector3 velosity = _rb.velocity;
            if (IsGrounded())
            {
                if (_input.jump)
                {
                    _rb.AddForce(Vector3.up * _jumpPower, ForceMode.Impulse);
                }
                else if (!_input.jump && velosity.y > 0)
                    velosity.y *= _gravityPower;
            }

            _rb.velocity = velosity;
        }
        if (_input.jump)
            _input.jump = false;
    }
    bool IsGrounded()
    {
        // Physics.Linecast() を使って足元から線を張り、そこに何かが衝突していたら true とする
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        Vector3 start = this.transform.position + col.center;   // start: 体の中心
        Vector3 end = start + Vector3.down * _isGroundedLength;  // end: start から真下の地点
        Debug.DrawLine(start, end); // 動作確認用に Scene ウィンドウ上で線を表示する
        bool isGrounded = Physics.Linecast(start, end); // 引いたラインに何かがぶつかっていたら true とする
        return isGrounded;
    }
    void StopMoveSwitch(int movenum)
    {
        switch (movenum)
        {
            case 1:
                _stopmovedir = true;
                break;
            case 2:
                _stopmovedir = false;
                break;
            case 3:
                _rb.isKinematic = true;
                _stopmovedir = true;
                break;
            case 4:
                _rb.isKinematic = false;
                _stopmovedir = false;
                break;
            default:
                Debug.LogWarning("movenumが指定の範囲外です。Animationのイベントから指定してください。");
                break;
        }
    }
    void LockAttack()
    {
        if (_lookon)
            _isattacklockdir = !_isattacklockdir;
    }
    public void TargetOff(float hp)
    {
        if (hp <= 0)
        {
            _input.lockon = false;
            if (_lookon)
            {
                _targetcamera.Priority = 9;
                _crosshair.SetActive(false);
            }
            target.targeton = true;
            if (_oncamerachangedir)
            {
                aim.m_HorizontalAxis.Value = _targetcamera.transform.localEulerAngles.y;
                aim.m_VerticalAxis.Value = _changeVerticalAxisValue;
                _oncamerachangedir = false;
            }
        }
    }
}

