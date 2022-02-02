using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    [Header("攻撃")]
    [SerializeField] GameObject _magicoverparticle = default;
    ParticleSystem _overparticle = default;
    [SerializeField] GameObject _magicMuzzle = default;
    [SerializeField] GameObject _magiceff = default;
    [SerializeField] GameObject _impactEff = default;
    [SerializeField] GameObject _FireSEff = default;
    [SerializeField] GameObject _IceBommerEff = default;
    [SerializeField] GameObject _EarthSpikeEff = default;
    [SerializeField] GameObject _rightattackmuzzle = default;
    [SerializeField] GameObject _leftattackmuzzle = default;
    [SerializeField] GameObject _rockAimPoint = default;
    [SerializeField] GameObject _shootHitEff = default;
    [SerializeField] float _magicCoolDownSpeed = 2f;
    [SerializeField] float _magiclimiter = 0f;
    [SerializeField] float _magiclimit = 100f;
    [SerializeField] float shootInterval = 0.15f;
    float shootRange = 50;
    bool shooting = default;
    [SerializeField] int maxAmmo = 100;
    int ammo;
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


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _input = GetComponent<ControllerSystem>();
        _overparticle = _magicoverparticle.GetComponent<ParticleSystem>();
        Ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        AttackMotion();
        MagicOverFlow();
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
        _anim.SetInteger("MagicMode", (int)MagicMode);
    }
    public IEnumerator ShootTimer()
    {
        if (!shooting)
        {
            shooting = true;

            Shoot();
            yield return new WaitForSeconds(shootInterval);
            shooting = false;
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
        Debug.DrawRay(ray.origin, ray.direction * shootRange, Color.red);
        //レイを飛ばして、ヒットしたオブジェクトの情報を得る
        if (Physics.Raycast(ray, out hit, shootRange))
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
            var obj=　Instantiate(_shootHitEff, hit.point, this.transform.rotation);
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
                break;
            case 1:
                obj = Instantiate(_impactEff, _rightattackmuzzle.transform.position, this.transform.rotation);
                break;
            case 2:
                obj = Instantiate(_magiceff, _rightattackmuzzle.transform.position, this.transform.rotation);
                _magiclimiter += 13f;
                break;
            case 3:
                obj = Instantiate(_FireSEff, transform.position, Quaternion.identity);
                break;
            case 4:
                obj = Instantiate(_IceBommerEff, _rightattackmuzzle.transform.position, this.transform.rotation);
                break;
            case 5:
                obj = Instantiate(_EarthSpikeEff, _magicMuzzle.transform.position, this.transform.rotation);
                break;
            default:
                break;
        }

        obj.transform.parent = _world.transform;
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
