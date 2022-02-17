using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    [Header("攻撃")]
    [SerializeField] GameObject _magicoverparticle = default;
    ParticleSystem _overparticle = default;
    [SerializeField] GameObject _magicMuzzle = default;
    [SerializeField] GameObject _IceBallEff = default;
    [SerializeField] GameObject _impactEff = default;
    [SerializeField] GameObject _FireSEff = default;
    [SerializeField] GameObject _IceLanceEff = default;
    [SerializeField] GameObject _EarthSpikeEff = default;
    [SerializeField] GameObject _rightattackmuzzle = default;
    [SerializeField] GameObject _leftattackmuzzle = default;
    [SerializeField] GameObject _rockAimPoint = default;
    [SerializeField] GameObject _shootHitEff = default;
    [SerializeField] float _magicCoolDownSpeed = 2f;
    [SerializeField]float _magiclimiter = 0f;
    [SerializeField] float _magiclimit = 100f;
    [SerializeField] float _coolDownTimer = 0;
    [SerializeField] float _magicCoolDown = 5f;
    [SerializeField] float _shootInterval = 0.15f;
    float _shootDis = 50;
    bool _isshooting = default;
    float _reloadTimer;
    bool _isReloadCool = false;
    public bool IsReloadCool { get => _isReloadCool; set => _isReloadCool = value; }
    [SerializeField] float _reloadTime = 3f;
    [SerializeField] int maxAmmo = 100;
    [SerializeField] int ammo;
    [SerializeField] GameObject _world = default;
    public int Ammo
    {
        set
        {
            ammo = Mathf.Clamp(value, 0, maxAmmo);
        }
        get
        {
            return ammo;
        }
    }
    bool _iscombo = default;
    public bool Iscombo { get => _iscombo; set => _iscombo = value; }

    public enum Action
    {
        Fire,
        Ice,
        Earth
    }
    [SerializeField] private Action _magicMode;
    public Action MagicMode { get => _magicMode; set => _magicMode = value; }

    Animator _anim = default;
    ControllerSystem _input;
    PlayerAudio _paudio;


    // Start is called before the first frame update
    void Start()
    {
        _paudio = GetComponent<PlayerAudio>();
        _anim = GetComponent<Animator>();
        _input = GetComponent<ControllerSystem>();
        _overparticle = _magicoverparticle.GetComponent<ParticleSystem>();
        Ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsReloadCool && Ammo == 0)
        {
            IsReloadCool = true;
        }
        else if (IsReloadCool && Ammo == maxAmmo)
        {
            IsReloadCool = false;
        }

        if (Ammo < maxAmmo && !_input.shoot || Ammo < maxAmmo && IsReloadCool)
        {
            _reloadTimer += Time.deltaTime;
            if (_reloadTimer >= _reloadTime)
            {
                Ammo++;
                _reloadTimer = 0;
            }
        }

        if (_coolDownTimer > 0)
        {
            _coolDownTimer -= _magicCoolDownSpeed * Time.deltaTime;
        }
        else if (_coolDownTimer < 0)
        {
            _coolDownTimer = 0;
        }
        
        MagicOverFlow();
    }
    private void LateUpdate()
    {
        AttackMotion();
    }
    void AttackMotion()
    {
        _anim.SetInteger("MagicMode", (int)MagicMode);
        _anim.SetBool("Punch", _input.attack);
        if (_magiclimiter < _magiclimit && _coolDownTimer <= 0)
        {
            _anim.SetBool("Magic", _input.fire);
        }
        else
        {
            _anim.SetBool("Magic", false);
        }

        if (_input.attack)
        {
            _input.attack = false;
        }
        if (_input.fire)
        {
            _input.fire = false;
        }
    }
    public IEnumerator ShootTimer()
    {
        if (!_isshooting)
        {
            _isshooting = true;
            if (!IsReloadCool)
            {
                MPCost(2);
                _paudio.RockGun();
                Shoot();
            }
            else
            {
                _paudio.NotGunBullet();
            }

            yield return new WaitForSeconds(_shootInterval);
            _isshooting = false;
        }
        else
        {
            yield return null;
        }
    }
    void Shoot()
    {
        Ray ray = new Ray(_rockAimPoint.transform.position, _rockAimPoint.transform.forward);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * _shootDis, Color.red);
        //レイを飛ばして、ヒットしたオブジェクトの情報を得る
        if (Physics.Raycast(ray, out hit, _shootDis))
        {
            //ヒットエフェクトON
            if (hit.collider.tag == "Enemy")
            {
                EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();

                if (enemy)
                {
                    enemy._stateMode = EnemyBase.State.SHit;
                }
            }
            var obj = Instantiate(_shootHitEff, hit.point, this.transform.rotation);
            obj.transform.parent = _world.transform;
            //★ここに敵へのダメージ処理などを追加
        }
        Ammo--;
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
    void Magic(int magics)
    {
        GameObject obj = null;

        switch (magics)
        {
            case 0:
                obj = Instantiate(_impactEff, _leftattackmuzzle.transform.position, this.transform.rotation);
                MPCost(5);
                break;
            case 1:
                obj = Instantiate(_impactEff, _rightattackmuzzle.transform.position, this.transform.rotation);
                MPCost(5);
                break;
            case 2:
                obj = Instantiate(_IceBallEff, _rightattackmuzzle.transform.position, this.transform.rotation);
                MPCost(30);
                CoolDown(30);
                break;
            case 3:
                obj = Instantiate(_FireSEff, transform.position, Quaternion.identity);
                MPCost(20);
                CoolDown(15);
                break;
            case 4:
                obj = Instantiate(_IceLanceEff, _rightattackmuzzle.transform.position, this.transform.rotation);
                MPCost(10);
                break;
            case 5:
                obj = Instantiate(_EarthSpikeEff, _magicMuzzle.transform.position, this.transform.rotation);
                MPCost(30);
                CoolDown(15);
                break;
            default:
                break;
        }

        obj.transform.parent = _world.transform;
    }
    float MPCost(float cost)
    {
        return _magiclimiter += cost;
    }

    void CoolDown(float cost)
    {
        _coolDownTimer = cost;
    }
    void ConboSwith(int OnOff)
    {
        switch (OnOff)
        {
            case 1:
                _iscombo = true;
                break;
            case 2:
                _iscombo = false;
                break;
            default:
                Debug.LogWarning("指定の範囲外です。Animationのイベントから指定してください。");
                break;
        }
    }
}
