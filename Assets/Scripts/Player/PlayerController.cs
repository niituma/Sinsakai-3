using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float _movePower = 3;
    [SerializeField] float _dashmovePower = 6;
    [SerializeField] float _jumpPower = 3;
    [SerializeField] float _gravityPower = 0.3f;
    [SerializeField] float _turnSpeed = 8.0f;
    [SerializeField] float _isGroundedLength = 1.1f;
    [SerializeField] float _magicCoolDownSpeed = 2f;
    [SerializeField] float _magiclimiter = 0f;
    [SerializeField] float _magiclimit = 100f;
    [SerializeField] Vector3 _gettargetsRangeCenter = default;
    /// <summary>敵のターゲットロックできる範囲の半径</summary>
    [SerializeField] float _targetsRangeRadius = 1f;
    [SerializeField] Vector3 _attackRangeCenter = default;
    /// <summary>攻撃範囲の半径</summary>
    [SerializeField] float _attackRangeRadius = 1f;
    [SerializeField] GameObject _magicoverparticle = default;
    ParticleSystem _overparticle = default;
    [SerializeField] GameObject _magiceff = default;
    [SerializeField] GameObject _rightattackmuzzle = default;
    [SerializeField] CinemachineVirtualCamera _mousecamera;
    [SerializeField] CinemachineVirtualCamera _targetcamera;
    [SerializeField] GameObject _crosshaircanvas;
    [SerializeField] float _changeVerticalAxisValue = 5f;
    GameObject _crosshair;
    [SerializeField] public List<Collider> _currentenemy = new List<Collider>();
    float h, v;
    public float _avdTime;
    float _animationspeed;
    bool _stopmovedir = default;
    bool _stopmove = default;
    bool _iscombo = default;
    public bool _ishit = default;
    bool _isjump = default;
    public bool _lookon = default;
    /// <summary>壁を検出するための ray のベクトル</summary>
    [SerializeField] Vector3 _rayForWall = Vector3.zero;
    [SerializeField] float _raydis = 0.5f;
    Vector3 raydir = default;

    CinemachinePOV aim;
    bool _oncamerachangedir = default;
    ControllerSystem _input;
    TargetLookOn target;
    PlayerHP _hp;
    Rigidbody _rb = default;
    Animator _anim = default;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    Vector3 _dir;


    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();
        _hp = GetComponent<PlayerHP>();
        _input = GetComponent<ControllerSystem>();
        _overparticle = _magicoverparticle.GetComponent<ParticleSystem>();
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
        if (_input.move != Vector2.zero && !_stopmovedir)
        {
            Quaternion targetRotation = Quaternion.LookRotation(_dir);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, Time.deltaTime * _turnSpeed);
        }

        if (_lookon && _input.move == Vector2.zero && !_stopmovedir)
        {
            var direction = _crosshaircanvas.transform.position - transform.position;
            direction.y = 0;

            var lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * _turnSpeed);
        }

        Vector3 velo = new Vector3(_rb.velocity.x, _rb.velocity.y, _rb.velocity.x);
        velo.y = _rb.velocity.y;
        _rb.velocity = velo;

        _isjump = _input.jump;
        TargetLookOn();
        Jump();
        Targets();
        MagicOverFlow();
    }
    private void LateUpdate()
    {
        _anim.SetBool("Grounded", IsGrounded());
        _anim.SetBool("Jump", _isjump);
        _anim.SetBool("Combo", _iscombo);
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
        AttackMotion();
    }

    void FixedUpdate()
    {
        Move();
    }

    void OnDrawGizmosSelected()
    {
        // 攻撃範囲を赤い線でシーンビューに表示する
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(GetTargetsRangeCenter(), _targetsRangeRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetAttackRangeCenter(), _attackRangeRadius);
    }
    Vector3 GetAttackRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _attackRangeCenter.z
            + this.transform.up * _attackRangeCenter.y
            + this.transform.right * _attackRangeCenter.x;
        return center;
    }
    Vector3 GetTargetsRangeCenter()
    {
        Vector3 center = this.transform.position + this.transform.forward * _gettargetsRangeCenter.z
            + this.transform.up * _gettargetsRangeCenter.y
            + this.transform.right * _gettargetsRangeCenter.x;
        return center;
    }
    void Attack()
    {
        var hit = Physics.OverlapSphere(GetAttackRangeCenter(), _attackRangeRadius);
        foreach (var c in hit)
        {
            EnemyR enemy = c.gameObject.GetComponent<EnemyR>();

            if (enemy)
            {
                enemy._ishit = true;
            }
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
    void Move()
    {

        float _targetSpeed = _input.sprint ? _dashmovePower : _movePower;

        Vector3 origin = this.transform.position + _rayForWall; // 原点
        if (_dir != Vector3.zero)
            raydir = _dir; // X軸方向を表すベクトル

        Ray ray = new Ray(origin, raydir); // Rayを生成
        Debug.DrawRay(ray.origin, ray.direction * _raydis, Color.red); // 長さ３０、赤色で可視化
        RaycastHit hit;

       
        if (Physics.Raycast(ray, out hit, _raydis) && !IsGrounded())
        {
            if (hit.collider.tag == "Ground")
            {
                _targetSpeed = 20f;
                _animationspeed = 20;
            }
        }
        // 「力を加える」処理は力学的処理なので FixedUpdate で行うこと
        if (!_stopmovedir && !_stopmove)
            _rb.AddForce(_dir.normalized * _targetSpeed, ForceMode.Force);

        if (_input.move == Vector2.zero)
        {
            _targetSpeed = 0.0f;
        }
        _animationspeed = Mathf.Lerp(_animationspeed, _targetSpeed, Time.deltaTime * 10f);

        _anim.SetFloat("Speed", _animationspeed);
    }
    void Avodance()
    {

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

            }
            target.targeton = true;
            if (_oncamerachangedir)
            {
                aim.m_HorizontalAxis.Value = _targetcamera.transform.localEulerAngles.y;
                aim.m_VerticalAxis.Value = _changeVerticalAxisValue;
                _oncamerachangedir = false;
            }
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
        _input.jump = false;
    }
    void AttackMotion()
    {
        _anim.SetBool("Punch", _input.attack);
        _input.attack = false;

        if (_magiclimiter < _magiclimit)
        {
            _anim.SetBool("Magic", _input.fire);
        }
        _input.fire = false;
    }
    void MagicOverFlow()
    {
        var emission = _overparticle.emission;

        if (_magiclimiter > 0)
        {
            _magiclimiter -= _magicCoolDownSpeed * Time.deltaTime;
        }
        else if (_magiclimiter < 0)
        {
            _magiclimiter = 0;
        }

        if (_magiclimiter >= 100f)
        {
            emission.rateOverTime = 300f;
        }
        else if (_magiclimiter >= 60f)
        {
            emission.rateOverTime = 100f;
        }
        else if (_magiclimiter >= 30f)
        {
            emission.rateOverTime = 10f;
        }
        else
        {
            emission.rateOverTime = 0f;
        }
    }
    void Magic()
    {
        Instantiate(_magiceff, _rightattackmuzzle.transform.position, this.transform.rotation);
        _magiclimiter += 13f;
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
            default:
                Debug.LogWarning("movenumが指定の範囲外です。Animationのイベントから指定してください。");
                break;
        }
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

    void DoCombo()
    {
        _iscombo = true;
    }
    void StopCombo()
    {
        _iscombo = false;
    }
}

