using UnityEngine.UI;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] bool _godMode = default;
    //最大HPと現在のHP。
    [SerializeField] float maxHp = 200;
    float currentHp = 200;
    [Tooltip("Playerが死亡する"), SerializeField]
    bool _isDead = false;
    //Sliderを入れる
    [SerializeField] public Slider slider;
    /// <summary>入力された方向の XZ 平面でのベクトル</summary>
    [SerializeField] GameObject _corpse = default;
    [SerializeField] GameObject _shieldEff = default;
    [SerializeField] GameObject _shieldBreakEff = default;
    [SerializeField] int _shieldCount = 0;
    [SerializeField] int _maxShield = 3;
    [SerializeField] float _heelShieldTimer = 0;
    [SerializeField] float _heelShieldlimitTime = 10f;
    float _slowtime = 0.3f;
    bool _justAvo = default;
    bool _isnotDamage = default;
    int _damageCut = 60;
    int _breakDamageCut = 20;
    bool _isShieldBreak = default;
    public bool IsShieldBreak { get => _isShieldBreak; set => _isShieldBreak = value; }
    public bool IsnotDamage { get => _isnotDamage; set => _isnotDamage = value; }

    PlayerController _playerCon;


    void Start()
    {
        _playerCon = GetComponent<PlayerController>();
        slider.value = 1;
        _shieldCount = _maxShield;
    }

    private void Update()
    {
        if (slider?.value <= 0 || _isDead)
        {
            _playerCon.TargetOff(slider.value);
            Instantiate(_corpse, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }

        if (currentHp >= maxHp)
            currentHp = maxHp;

        if (_shieldCount < _maxShield)
        {
            _heelShieldTimer += Time.deltaTime;
            if (_heelShieldTimer >= _heelShieldlimitTime)
            {
                _shieldCount++;
                _heelShieldTimer = 0;
            }
        }
        else if (_shieldCount == _maxShield && IsShieldBreak)
        {
            IsShieldBreak = false;
        }

    }

    //ColliderオブジェクトのIsTriggerにチェック入れること。
    private void OnTriggerEnter(Collider collision)
    {
        if ((collision.tag == "EnemyHit" || collision.tag == "BossTail") && _playerCon.IsAvo && !_isnotDamage && !_playerCon.RockGunOn)
        {
            _justAvo = true;
            Time.timeScale = _slowtime;
            StartCoroutine(_playerCon.DelayMethod(0.3f, () =>
            {
                _justAvo = false;
                Time.timeScale = 1;
            }));
        }
        else if ((collision.tag == "EnemyHit" || collision.tag == "BossTail") && !_justAvo && !_isnotDamage)
        {
            _playerCon.Ishit = true;
            _isnotDamage = true;
            if (collision.tag == "EnemyHit")
            {
                Damage(15, 21);
            }
            else if (collision.tag == "BossTail")
            {
                Damage(50, 66);
            }
            if (!_godMode)
            {
                if (_shieldCount > 0)
                {
                    Vector3 hitPos = collision.bounds.ClosestPoint(this.transform.position);
                    if ((collision.tag == "EnemyHit" || collision.tag == "BossTail") && !IsShieldBreak)
                    {
                        if (_shieldCount == 1)
                        {
                            IsShieldBreak = true;
                            Instantiate(_shieldBreakEff, hitPos, Quaternion.identity);
                        }
                        else if (_shieldCount <= _maxShield)
                        {
                            Instantiate(_shieldEff, hitPos, Quaternion.identity);
                        }
                        _shieldCount--;

                    }
                }
            }
        }

    }
    public void Damage(int min, int max)
    {
        if (slider && !_godMode)
        {
            float damage = Random.Range(min, max);

            //damageを何パーセントかカットする  割合 =（百分率 / 100）
            if (_shieldCount > 0 && !IsShieldBreak)
                damage -= damage * _damageCut / 100;
            else if (_shieldCount == 0)
                damage -= damage * _breakDamageCut / 100;

            currentHp = currentHp - (int)damage;
            float value = (float)currentHp / (float)maxHp;
            DOTween.To(() => slider.value, x => slider.value = x, value, 0.5f);
        }
    }
    public void Heel()
    {
        currentHp = Mathf.Min(maxHp, currentHp + 50);
        slider.value = (float)currentHp / (float)maxHp;
    }
}
